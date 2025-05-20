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