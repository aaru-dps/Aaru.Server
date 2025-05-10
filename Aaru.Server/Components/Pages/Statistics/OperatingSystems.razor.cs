using Aaru.CommonTypes.Interop;
using Aaru.CommonTypes.Metadata;
using Blazorise;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using PlatformID = Aaru.CommonTypes.Interop.PlatformID;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class OperatingSystems
{
    bool                 _isAlreadyInitialized;
    PieChart<long>?      _linuxChart;
    List<long>           _linuxCounts = [];
    string[]             _linuxLabels = [];
    PieChart<long>?      _macosChart;
    List<long>           _macosCounts = [];
    string[]             _macosLabels = [];
    Carousel?            _operatingSystemsCarousel;
    PieChart<long>?      _operatingSystemsChart;
    List<long>           _operatingSystemsCounts = [];
    string[]             _operatingSystemsLabels = [];
    PieChart<long>?      _windowsChart;
    List<long>           _windowsCounts = [];
    string[]             _windowsLabels = [];
    List<NameValueStats> OperatingSystemsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        // TODO: Cache real OS name in database, lookups would be much faster
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        OperatingSystemsList = (await ctx.OperatingSystems.OrderBy(static os => os.Name)
                                         .ThenBy(static os => os.Version)
                                         .Select(static nvs => new NameValueStats
                                          {
                                              name =
                                                  $"{GetPlatformName(nvs.Name, nvs.Version)}{(string.IsNullOrEmpty(nvs.Version) ? "" : " ")}{nvs.Version}",
                                              Value = nvs.Count
                                          })
                                         .ToListAsync()).OrderBy(static os => os.name)
                                                        .ToList();

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;

        // TODO: Cache real OS name in database, lookups would be much faster
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        var osQuery = ctx.OperatingSystems.GroupBy(static x => new
                                                   {
                                                       x.Name
                                                   },
                                                   static x => x.Count)
                         .Select(static g => new
                          {
                              g.Key.Name,
                              Count = g.Sum()
                          });

        _operatingSystemsLabels = await osQuery.Select(static x => x.Name).ToArrayAsync();
        _operatingSystemsCounts = await osQuery.Select(static x => x.Count).ToListAsync();

        for(var i = 0; i < _operatingSystemsLabels.Length; i++)
        {
            _operatingSystemsLabels[i] =
                DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), _operatingSystemsLabels[i]));
        }

        _linuxLabels = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Linux))
                                .OrderByDescending(static o => o.Count)
                                .Take(10)
                                .Select(static x =>
                                            $"{DetectOS.GetPlatformName(PlatformID.Linux, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}")
                                .ToArrayAsync();

        _linuxCounts = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Linux))
                                .OrderByDescending(static o => o.Count)
                                .Take(10)
                                .Select(static x => x.Count)
                                .ToListAsync();


        if(_linuxLabels.Length >= 10)
        {
            _linuxLabels[9] = "Other";

            _linuxCounts[9] =
                ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Linux)).Sum(static o => o.Count) -
                _linuxCounts.Take(9).Sum();
        }

        _macosLabels = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.MacOSX))
                                .OrderByDescending(static o => o.Count)
                                .Take(10)
                                .Select(static x =>
                                            $"{DetectOS.GetPlatformName(PlatformID.MacOSX, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}")
                                .ToArrayAsync();

        _macosCounts = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.MacOSX))
                                .OrderByDescending(static o => o.Count)
                                .Take(10)
                                .Select(static x => x.Count)
                                .ToListAsync();


        if(_macosLabels.Length >= 10)
        {
            _macosLabels[9] = "Other";

            _macosCounts[9] =
                ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.MacOSX)).Sum(static o => o.Count) -
                _macosCounts.Take(9).Sum();
        }

        _windowsLabels = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Win32NT))
                                  .OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static x =>
                                              $"{DetectOS.GetPlatformName(PlatformID.Win32NT, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}")
                                  .ToArrayAsync();

        _windowsCounts = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Win32NT))
                                  .OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static x => x.Count)
                                  .ToListAsync();


        if(_windowsLabels.Length >= 10)
        {
            _windowsLabels[9] = "Other";

            _windowsCounts[9] =
                ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Win32NT)).Sum(static o => o.Count) -
                _windowsCounts.Take(9).Sum();
        }

#pragma warning disable CS8604 // Possible null reference argument.

        await Task.WhenAll(Common.HandleRedrawAsync(_operatingSystemsChart,
                                                    _operatingSystemsLabels,
                                                    GetOperatingSystemsChartDataset),
                           Common.HandleRedrawAsync(_linuxChart,   _linuxLabels,   GetLinuxChartDataset),
                           Common.HandleRedrawAsync(_macosChart,   _macosLabels,   GetMacosChartDataset),
                           Common.HandleRedrawAsync(_windowsChart, _windowsLabels, GetWindowsChartDataset));
#pragma warning restore CS8604 // Possible null reference argument.

        // Upstream: https://github.com/Megabit/Blazorise/issues/5491
        _operatingSystemsCarousel.Interval = 5000;
    }

    PieChartDataset<long> GetOperatingSystemsChartDataset() => new()
    {
        Label           = "Operating systems",
        Data            = _operatingSystemsCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetLinuxChartDataset() => new()
    {
        Label           = $"Top {_linuxLabels.Length} Linux versions",
        Data            = _linuxCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetMacosChartDataset() => new()
    {
        Label           = $"Top {_macosLabels.Length} macOS versions",
        Data            = _macosCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetWindowsChartDataset() => new()
    {
        Label           = $"Top {_windowsLabels.Length} Windows versions",
        Data            = _windowsCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };

    static string GetPlatformName(string name, string version) =>
        DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), name), version);
}