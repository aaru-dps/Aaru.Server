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

using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Scsi.Cdrom;

public partial class List
{
    private int           _deleteId;
    private Modal?        _deleteModal;
    bool                  _initialized;
    List<MmcModelForView> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Mmc.Where(static m => m.ModeSense2AData != null)
                          .Select(static m => new MmcModelForView
                           {
                               Id         = m.Id,
                               FeaturesId = m.FeaturesId,
                               DataLength = m.ModeSense2AData.Length
                           })
                          .Concat(ctx.Mmc.Where(static m => m.ModeSense2AData == null)
                                     .Select(static m => new MmcModelForView
                                      {
                                          Id         = m.Id,
                                          FeaturesId = m.FeaturesId,
                                          DataLength = 0
                                      }))
                          .OrderBy(static m => m.Id)
                          .ToListAsync();

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
        await RefreshItemsAsync();
    }

    private async Task DeleteAsync(int id)
    {
        await using DbContext     ctx = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.Mmc? mmc = await ctx.Mmc.FindAsync(id);

        if(mmc is not null)
        {
            ctx.Mmc.Remove(mmc);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Mmc.Where(static m => m.ModeSense2AData != null)
                          .Select(static m => new MmcModelForView
                           {
                               Id         = m.Id,
                               FeaturesId = m.FeaturesId,
                               DataLength = m.ModeSense2AData.Length
                           })
                          .Concat(ctx.Mmc.Where(static m => m.ModeSense2AData == null)
                                     .Select(static m => new MmcModelForView
                                      {
                                          Id         = m.Id,
                                          FeaturesId = m.FeaturesId,
                                          DataLength = 0
                                      }))
                          .OrderBy(static m => m.Id)
                          .ToListAsync();

        StateHasChanged();
    }
}