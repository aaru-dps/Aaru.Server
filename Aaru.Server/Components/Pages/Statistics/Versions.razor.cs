using Aaru.CommonTypes.Metadata;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class Versions
{
    bool                 _isAlreadyInitialized;
    PieChart<long>?      _versionsChart;
    List<long>           _versionsCounts = [];
    string[]             _versionsLabels = [];
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
                                   .ToArrayAsync();

        _versionsCounts = await ctx.Versions.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static x => x.Count)
                                   .ToListAsync();

        if(_versionsLabels.Length >= 10)
        {
            _versionsLabels[9] = "Other";

            _versionsCounts[9] = ctx.Versions.Sum(static o => o.Count) - _versionsCounts.Take(9).Sum();
        }

#pragma warning disable CS8604 // Possible null reference argument.
        await Common.HandleRedrawAsync(_versionsChart, _versionsLabels, GetVersionsChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    PieChartDataset<long> GetVersionsChartDataset() => new()
    {
        Label           = $"Top {_versionsLabels.Length} Aaru versions",
        Data            = _versionsCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };
}