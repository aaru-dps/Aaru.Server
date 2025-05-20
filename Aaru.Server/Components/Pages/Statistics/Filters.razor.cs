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
                    Data = _filtersCounts
                }
            ]
        };

        await _filtersChart.InitializeAsync(chartData, pieChartOptions);
    }
}