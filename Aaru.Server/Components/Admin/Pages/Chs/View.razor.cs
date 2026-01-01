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

using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Chs;

public partial class View
{
    private Modal?                         _consolidateModal;
    private List<ChsModel>                 _duplicates = new();
    private bool                           _initialized;
    private List<CommonTypes.Metadata.Chs> _items      = new();
    private string                         _searchTerm = string.Empty;

    private IEnumerable<CommonTypes.Metadata.Chs> FilteredItems
    {
        get
        {
            if(string.IsNullOrWhiteSpace(_searchTerm)) return _items;

            return _items.Where(item =>
                                    item.Cylinders.ToString()
                                        .Contains(_searchTerm, StringComparison.OrdinalIgnoreCase)                    ||
                                    item.Heads.ToString().Contains(_searchTerm, StringComparison.OrdinalIgnoreCase)   ||
                                    item.Sectors.ToString().Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                    item.Id.ToString().Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }


    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Chs.OrderBy(static c => c.Cylinders)
                          .ThenBy(static c => c.Heads)
                          .ThenBy(static c => c.Sectors)
                          .ToListAsync();

        _duplicates = await ctx.Chs.GroupBy(static x => new
                                {
                                    x.Cylinders,
                                    x.Heads,
                                    x.Sectors
                                })
                               .Where(static x => x.Count() > 1)
                               .Select(static x => new ChsModel
                                {
                                    Cylinders = x.Key.Cylinders,
                                    Heads     = x.Key.Heads,
                                    Sectors   = x.Key.Sectors
                                })
                               .ToListAsync();

        _initialized = true;

        StateHasChanged();
    }

    Task ConsolidateDuplicatesAsync() => _consolidateModal?.ShowAsync();

    Task HideConsolidateModalAsync() => _consolidateModal?.HideAsync();

    async Task ConfirmConsolidateAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(ChsModel duplicate in _duplicates)
        {
            CommonTypes.Metadata.Chs? master = ctx.Chs.FirstOrDefault(m => m.Cylinders == duplicate.Cylinders &&
                                                                           m.Heads     == duplicate.Heads     &&
                                                                           m.Sectors   == duplicate.Sectors);

            if(master is null) continue;

            foreach(CommonTypes.Metadata.Chs chs in await ctx.Chs.Where(m => m.Cylinders == duplicate.Cylinders &&
                                                                             m.Heads     == duplicate.Heads     &&
                                                                             m.Sectors   == duplicate.Sectors)
                                                             .Skip(1)
                                                             .ToArrayAsync())
            {
                foreach(CommonTypes.Metadata.TestedMedia media in ctx.TestedMedia.Where(d => d.CHS.Id == chs.Id))
                    media.CHS = master;

                foreach(CommonTypes.Metadata.TestedMedia media in ctx.TestedMedia.Where(d => d.CurrentCHS.Id == chs.Id))
                    media.CurrentCHS = master;

                ctx.Chs.Remove(chs);
            }
        }

        await ctx.SaveChangesAsync();

        _items = await ctx.Chs.OrderBy(static c => c.Cylinders)
                          .ThenBy(static c => c.Heads)
                          .ThenBy(static c => c.Sectors)
                          .ToListAsync();

        _duplicates = await ctx.Chs.GroupBy(static x => new
                                {
                                    x.Cylinders,
                                    x.Heads,
                                    x.Sectors
                                })
                               .Where(static x => x.Count() > 1)
                               .Select(static x => new ChsModel
                                {
                                    Cylinders = x.Key.Cylinders,
                                    Heads     = x.Key.Heads,
                                    Sectors   = x.Key.Sectors
                                })
                               .ToListAsync();

        StateHasChanged();
    }
}