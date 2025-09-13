using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Reports;

public partial class Details
{
    bool                  _initialized;
    UploadedReportDetails _model;
    [Parameter]
    public int Id { get; set; }
    [Inject]
    NavigationManager Navigator { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _model = new UploadedReportDetails
        {
            Report = await ctx.Reports.Include(static deviceReport => deviceReport.ATAPI)
                              .Include(static deviceReport => deviceReport.ATA)
                              .ThenInclude(static ata => ata.ReadCapabilities)
                              .Include(static deviceReport => deviceReport.SCSI)
                              .ThenInclude(static scsi => scsi.ReadCapabilities)
                              .Include(static deviceReport => deviceReport.SCSI)
                              .ThenInclude(static scsi => scsi.MultiMediaDevice)
                              .Include(static deviceReport => deviceReport.SCSI)
                              .ThenInclude(static scsi => scsi.SequentialDevice)
                              .FirstOrDefaultAsync(m => m.Id == Id)
        };

        if(_model.Report is null) return;

        _model.ReportAll = await ctx.Devices
                                    .Where(d => d.Manufacturer == _model.Report.Manufacturer &&
                                                d.Model        == _model.Report.Model        &&
                                                d.Revision     == _model.Report.Revision)
                                    .Select(static d => d.Id)
                                    .ToListAsync();

        _model.ReportButManufacturer = await ctx.Devices
                                                .Where(d => d.Model    == _model.Report.Model &&
                                                            d.Revision == _model.Report.Revision)
                                                .Select(static d => d.Id)
                                                .Where(d => _model.ReportAll.All(r => r != d))
                                                .ToListAsync();

        _model.SameAll = await ctx.Reports
                                  .Where(d => d.Manufacturer == _model.Report.Manufacturer &&
                                              d.Model        == _model.Report.Model        &&
                                              d.Revision     == _model.Report.Revision     &&
                                              d.Id           != Id)
                                  .Select(static d => d.Id)
                                  .ToListAsync();

        _model.SameButManufacturer = await ctx.Reports
                                              .Where(d => d.Model    == _model.Report.Model    &&
                                                          d.Revision == _model.Report.Revision &&
                                                          d.Id       != Id)
                                              .Select(static d => d.Id)
                                              .Where(d => _model.SameAll.All(r => r != d))
                                              .ToListAsync();

        _model.ReadCapabilitiesId =
            _model.Report.ATA?.ReadCapabilities?.Id ?? _model.Report.SCSI?.ReadCapabilities?.Id ?? 0;

        // So we can check, as we know IDs with 0 will never exist, and EFCore does not allow null propagation in the LINQ
        int ataId   = _model.Report.ATA?.Id                    ?? 0;
        int atapiId = _model.Report.ATAPI?.Id                  ?? 0;
        int scsiId  = _model.Report.SCSI?.Id                   ?? 0;
        int mmcId   = _model.Report.SCSI?.MultiMediaDevice?.Id ?? 0;
        int sscId   = _model.Report.SCSI?.SequentialDevice?.Id ?? 0;

        _model.TestedMedias = await ctx.TestedMedia
                                       .Where(t => t.AtaId  == ataId   ||
                                                   t.AtaId  == atapiId ||
                                                   t.ScsiId == scsiId  ||
                                                   t.MmcId  == mmcId)
                                       .OrderBy(static t => t.Manufacturer)
                                       .ThenBy(static t => t.Model)
                                       .ThenBy(static t => t.MediumTypeName)
                                       .ToListAsync();

        _model.TestedSequentialMedias = await ctx.TestedSequentialMedia.Where(t => t.SscId == sscId)
                                                 .OrderBy(static t => t.Manufacturer)
                                                 .ThenBy(static t => t.Model)
                                                 .ThenBy(static t => t.MediumTypeName)
                                                 .ToListAsync();

        _initialized = true;
        StateHasChanged();
    }


    async Task Promote()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        UploadedReport? uploadedReport = await ctx.Reports.FirstOrDefaultAsync(m => m.Id == Id);

        if(uploadedReport == null) return;

        var device = new Device(uploadedReport.ATAId,
                                uploadedReport.ATAPIId,
                                uploadedReport.FireWireId,
                                uploadedReport.MultiMediaCardId,
                                uploadedReport.PCMCIAId,
                                uploadedReport.SecureDigitalId,
                                uploadedReport.SCSIId,
                                uploadedReport.USBId,
                                uploadedReport.UploadedWhen,
                                uploadedReport.Manufacturer,
                                uploadedReport.Model,
                                uploadedReport.Revision,
                                uploadedReport.CompactFlash,
                                uploadedReport.Type,
                                uploadedReport.GdRomSwapDiscCapabilitiesId);

        EntityEntry<Device> res = ctx.Devices.Add(device);
        ctx.Reports.Remove(uploadedReport);
        await ctx.SaveChangesAsync();

        Navigator.NavigateTo($"/admin/devices/{res.Entity.Id}");
    }

    async Task Merge(int? master, int? slave)
    {
        if(master is null || slave is null) return;

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        UploadedReport? masterReport = await ctx.Reports.Include(static deviceReport => deviceReport.SCSI)
                                                .FirstOrDefaultAsync(m => m.Id == master);

        UploadedReport? slaveReport = await ctx.Reports.Include(static deviceReport => deviceReport.SCSI)
                                               .FirstOrDefaultAsync(m => m.Id == slave);

        if(masterReport is null || slaveReport is null) return;

        if(masterReport.ATAId != null && masterReport.ATAId != slaveReport.ATAId)
        {
            foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                    ctx.TestedMedia.Where(d => d.AtaId == slaveReport.ATAId))
            {
                testedMedia.AtaId = masterReport.ATAId;
                ctx.Update(testedMedia);
            }
        }
        else if(masterReport.ATAId == null && slaveReport.ATAId != null)
        {
            masterReport.ATAId = slaveReport.ATAId;
            ctx.Update(masterReport);
        }

        if(masterReport.ATAPIId != null && masterReport.ATAPIId != slaveReport.ATAPIId)
        {
            foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                    ctx.TestedMedia.Where(d => d.AtaId == slaveReport.ATAPIId))
            {
                testedMedia.AtaId = masterReport.ATAPIId;
                ctx.Update(testedMedia);
            }
        }
        else if(masterReport.ATAPIId == null && slaveReport.ATAPIId != null)
        {
            masterReport.ATAPIId = slaveReport.ATAPIId;
            ctx.Update(masterReport);
        }

        if(masterReport.SCSIId != null && masterReport.SCSIId != slaveReport.SCSIId)
        {
            foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                    ctx.TestedMedia.Where(d => d.ScsiId == slaveReport.SCSIId))
            {
                testedMedia.ScsiId = masterReport.SCSIId;
                ctx.Update(testedMedia);
            }
        }
        else if(masterReport.SCSIId == null && slaveReport.SCSIId != null)
        {
            masterReport.SCSIId = slaveReport.SCSIId;
            ctx.Update(masterReport);
        }

        if(masterReport.SCSI?.SequentialDeviceId != null &&
           masterReport.SCSI?.SequentialDeviceId != slaveReport.SCSI?.SequentialDeviceId)
        {
            foreach(TestedSequentialMedia testedMedia in
                    ctx.TestedSequentialMedia.Where(d => slaveReport.SCSI != null &&
                                                         d.SscId          == slaveReport.SCSI.SequentialDeviceId))
            {
                if(masterReport.SCSI != null) testedMedia.SscId = masterReport.SCSI.SequentialDeviceId;
                ctx.Update(testedMedia);
            }
        }
        else if(masterReport.SCSI is { SequentialDeviceId: null } && slaveReport.SCSI?.SequentialDeviceId != null)
        {
            if(masterReport.SCSI != null) masterReport.SCSI.SequentialDeviceId = slaveReport.SCSI.SequentialDeviceId;
            ctx.Update(masterReport);
        }

        if(masterReport.GdRomSwapDiscCapabilitiesId == null && slaveReport.GdRomSwapDiscCapabilitiesId != null ||
           masterReport.GdRomSwapDiscCapabilitiesId != null && slaveReport.GdRomSwapDiscCapabilitiesId != null)
        {
            masterReport.GdRomSwapDiscCapabilitiesId = slaveReport.GdRomSwapDiscCapabilitiesId;
            ctx.Update(masterReport);
        }

        ctx.Remove(slaveReport);
        await ctx.SaveChangesAsync();

        Navigator.Refresh(true);
    }

    async Task MergeReports(int? deviceId, int? reportId)
    {
        if(deviceId is null || reportId is null) return;

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        Device? device = await ctx.Devices.Include(static deviceReport => deviceReport.ATA)
                                  .ThenInclude(static ata => ata.ReadCapabilities)
                                  .Include(static deviceReport => deviceReport.SCSI)
                                  .ThenInclude(static scsi => scsi.ReadCapabilities)
                                  .Include(static deviceReport => deviceReport.SCSI)
                                  .ThenInclude(static scsi => scsi.MultiMediaDevice)
                                  .Include(static deviceReport => deviceReport.SCSI)
                                  .ThenInclude(static scsi => scsi.SequentialDevice)
                                  .FirstOrDefaultAsync(m => m.Id == deviceId);

        UploadedReport? report = await ctx.Reports.Include(static deviceReport => deviceReport.ATA)
                                          .ThenInclude(static ata => ata.ReadCapabilities)
                                          .Include(static deviceReport => deviceReport.SCSI)
                                          .ThenInclude(static scsi => scsi.ReadCapabilities)
                                          .Include(static deviceReport => deviceReport.SCSI)
                                          .ThenInclude(static scsi => scsi.MultiMediaDevice)
                                          .Include(static deviceReport => deviceReport.SCSI)
                                          .ThenInclude(static scsi => scsi.SequentialDevice)
                                          .FirstOrDefaultAsync(m => m.Id == reportId);

        if(device?.ATAId != null && device.ATAId != report?.ATAId)
        {
            foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                    ctx.TestedMedia.Where(d => report != null && d.AtaId == report.ATAId))
            {
                testedMedia.AtaId = device.ATAId;
                ctx.Update(testedMedia);
            }

            if(device.ATA is { ReadCapabilities: null } && report?.ATA?.ReadCapabilities != null)
            {
                device.ATA.ReadCapabilities = report.ATA.ReadCapabilities;
                ctx.Update(device.ATA);
            }
        }
        else if(device?.ATAId == null && report?.ATAId != null)
        {
            if(device != null)
            {
                device.ATAId = report.ATAId;
                ctx.Update(device);
            }
        }

        switch(device)
        {
            case { ATAPIId: not null } when device.ATAPIId != report?.ATAPIId:
            {
                foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                        ctx.TestedMedia.Where(d => report != null && d.AtaId == report.ATAPIId))
                {
                    testedMedia.AtaId = device.ATAPIId;
                    ctx.Update(testedMedia);
                }

                break;
            }
            case { ATAPIId: null } when report?.ATAPIId != null:
                device.ATAPIId = report.ATAPIId;
                ctx.Update(device);

                break;
        }

        switch(device)
        {
            case { SCSIId: not null } when device.SCSIId != report?.SCSIId:
            {
                foreach(CommonTypes.Metadata.TestedMedia testedMedia in ctx.TestedMedia.Where(d => report != null &&
                            d.ScsiId == report.SCSIId))
                {
                    testedMedia.ScsiId = device.SCSIId;
                    ctx.Update(testedMedia);
                }

                if(device.SCSI is { ReadCapabilities: null } && report?.SCSI?.ReadCapabilities != null)
                {
                    device.SCSI.ReadCapabilities = report.SCSI.ReadCapabilities;
                    ctx.Update(device.SCSI);
                }

                if(device.SCSI is { MultiMediaDevice: null } && report?.SCSI?.MultiMediaDevice != null)
                {
                    device.SCSI.MultiMediaDevice = report.SCSI.MultiMediaDevice;
                    ctx.Update(device.SCSI);
                }
                else if(device.SCSI?.MultiMediaDevice != null && report?.SCSI?.MultiMediaDevice != null)
                {
                    foreach(CommonTypes.Metadata.TestedMedia testedMedia in ctx.TestedMedia.Where(d => d.MmcId ==
                                report.SCSI.MultiMediaDevice.Id))
                    {
                        testedMedia.MmcId = device.SCSI.MultiMediaDevice.Id;
                        ctx.Update(testedMedia);
                    }
                }

                if(device.SCSI is { SequentialDevice: null } && report?.SCSI?.SequentialDevice != null)
                {
                    device.SCSI.SequentialDevice = report.SCSI.SequentialDevice;
                    ctx.Update(device.SCSI);
                }
                else if(device.SCSI?.SequentialDevice != null && report?.SCSI?.SequentialDevice != null)
                {
                    foreach(TestedSequentialMedia testedSequentialMedia in
                            ctx.TestedSequentialMedia.Where(d => d.SscId == report.SCSI.SequentialDevice.Id))
                    {
                        testedSequentialMedia.SscId = device.SCSI.SequentialDevice.Id;
                        ctx.Update(testedSequentialMedia);
                    }
                }

                break;
            }
            case { SCSIId: null } when report?.SCSIId != null:
                device.SCSIId = report.SCSIId;
                ctx.Update(device);

                break;
        }

        ctx.Remove(report);
        await ctx.SaveChangesAsync();

        Navigator.Refresh(true);
    }
}