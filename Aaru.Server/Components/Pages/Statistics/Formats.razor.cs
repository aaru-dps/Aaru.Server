using Aaru.Server.Database.Models;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.New.Components.Pages.Statistics;

public partial class Formats
{
    PieChart<long>?   _formatsChart;
    List<long>        _formatsCounts = [];
    string[]          _formatsLabels = [];
    List<MediaFormat> MediaImages { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();


        MediaImages = await ctx.MediaFormats.OrderBy(static format => format.Name).ToListAsync();

        _formatsLabels = await ctx.MediaFormats.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static v => v.Name)
                                  .ToArrayAsync();

        _formatsCounts = await ctx.MediaFormats.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static x => x.Count)
                                  .ToListAsync();

        if(_formatsLabels.Length >= 10)
        {
            _formatsLabels[9] = "Other";

            _formatsCounts[9] = ctx.MediaFormats.Sum(static o => o.Count) - _formatsCounts.Take(9).Sum();
        }


#pragma warning disable CS8604 // Possible null reference argument.
        await Common.HandleRedraw(_formatsChart, _formatsLabels, GetFormatsChartDataset);

#pragma warning restore CS8604 // Possible null reference argument.
    }

    PieChartDataset<long> GetFormatsChartDataset() => new()
    {
        Label           = $"Top {_formatsLabels.Length} media image formats found",
        Data            = _formatsCounts,
        BackgroundColor = Common._backgroundColors,
        BorderColor     = Common._borderColors,
        BorderWidth     = 1
    };
}