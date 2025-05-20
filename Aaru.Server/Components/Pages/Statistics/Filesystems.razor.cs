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