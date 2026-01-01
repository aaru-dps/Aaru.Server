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

using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.TestedMedia;

public partial class List
{
    private int                                    _deleteId;
    private Modal?                                 _deleteModal;
    private bool                                   _initialized;
    private List<CommonTypes.Metadata.TestedMedia> _items      = new();
    private string                                 _searchTerm = string.Empty;

    private IEnumerable<CommonTypes.Metadata.TestedMedia> FilteredItems
    {
        get
        {
            if(string.IsNullOrWhiteSpace(_searchTerm)) return _items;

            return _items.Where(item =>
                                    (item.Manufacturer?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false)                                                                          ||
                                    (item.Model?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                    (item.MediumTypeName?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
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

        _items = await ctx.TestedMedia.OrderBy(static m => m.Manufacturer)
                          .ThenBy(static m => m.Model)
                          .ThenBy(static m => m.MediumTypeName)
                          .ThenBy(static m => m.MediaIsRecognized)
                          .ThenBy(static m => m.LongBlockSize)
                          .ThenBy(static m => m.BlockSize)
                          .ThenBy(static m => m.Blocks)
                          .ToListAsync();
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
        await using DbContext             ctx   = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.TestedMedia? media = await ctx.TestedMedia.FindAsync(id);

        if(media is not null)
        {
            ctx.TestedMedia.Remove(media);
            await ctx.SaveChangesAsync();
        }
    }
}