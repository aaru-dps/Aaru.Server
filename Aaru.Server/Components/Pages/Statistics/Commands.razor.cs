// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Commands.razor.cs
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

public partial class Commands
{
    PieChart      _commandsChart;
    List<double?> _commandsCounts = [];
    List<string>  _commandsLabels = [];
    bool          _isAlreadyInitialized;
    List<Command> CommandsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        CommandsList = await ctx.Commands.OrderBy(static c => c.Name).ToListAsync();

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();


        _commandsLabels = await ctx.Commands.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static v => v.Name)
                                   .ToListAsync();

        _commandsCounts = await ctx.Commands.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static x => (double?)x.Count)
                                   .ToListAsync();

        if(_commandsLabels.Count >= 10)
        {
            _commandsLabels[9] = "Other";

            _commandsCounts[9] = ctx.Commands.Sum(static o => o.Count) - _commandsCounts.Take(9).Sum();
        }

        PieChartOptions pieChartOptions = new()
        {
            Responsive = true
        };

        pieChartOptions.Plugins.Title.Text    = $"Top {_commandsLabels.Count} used commands";
        pieChartOptions.Plugins.Title.Display = true;

        var chartData = new ChartData
        {
            Labels = _commandsLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _commandsCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _commandsChart.InitializeAsync(chartData, pieChartOptions);
    }
}