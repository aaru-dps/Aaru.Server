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

using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using OperatingSystem = Aaru.Server.Database.Models.OperatingSystem;

namespace Aaru.Server.Components.Admin.Pages.OperatingSystems;

public partial class View
{
    private bool                  _initialized;
    private List<OperatingSystem> _items      = new();
    private string                _searchTerm = string.Empty;

    private IEnumerable<OperatingSystem> FilteredItems
    {
        get
        {
            if(string.IsNullOrWhiteSpace(_searchTerm)) return _items;

            return _items.Where(item =>
                                    (item.Name?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                    (item.Version?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false)                                                                         ||
                                    item.Count.ToString().Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                    item.Id.ToString().Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.OperatingSystems.OrderBy(static o => o.Name).ThenBy(static o => o.Version).ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}