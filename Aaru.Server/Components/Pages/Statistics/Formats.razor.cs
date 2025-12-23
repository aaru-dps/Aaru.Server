// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Formats.razor.cs
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

public partial class Formats
{
    PieChart          _formatsChart;
    List<double?>     _formatsCounts = [];
    List<string>      _formatsLabels = [];
    bool              _isAlreadyInitialized;
    List<MediaFormat> MediaImages { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        MediaImages = await ctx.MediaFormats.OrderBy(static format => format.Name).ToListAsync();

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _formatsLabels = await ctx.MediaFormats.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static v => v.Name)
                                  .ToListAsync();

        _formatsCounts = await ctx.MediaFormats.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static x => (double?)x.Count)
                                  .ToListAsync();

        if(_formatsLabels.Count >= 10)
        {
            _formatsLabels[9] = "Other";

            _formatsCounts[9] = ctx.MediaFormats.Sum(static o => o.Count) - _formatsCounts.Take(9).Sum();
        }

        PieChartOptions pieChartOptions = new()
        {
            Responsive = true
        };

        pieChartOptions.Plugins.Title.Text    = $"Top {_formatsLabels.Count} media image formats found";
        pieChartOptions.Plugins.Title.Display = true;

        var chartData = new ChartData
        {
            Labels = _formatsLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _formatsCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _formatsChart.InitializeAsync(chartData, pieChartOptions);
    }
}