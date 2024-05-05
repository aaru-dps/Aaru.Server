using Aaru.CommonTypes.Metadata;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.New.Components.Pages.Statistics;

public partial class Versions
{
    PieChart<long>?      _versionsChart;
    List<long>           _versionsCounts = [];
    string[]             _versionsLabels = [];
    List<NameValueStats> VersionsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        VersionsList = (await ctx.Versions.Select(static nvs => new NameValueStats
                                  {
                                      name  = nvs.Name == "previous" ? "Previous than 3.4.99.0" : nvs.Name,
                                      Value = nvs.Count
                                  })
                                 .ToListAsync()).OrderBy(static version => version.name)
                                                .ToList();

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
        await Common.HandleRedraw(_versionsChart, _versionsLabels, GetVersionsChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    PieChartDataset<long> GetVersionsChartDataset() => new()
    {
        Label           = $"Top {_versionsLabels.Length} Aaru versions",
        Data            = _versionsCounts,
        BackgroundColor = Common._backgroundColors,
        BorderColor     = Common._borderColors,
        BorderWidth     = 1
    };
}