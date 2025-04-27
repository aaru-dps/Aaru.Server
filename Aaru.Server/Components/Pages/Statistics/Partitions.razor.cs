using Aaru.Server.Database.Models;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class Partitions
{
    PieChart<long>? _partitionsChart;
    List<long>      _partitionsCounts = [];
    string[]        _partitionsLabels = [];
    List<Partition> PartitionsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        PartitionsList = await ctx.Partitions.OrderBy(static partition => partition.Name).ToListAsync();

        _partitionsLabels = await ctx.Partitions.OrderByDescending(static o => o.Count)
                                     .Take(10)
                                     .Select(static v => v.Name)
                                     .ToArrayAsync();

        _partitionsCounts = await ctx.Partitions.OrderByDescending(static o => o.Count)
                                     .Take(10)
                                     .Select(static x => x.Count)
                                     .ToListAsync();

        if(_partitionsLabels.Length >= 10)
        {
            _partitionsLabels[9] = "Other";

            _partitionsCounts[9] = ctx.Partitions.Sum(static o => o.Count) - _partitionsCounts.Take(9).Sum();
        }

#pragma warning disable CS8604 // Possible null reference argument.
        await Common.HandleRedraw(_partitionsChart, _partitionsLabels, GetPartitionsChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    PieChartDataset<long> GetPartitionsChartDataset() => new()
    {
        Label           = $"Top {_partitionsLabels.Length} partitioning schemes found",
        Data            = _partitionsCounts,
        BackgroundColor = Common._backgroundColors,
        BorderColor     = Common._borderColors,
        BorderWidth     = 1
    };
}