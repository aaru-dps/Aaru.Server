using Aaru.Server.Database.Models;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.New.Components.Pages.Statistics;

public partial class Commands
{
    PieChart<long>? _commandsChart;
    List<long>      _commandsCounts = [];
    string[]        _commandsLabels = [];
    List<Command>   CommandsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        CommandsList = await ctx.Commands.OrderBy(static c => c.Name).ToListAsync();

        _commandsLabels = await ctx.Commands.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static v => v.Name)
                                   .ToArrayAsync();

        _commandsCounts = await ctx.Commands.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static x => x.Count)
                                   .ToListAsync();

        if(_commandsLabels.Length >= 10)
        {
            _commandsLabels[9] = "Other";

            _commandsCounts[9] = ctx.Commands.Sum(static o => o.Count) - _commandsCounts.Take(9).Sum();
        }

#pragma warning disable CS8604 // Possible null reference argument.
        await Common.HandleRedraw(_commandsChart, _commandsLabels, GetCommandsChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    PieChartDataset<long> GetCommandsChartDataset() => new()
    {
        Label           = $"Top {_commandsLabels.Length} used commands",
        Data            = _commandsCounts,
        BackgroundColor = Common._backgroundColors,
        BorderColor     = Common._borderColors,
        BorderWidth     = 1
    };
}