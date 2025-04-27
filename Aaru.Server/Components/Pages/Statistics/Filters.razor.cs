using Aaru.Server.Database.Models;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.New.Components.Pages.Statistics;

public partial class Filters
{
    PieChart<long>? _filtersChart;
    List<long>      _filtersCounts = [];
    string[]        _filtersLabels = [];
    List<Filter>    FiltersList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        FiltersList = await ctx.Filters.OrderBy(static filter => filter.Name).ToListAsync();

        _filtersLabels = await ctx.Filters.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static v => v.Name)
                                  .ToArrayAsync();

        _filtersCounts = await ctx.Filters.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static x => x.Count)
                                  .ToListAsync();

        if(_filtersLabels.Length >= 10)
        {
            _filtersLabels[9] = "Other";

            _filtersCounts[9] = ctx.Filters.Sum(static o => o.Count) - _filtersCounts.Take(9).Sum();
        }

#pragma warning disable CS8604 // Possible null reference argument.
        await Common.HandleRedraw(_filtersChart, _filtersLabels, GetFiltersChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.
    }


    PieChartDataset<long> GetFiltersChartDataset() => new()
    {
        Label           = $"Top {_filtersLabels.Length} filters found",
        Data            = _filtersCounts,
        BackgroundColor = Common._backgroundColors,
        BorderColor     = Common._borderColors,
        BorderWidth     = 1
    };
}