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
                    Data = _commandsCounts
                }
            ]
        };

        await _commandsChart.InitializeAsync(chartData, pieChartOptions);
    }
}