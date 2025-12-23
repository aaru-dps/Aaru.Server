// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Filesystems.razor.cs
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

public partial class Filesystems
{
    PieChart         _filesystemsChart;
    List<double?>    _filesystemsCounts = [];
    List<string>     _filesystemsLabels = [];
    bool             _isAlreadyInitialized;
    List<Filesystem> FilesystemsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        FilesystemsList = await ctx.Filesystems.OrderBy(static filesystem => filesystem.Name).ToListAsync();

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _filesystemsLabels = await ctx.Filesystems.OrderByDescending(static o => o.Count)
                                      .Take(10)
                                      .Select(static v => v.Name)
                                      .ToListAsync();

        _filesystemsCounts = await ctx.Filesystems.OrderByDescending(static o => o.Count)
                                      .Take(10)
                                      .Select(static x => (double?)x.Count)
                                      .ToListAsync();

        if(_filesystemsLabels.Count >= 10)
        {
            _filesystemsLabels[9] = "Other";

            _filesystemsCounts[9] = ctx.Filesystems.Sum(static o => o.Count) - _filesystemsCounts.Take(9).Sum();
        }

        PieChartOptions pieChartOptions = new()
        {
            Responsive = true
        };

        pieChartOptions.Plugins.Title.Text    = $"Top {_filesystemsLabels.Count} filesystems found";
        pieChartOptions.Plugins.Title.Display = true;

        var chartData = new ChartData
        {
            Labels = _filesystemsLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _filesystemsCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _filesystemsChart.InitializeAsync(chartData, pieChartOptions);
    }
}