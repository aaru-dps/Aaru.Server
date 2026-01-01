// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Compare.razor.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ License ] --------------------------------------------------------------
//
//     This library is free software; you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as
//     published by the Free Software Foundation; either version 2.1 of the
//     License, or (at your option) any later version.
//
//     This library is distributed in the hope that it will be useful, but
//     WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//     Lesser General Public License for more details.
//
//     You should have received a copy of the GNU Lesser General Public
//     License along with this library; if not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright © 2011-2026 Natalia Portillo
// ****************************************************************************/

using System.Reflection;
using Aaru.CommonTypes.Structs.Devices.SCSI;
using Aaru.Helpers;
using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Scsi;

public partial class Compare
{
    Modal?       _consolidateModal;
    int          _deleteId;
    Modal?       _deleteModal;
    bool         _initialized;
    int          _masterId;
    int          _slaveId;
    CompareModel Model;
    [Parameter]
    public int Id { get; set; }
    [Parameter]
    public int CompareId { get; set; }
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        Model = new CompareModel
        {
            LeftId  = Id,
            RightId = CompareId
        };

        CommonTypes.Metadata.Scsi? left  = ctx.Scsi.FirstOrDefault(l => l.Id == Id);
        CommonTypes.Metadata.Scsi? right = ctx.Scsi.FirstOrDefault(r => r.Id == CompareId);

        if(left is null)
        {
            Model.ErrorMessage = $"SCSI with id {Id} has not been found";
            Model.HasError     = true;

            return;
        }

        if(right is null)
        {
            Model.ErrorMessage = $"SCSI with id {CompareId} has not been found";
            Model.HasError     = true;

            return;
        }

        Inquiry? leftNullable  = left.Inquiry;
        Inquiry? rightNullable = right.Inquiry;
        Model.ValueNames  = [];
        Model.LeftValues  = [];
        Model.RightValues = [];

        if(leftNullable == null && rightNullable == null)
        {
            Model.AreEqual = true;

            return;
        }

        if(leftNullable != null && rightNullable == null)
        {
            Model.ValueNames.Add("Decoded");
            Model.LeftValues.Add("decoded");
            Model.RightValues.Add("null");

            return;
        }

        if(leftNullable == null)
        {
            Model.ValueNames.Add("Decoded");
            Model.LeftValues.Add("null");
            Model.RightValues.Add("decoded");

            return;
        }

        var leftValue  = (Inquiry)left.Inquiry!;
        var rightValue = (Inquiry)right.Inquiry!;

        foreach(FieldInfo fieldInfo in leftValue.GetType().GetFields())
        {
            object lv = fieldInfo.GetValue(leftValue);
            object rv = fieldInfo.GetValue(rightValue);

            if(fieldInfo.FieldType.IsArray)
            {
                var la = lv as Array;
                var ra = rv as Array;

                switch(la)
                {
                    case null when ra is null:
                        continue;
                    case null:
                        Model.ValueNames.Add(fieldInfo.Name);
                        Model.LeftValues.Add("null");
                        Model.RightValues.Add("[]");

                        continue;
                }

                if(ra is null)
                {
                    Model.ValueNames.Add(fieldInfo.Name);
                    Model.LeftValues.Add("[]");
                    Model.RightValues.Add("null");

                    continue;
                }

                var ll = la.Cast<object>().ToList();
                var rl = ra.Cast<object>().ToList();

                for(int i = 0; i < ll.Count; i++)
                {
                    if(ll[i].Equals(rl[i])) continue;

                    switch(fieldInfo.Name)
                    {
                        case nameof(Inquiry.KreonIdentifier):
                        case nameof(Inquiry.ProductIdentification):
                        case nameof(Inquiry.ProductRevisionLevel):
                        case nameof(Inquiry.Qt_ModuleRevision):
                        case nameof(Inquiry.Seagate_Copyright):
                        case nameof(Inquiry.Seagate_DriveSerialNumber):
                        case nameof(Inquiry.Seagate_ServoPROMPartNo):
                        case nameof(Inquiry.VendorIdentification):
                            byte[] lb = new byte[ll.Count];
                            byte[] rb = new byte[rl.Count];

                            for(int j = 0; j < ll.Count; j++) lb[j] = (byte)ll[j];

                            for(int j = 0; j < ll.Count; j++) rb[j] = (byte)rl[j];

                            Model.ValueNames.Add(fieldInfo.Name);
                            Model.LeftValues.Add($"{StringHandlers.CToString(lb)  ?? "<null>"}");
                            Model.RightValues.Add($"{StringHandlers.CToString(rb) ?? "<null>"}");

                            break;

                        default:
                            Model.ValueNames.Add(fieldInfo.Name);
                            Model.LeftValues.Add("[]");
                            Model.RightValues.Add("[]");

                            break;
                    }

                    break;
                }
            }
            else if(lv == null && rv == null) {}
            else if(lv != null && rv == null)
            {
                Model.ValueNames.Add(fieldInfo.Name);
                Model.LeftValues.Add($"{lv}");
                Model.RightValues.Add("null");
            }
            else if(lv == null)
            {
                Model.ValueNames.Add(fieldInfo.Name);
                Model.LeftValues.Add("null");
                Model.RightValues.Add($"{rv}");
            }
            else if(!lv.Equals(rv))

            {
                Model.ValueNames.Add(fieldInfo.Name);
                Model.LeftValues.Add($"{lv}");
                Model.RightValues.Add($"{rv}");
            }
        }

        Model.AreEqual = Model.LeftValues.Count == 0 && Model.RightValues.Count == 0;

        _initialized = true;

        StateHasChanged();
    }

    private async Task ShowDeleteModal(int id)
    {
        _deleteId = id;
        if(_deleteModal != null) await _deleteModal.ShowAsync();
    }

    private async Task HideDeleteModal()
    {
        if(_deleteModal != null) await _deleteModal.HideAsync();
    }

    private async Task ConfirmDelete()
    {
        await DeleteAsync(_deleteId);
        await HideDeleteModal();
        NavigationManager.NavigateTo("/admin/scsi");
    }

    private async Task DeleteAsync(int id)
    {
        await using DbContext      ctx  = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.Scsi? scsi = await ctx.Scsi.FindAsync(id);

        if(scsi is not null)
        {
            ctx.Scsi.Remove(scsi);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task ShowConsolidateModal(int masterId, int slaveId)
    {
        _masterId = masterId;
        _slaveId  = slaveId;
        if(_consolidateModal != null) await _consolidateModal.ShowAsync();
    }

    private async Task HideConsolidateModal()
    {
        if(_consolidateModal != null) await _consolidateModal.HideAsync();
    }

    private async Task ConfirmConsolidate()
    {
        await ConsolidateAsync(_masterId, _slaveId);
        await HideConsolidateModal();
        NavigationManager.NavigateTo($"/admin/scsi/{_masterId}");
    }

    private async Task ConsolidateAsync(int masterId, int slaveId)
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        CommonTypes.Metadata.Scsi? master = ctx.Scsi.Include(static scsi => scsi.ReadCapabilities)
                                               .FirstOrDefault(m => m.Id == masterId);

        if(master is null) return;

        CommonTypes.Metadata.Scsi? slave = ctx.Scsi.Include(static scsi => scsi.ReadCapabilities)
                                              .FirstOrDefault(m => m.Id == slaveId);

        if(slave is null) return;

        foreach(Device scsiDevice in ctx.Devices.Where(d => d.SCSI.Id == slaveId)) scsiDevice.SCSI = master;

        foreach(UploadedReport scsiReport in ctx.Reports.Where(d => d.SCSI.Id == slaveId)) scsiReport.SCSI = master;

        foreach(CommonTypes.Metadata.TestedMedia testedMedia in ctx.TestedMedia.Where(d => d.ScsiId == slaveId))
        {
            testedMedia.ScsiId = masterId;
            ctx.Update(testedMedia);
        }

        if(master.ReadCapabilities is null && slave.ReadCapabilities != null)
            master.ReadCapabilities = slave.ReadCapabilities;

        ctx.Scsi.Remove(slave);

        await ctx.SaveChangesAsync();
    }
}