using Aaru.CommonTypes.Interop;
using Aaru.CommonTypes.Metadata;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using PlatformID = Aaru.CommonTypes.Interop.PlatformID;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class OperatingSystems
{
    bool                 _isAlreadyInitialized;
    PieChart             _linuxChart;
    List<double?>        _linuxCounts = [];
    List<string>         _linuxLabels = [];
    PieChart             _macosChart;
    List<double?>        _macosCounts = [];
    List<string>         _macosLabels = [];
    Carousel?            _operatingSystemsCarousel;
    PieChart             _operatingSystemsChart;
    List<double?>        _operatingSystemsCounts = [];
    List<string>         _operatingSystemsLabels = [];
    PieChart             _windowsChart;
    List<double?>        _windowsCounts = [];
    List<string>         _windowsLabels = [];
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

        _operatingSystemsLabels = await osQuery.Select(static x => x.Name).ToListAsync();
        _operatingSystemsCounts = await osQuery.Select(static x => (double?)x.Count).ToListAsync();

        for(var i = 0; i < _operatingSystemsLabels.Count; i++)
        {
            _operatingSystemsLabels[i] =
                DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), _operatingSystemsLabels[i]));
        }

        _linuxLabels = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Linux))
                                .OrderByDescending(static o => o.Count)
                                .Take(10)
                                .Select(static x =>
                                            $"{DetectOS.GetPlatformName(PlatformID.Linux, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}")
                                .ToListAsync();

        _linuxCounts = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Linux))
                                .OrderByDescending(static o => o.Count)
                                .Take(10)
                                .Select(static x => (double?)x.Count)
                                .ToListAsync();

        if(_linuxLabels.Count >= 10)
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
                                .ToListAsync();

        _macosCounts = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.MacOSX))
                                .OrderByDescending(static o => o.Count)
                                .Take(10)
                                .Select(static x => (double?)x.Count)
                                .ToListAsync();


        if(_macosLabels.Count >= 10)
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
                                  .ToListAsync();

        _windowsCounts = await ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Win32NT))
                                  .OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static x => (double?)x.Count)
                                  .ToListAsync();


        if(_windowsLabels.Count >= 10)
        {
            _windowsLabels[9] = "Other";

            _windowsCounts[9] =
                ctx.OperatingSystems.Where(static o => o.Name == nameof(PlatformID.Win32NT)).Sum(static o => o.Count) -
                _windowsCounts.Take(9).Sum();
        }

        PieChartOptions osesChartOptions = new()
        {
            Responsive = true
        };

        osesChartOptions.Plugins.Title.Text    = "Operating systems";
        osesChartOptions.Plugins.Title.Display = true;

        var osesChartData = new ChartData
        {
            Labels = _operatingSystemsLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _operatingSystemsCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _operatingSystemsChart.InitializeAsync(osesChartData, osesChartOptions);

        PieChartOptions linuxChartOptions = new()
        {
            Responsive = true
        };

        linuxChartOptions.Plugins.Title.Text    = $"Top {_linuxLabels.Count} Linux versions";
        linuxChartOptions.Plugins.Title.Display = true;

        var linuxChartData = new ChartData
        {
            Labels = _linuxLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _linuxCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _linuxChart.InitializeAsync(linuxChartData, linuxChartOptions);

        PieChartOptions macosChartOptions = new()
        {
            Responsive = true
        };

        macosChartOptions.Plugins.Title.Text    = $"Top {_macosLabels.Count} macOS versions";
        macosChartOptions.Plugins.Title.Display = true;

        var macosChartData = new ChartData
        {
            Labels = _macosLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _macosCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _macosChart.InitializeAsync(macosChartData, macosChartOptions);

        PieChartOptions windowsChartOptions = new()
        {
            Responsive = true
        };

        windowsChartOptions.Plugins.Title.Text    = $"Top {_windowsLabels.Count} Windows versions";
        windowsChartOptions.Plugins.Title.Display = true;

        var windowsChartData = new ChartData
        {
            Labels = _windowsLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _windowsCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _windowsChart.InitializeAsync(windowsChartData, windowsChartOptions);

//        _operatingSystemsCarousel.Interval = 5000;
    }

    static string GetPlatformName(string name, string version) =>
        DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), name), version);
}