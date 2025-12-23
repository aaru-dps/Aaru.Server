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

using Aaru.Server.Core;
using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Ata;

public partial class List
{
    private Modal?                 _consolidateModal;
    private int                    _deleteId;
    private Modal?                 _deleteModal;
    List<IdHashModel?>             _duplicates;
    bool                           _initialized;
    List<CommonTypes.Metadata.Ata> _items;

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

        _items = ctx.Ata.AsEnumerable()
                    .OrderBy(static m => m.IdentifyDevice?.Model)
                    .ThenBy(static m => m.IdentifyDevice?.FirmwareRevision)
                    .ToList();

        List<IdHashModel> hashes = await ctx.Ata.Select(static m => new IdHashModel
                                             {
                                                 Id   = m.Id,
                                                 Hash = Hash.Sha512(m.Identify)
                                             })
                                            .ToListAsync();

        _duplicates = hashes.GroupBy(static x => x.Hash)
                            .Where(static g => g.Count() > 1)
                            .Select(x => hashes.FirstOrDefault(y => y.Hash == x.Key))
                            .ToList();

        for(int i = 0; i < _duplicates.Count; i++)
        {
            CommonTypes.Metadata.Ata unique = ctx.Ata.First(a => a.Id == _duplicates[i].Id);

            _duplicates[i].Description = unique.IdentifyDevice?.Model;

            _duplicates[i].Duplicates = hashes.Where(h => h.Hash == _duplicates[i]?.Hash)
                                              .Skip(1)
                                              .Select(static x => x.Id)
                                              .ToArray();
        }
    }

    Task ConsolidateDuplicatesAsync() => _consolidateModal?.ShowAsync();

    Task HideConsolidateModalAsync() => _consolidateModal?.HideAsync();

    async Task ConfirmConsolidateAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(IdHashModel? duplicate in _duplicates)
        {
            CommonTypes.Metadata.Ata? master = ctx.Ata.FirstOrDefault(m => duplicate != null && m.Id == duplicate.Id);

            if(master is null) continue;

            if(duplicate?.Duplicates == null) continue;

            foreach(int duplicateId in duplicate.Duplicates)
            {
                CommonTypes.Metadata.Ata? slave = ctx.Ata.Include(static ata => ata.ReadCapabilities)
                                                     .FirstOrDefault(m => m.Id == duplicateId);

                if(slave is null) continue;

                foreach(Device ataDevice in ctx.Devices.Where(d => d.ATA.Id == duplicateId)) ataDevice.ATA = master;

                foreach(Device atapiDevice in ctx.Devices.Where(d => d.ATAPI.Id == duplicateId))
                    atapiDevice.ATAPI = master;

                foreach(UploadedReport ataReport in ctx.Reports.Where(d => d.ATA.Id == duplicateId))
                    ataReport.ATA = master;

                foreach(UploadedReport atapiReport in ctx.Reports.Where(d => d.ATAPI.Id == duplicateId))
                    atapiReport.ATAPI = master;

                foreach(CommonTypes.Metadata.TestedMedia testedMedia in
                        ctx.TestedMedia.Where(d => d.AtaId == duplicateId))
                {
                    testedMedia.AtaId = duplicate.Id;
                    ctx.Update(testedMedia);
                }

                if(master.ReadCapabilities is null && slave.ReadCapabilities != null)
                    master.ReadCapabilities = slave.ReadCapabilities;

                ctx.Ata.Remove(slave);
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
        await using DbContext     ctx = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.Ata? ata = await ctx.Ata.FindAsync(id);

        if(ata is not null)
        {
            ctx.Ata.Remove(ata);
            await ctx.SaveChangesAsync();
        }
    }
}