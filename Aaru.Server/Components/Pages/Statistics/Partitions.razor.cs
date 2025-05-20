using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class Partitions
{
    bool            _isAlreadyInitialized;
    PieChart        _partitionsChart;
    List<double?>   _partitionsCounts = [];
    List<string>    _partitionsLabels = [];
    List<Partition> PartitionsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        PartitionsList = await ctx.Partitions.OrderBy(static partition => partition.Name).ToListAsync();

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _partitionsLabels = await ctx.Partitions.OrderByDescending(static o => o.Count)
                                     .Take(10)
                                     .Select(static v => v.Name)
                                     .ToListAsync();

        _partitionsCounts = await ctx.Partitions.OrderByDescending(static o => o.Count)
                                     .Take(10)
                                     .Select(static x => (double?)x.Count)
                                     .ToListAsync();

        if(_partitionsLabels.Count >= 10)
        {
            _partitionsLabels[9] = "Other";

            _partitionsCounts[9] = ctx.Partitions.Sum(static o => o.Count) - _partitionsCounts.Take(9).Sum();
        }

        PieChartOptions pieChartOptions = new()
        {
            Responsive = true
        };

        pieChartOptions.Plugins.Title.Text    = $"Top {_partitionsLabels.Count} partitioning schemes found";
        pieChartOptions.Plugins.Title.Display = true;

        var chartData = new ChartData
        {
            Labels = _partitionsLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _partitionsCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _partitionsChart.InitializeAsync(chartData, pieChartOptions);
    }
}