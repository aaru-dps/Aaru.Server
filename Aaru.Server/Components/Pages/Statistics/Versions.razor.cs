using Aaru.CommonTypes.Metadata;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class Versions
{
    bool                 _isAlreadyInitialized;
    PieChart             _versionsChart;
    List<double?>        _versionsCounts = [];
    List<string>         _versionsLabels = [];
    List<NameValueStats> VersionsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        VersionsList = (await ctx.Versions.Select(static nvs => new NameValueStats
                                  {
                                      name  = nvs.Name == "previous" ? "Previous than 3.4.99.0" : nvs.Name,
                                      Value = nvs.Count
                                  })
                                 .ToListAsync()).OrderBy(static version => version.name)
                                                .ToList();

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _versionsLabels = await ctx.Versions.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static v => v.Name == "previous" ? "Previous than 3.4.99.0" : v.Name)
                                   .ToListAsync();

        _versionsCounts = await ctx.Versions.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static x => (double?)x.Count)
                                   .ToListAsync();

        if(_versionsLabels.Count >= 10)
        {
            _versionsLabels[9] = "Other";

            _versionsCounts[9] = ctx.Versions.Sum(static o => o.Count) - _versionsCounts.Take(9).Sum();
        }

        PieChartOptions pieChartOptions = new()
        {
            Responsive = true
        };

        pieChartOptions.Plugins.Title.Text    = $"Top {_versionsLabels.Count} Aaru versions";
        pieChartOptions.Plugins.Title.Display = true;

        var chartData = new ChartData
        {
            Labels = _versionsLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _versionsCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _versionsChart.InitializeAsync(chartData, pieChartOptions);
    }
}