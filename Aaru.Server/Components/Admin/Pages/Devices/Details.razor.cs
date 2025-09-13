using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Devices;

public partial class Details
{
    bool          _initialized;
    bool          _notFound;
    DeviceDetails model;
    [Parameter]
    public int Id { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await RefreshItemsAsync();
    }

    async Task RefreshItemsAsync()
    {
        _initialized = false;
        _notFound    = false;

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        model = new DeviceDetails
        {
            Report = await ctx.Devices.Include(static deviceReport => deviceReport.ATAPI)
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

        if(model.Report is null)
        {
            _initialized = true;
            _notFound    = true;
            StateHasChanged();

            return;
        }

        model.ReportAll = await ctx.Reports
                                   .Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                               d.Model        == model.Report.Model        &&
                                               d.Revision     == model.Report.Revision)
                                   .Select(static d => d.Id)
                                   .ToListAsync();

        model.ReportButManufacturer = await ctx.Reports
                                               .Where(d => d.Model    == model.Report.Model &&
                                                           d.Revision == model.Report.Revision)
                                               .Select(static d => d.Id)
                                               .Where(d => model.ReportAll.All(r => r != d))
                                               .ToListAsync();

        model.SameAll = await ctx.Devices
                                 .Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                             d.Model        == model.Report.Model        &&
                                             d.Revision     == model.Report.Revision     &&
                                             d.Id           != Id)
                                 .Select(static d => d.Id)
                                 .ToListAsync();

        model.SameButManufacturer = await ctx.Devices
                                             .Where(d => d.Model    == model.Report.Model    &&
                                                         d.Revision == model.Report.Revision &&
                                                         d.Id       != Id)
                                             .Select(static d => d.Id)
                                             .Where(d => model.SameAll.All(r => r != d))
                                             .ToListAsync();

        model.StatsAll = await ctx.DeviceStats
                                  .Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                              d.Model        == model.Report.Model        &&
                                              d.Revision     == model.Report.Revision     &&
                                              d.Report       != null                      &&
                                              d.Report.Id    != model.Report.Id)
                                  .ToListAsync();

        model.StatsButManufacturer = ctx.DeviceStats
                                        .Where(d => d.Model     == model.Report.Model    &&
                                                    d.Revision  == model.Report.Revision &&
                                                    d.Report    != null                  &&
                                                    d.Report.Id != model.Report.Id)
                                        .AsEnumerable()
                                        .Where(d => model.StatsAll.All(s => s.Id != d.Id))
                                        .ToList();

        model.ReadCapabilitiesId =
            model.Report.ATA?.ReadCapabilities?.Id ?? model.Report.SCSI?.ReadCapabilities?.Id ?? 0;

        // So we can check, as we know IDs with 0 will never exist, and EFCore does not allow null propagation in the LINQ
        int ataId   = model.Report.ATA?.Id                    ?? 0;
        int atapiId = model.Report.ATAPI?.Id                  ?? 0;
        int scsiId  = model.Report.SCSI?.Id                   ?? 0;
        int mmcId   = model.Report.SCSI?.MultiMediaDevice?.Id ?? 0;
        int sscId   = model.Report.SCSI?.SequentialDevice?.Id ?? 0;

        model.TestedMedias = await ctx.TestedMedia
                                      .Where(t => t.AtaId  == ataId   ||
                                                  t.AtaId  == atapiId ||
                                                  t.ScsiId == scsiId  ||
                                                  t.MmcId  == mmcId)
                                      .OrderBy(static t => t.Manufacturer)
                                      .ThenBy(static t => t.Model)
                                      .ThenBy(static t => t.MediumTypeName)
                                      .ToListAsync();

        model.TestedSequentialMedias = await ctx.TestedSequentialMedia.Where(t => t.SscId == sscId)
                                                .OrderBy(static t => t.Manufacturer)
                                                .ThenBy(static t => t.Model)
                                                .ThenBy(static t => t.MediumTypeName)
                                                .ToListAsync();

        _initialized = true;
        StateHasChanged();
    }

    async Task Merge(int master, int slave)
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        Device? masterDevice = await ctx.Devices.FirstOrDefaultAsync(m => m.Id == master);
        Device? slaveDevice  = await ctx.Devices.FirstOrDefaultAsync(m => m.Id == slave);

        if(masterDevice is null || slaveDevice is null) return;

        if(masterDevice.ATAId != null && masterDevice.ATAId != slaveDevice.ATAId)
        {
            foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                    ctx.TestedMedia.Where(d => d.AtaId == slaveDevice.ATAId))
            {
                testedMedia.AtaId = masterDevice.ATAId;
                ctx.Update(testedMedia);
            }
        }
        else if(masterDevice.ATAId == null && slaveDevice.ATAId != null)
        {
            masterDevice.ATAId = slaveDevice.ATAId;
            ctx.Update(masterDevice);
        }

        if(masterDevice.ATAPIId != null && masterDevice.ATAPIId != slaveDevice.ATAPIId)
        {
            foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                    ctx.TestedMedia.Where(d => d.AtaId == slaveDevice.ATAPIId))
            {
                testedMedia.AtaId = masterDevice.ATAPIId;
                ctx.Update(testedMedia);
            }
        }
        else if(masterDevice.ATAPIId == null && slaveDevice.ATAPIId != null)
        {
            masterDevice.ATAPIId = slaveDevice.ATAPIId;
            ctx.Update(masterDevice);
        }

        if(masterDevice.SCSIId != null && masterDevice.SCSIId != slaveDevice.SCSIId)
        {
            foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                    ctx.TestedMedia.Where(d => d.ScsiId == slaveDevice.SCSIId))
            {
                testedMedia.ScsiId = masterDevice.SCSIId;
                ctx.Update(testedMedia);
            }
        }
        else if(masterDevice.SCSIId == null && slaveDevice.SCSIId != null)
        {
            masterDevice.SCSIId = slaveDevice.SCSIId;
            ctx.Update(masterDevice);
        }

        masterDevice.ModifiedWhen = DateTime.UtcNow;
        ctx.Update(masterDevice);
        ctx.Remove(slaveDevice);
        await ctx.SaveChangesAsync();

        Id = master;

        await RefreshItemsAsync();
    }

    async Task MergeReports(int deviceId, int reportId)
    {
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

        Id = deviceId;
        await RefreshItemsAsync();
    }

    async Task LinkReports(int deviceId, int statsId)
    {
        await using DbContext ctx    = await DbContextFactory.CreateDbContextAsync();
        Device?               device = await ctx.Devices.FirstOrDefaultAsync(m => m.Id     == deviceId);
        DeviceStat?           stat   = await ctx.DeviceStats.FirstOrDefaultAsync(m => m.Id == statsId);

        if(stat != null)
        {
            stat.Report = device;
            ctx.Update(stat);
        }

        await ctx.SaveChangesAsync();
        Id = deviceId;
        await RefreshItemsAsync();
    }
}