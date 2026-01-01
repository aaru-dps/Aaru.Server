// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : View.razor.cs
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

namespace Aaru.Server.Components.Admin.Pages.Pcmcia;

public partial class View
{
    private int                               _deleteId;
    private Modal?                            _deleteModal;
    private bool                              _initialized;
    private List<CommonTypes.Metadata.Pcmcia> _items      = new();
    private string                            _searchTerm = string.Empty;

    private IEnumerable<CommonTypes.Metadata.Pcmcia> FilteredItems
    {
        get
        {
            if(string.IsNullOrWhiteSpace(_searchTerm)) return _items;

            return _items.Where(item =>
                                    (item.Manufacturer?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    (item.ProductName?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    (item.Compliance?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    (item.ManufacturerCode?.ToString()
                                         .Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    (item.CardCode?.ToString()
                                         .Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    item.Id.ToString().Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Pcmcia.ToListAsync();

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
        await using DbContext        ctx    = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.Pcmcia? pcmcia = await ctx.Pcmcia.FindAsync(id);

        if(pcmcia is not null)
        {
            ctx.Pcmcia.Remove(pcmcia);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Pcmcia.ToListAsync();

        StateHasChanged();
    }
}