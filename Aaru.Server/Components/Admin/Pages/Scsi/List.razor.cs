// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : List.razor.cs
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

using Aaru.Helpers;
using Aaru.Server.Core;
using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Scsi;

public partial class List
{
    private Modal?                          _consolidateModal;
    private int                             _deleteId;
    private Modal?                          _deleteModal;
    private List<IdHashModel?>              _duplicates = new();
    private bool                            _initialized;
    private List<CommonTypes.Metadata.Scsi> _items      = new();
    private string                          _searchTerm = string.Empty;

    private IEnumerable<CommonTypes.Metadata.Scsi> FilteredItems
    {
        get
        {
            if(string.IsNullOrWhiteSpace(_searchTerm)) return _items;

            return _items.Where(item =>
                                    (StringHandlers.CToString(item.Inquiry?.VendorIdentification)
                                                  ?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    (StringHandlers.CToString(item.Inquiry?.ProductIdentification)
                                                  ?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    (StringHandlers.CToString(item.Inquiry?.ProductRevisionLevel)
                                                  ?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    item.Id.ToString().Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }


    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await RefreshItemsAsync();

        _initialized = true;

        StateHasChanged();
    }

    async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = ctx.Scsi.AsEnumerable()
                    .OrderBy(static m => StringHandlers.CToString(m.Inquiry?.VendorIdentification))
                    .ThenBy(static m => StringHandlers.CToString(m.Inquiry?.ProductIdentification))
                    .ThenBy(static m => StringHandlers.CToString(m.Inquiry?.ProductRevisionLevel))
                    .ToList();

        List<IdHashModel> hashes = await ctx.Scsi.Where(static m => m.InquiryData != null)
                                            .Select(static m => new IdHashModel
                                             {
                                                 Id      = m.Id,
                                                 Hash    = Hash.Sha512(m.InquiryData),
                                                 Inquiry = m.Inquiry
                                             })
                                            .ToListAsync();

        _duplicates = hashes.GroupBy(static x => x.Hash)
                            .Where(static g => g.Count() > 1)
                            .Select(x => hashes.FirstOrDefault(y => y.Hash == x.Key))
                            .OrderBy(static d => d?.Description)
                            .ToList();
    }

    Task ConsolidateDuplicatesAsync() => _consolidateModal?.ShowAsync();

    Task HideConsolidateModalAsync() => _consolidateModal?.HideAsync();

    async Task ConfirmConsolidateAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(IdHashModel duplicate in _duplicates)
        {
            CommonTypes.Metadata.Scsi? master = ctx.Scsi.FirstOrDefault(m => duplicate != null && m.Id == duplicate.Id);

            if(master is null) continue;

            if(duplicate?.Duplicates == null) continue;

            foreach(int duplicateId in duplicate.Duplicates)
            {
                CommonTypes.Metadata.Scsi? slave = ctx.Scsi.Include(static scsi => scsi.ReadCapabilities)
                                                      .FirstOrDefault(m => m.Id == duplicateId);

                if(slave is null) continue;

                foreach(Device scsiDevice in ctx.Devices.Where(d => d.SCSI.Id == duplicateId)) scsiDevice.SCSI = master;

                foreach(UploadedReport scsiReport in ctx.Reports.Where(d => d.SCSI.Id == duplicateId))
                    scsiReport.SCSI = master;

                foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                        ctx.TestedMedia.Where(d => d.ScsiId == duplicateId))
                {
                    testedMedia.ScsiId = duplicate.Id;
                    ctx.Update(testedMedia);
                }

                if(master.ReadCapabilities is null && slave.ReadCapabilities != null)
                    master.ReadCapabilities = slave.ReadCapabilities;

                ctx.Scsi.Remove(slave);
            }
        }

        await ctx.SaveChangesAsync();

        await RefreshItemsAsync();

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
        await RefreshItemsAsync();
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
}