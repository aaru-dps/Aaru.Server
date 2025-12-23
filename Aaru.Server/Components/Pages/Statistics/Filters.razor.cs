// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Filters.razor.cs
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

namespace Aaru.Server.Components.Pages.Statistics;

public partial class Filters
{
    PieChart      _filtersChart;
    List<double?> _filtersCounts = [];
    List<string>  _filtersLabels = [];
    bool          _isAlreadyInitialized;
    List<Filter>  FiltersList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        FiltersList = await ctx.Filters.OrderBy(static filter => filter.Name).ToListAsync();

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _filtersLabels = await ctx.Filters.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static v => v.Name)
                                  .ToListAsync();

        _filtersCounts = await ctx.Filters.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static x => (double?)x.Count)
                                  .ToListAsync();

        if(_filtersLabels.Count >= 10)
        {
            _filtersLabels[9] = "Other";

            _filtersCounts[9] = ctx.Filters.Sum(static o => o.Count) - _filtersCounts.Take(9).Sum();
        }

        PieChartOptions pieChartOptions = new()
        {
            Responsive = true
        };

        pieChartOptions.Plugins.Title.Text    = $"Top {_filtersLabels.Count} filters found";
        pieChartOptions.Plugins.Title.Display = true;

        var chartData = new ChartData
        {
            Labels = _filtersLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _filtersCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _filtersChart.InitializeAsync(chartData, pieChartOptions);
    }
}