using Aaru.CommonTypes.Interop;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using Blazorise;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Media = Aaru.Server.Database.Models.Media;
using PlatformID = Aaru.CommonTypes.Interop.PlatformID;

namespace Aaru.Server.New.Components.Pages;

public partial class Stats
{
    readonly List<string> _backgroundColors =
    [
        ChartColor.FromHtmlColorCode("#006412"), ChartColor.FromHtmlColorCode("#0000D3"),
        ChartColor.FromHtmlColorCode("#FF6403"), ChartColor.FromHtmlColorCode("#562C05"),
        ChartColor.FromHtmlColorCode("#DD0907"), ChartColor.FromHtmlColorCode("#F20884"),
        ChartColor.FromHtmlColorCode("#4700A5"), ChartColor.FromHtmlColorCode("#90713A"),
        ChartColor.FromHtmlColorCode("#1FB714"), ChartColor.FromHtmlColorCode("#02ABEA"),
        ChartColor.FromHtmlColorCode("#FBF305")
    ];
    readonly List<string> _borderColors = [ChartColor.FromHtmlColorCode("#8b0000")];
    PieChart<long>?       _commandsChart;
    List<long>            _commandsCounts = [];
    string[]              _commandsLabels = [];
    PieChart<long>?       _devicesByBusChart;
    List<long>            _devicesByBusCounts = [];
    string[]              _devicesByBusLabels = [];
    PieChart<long>?       _devicesByManufacturerChart;
    List<long>            _devicesByManufacturerCounts = [];
    string[]              _devicesByManufacturerLabels = [];
    Carousel?             _devicesCarousel;
    PieChart<long>?       _filesystemsChart;
    List<long>            _filesystemsCounts = [];
    string[]              _filesystemsLabels = [];
    PieChart<long>?       _filtersChart;
    List<long>            _filtersCounts = [];
    string[]              _filtersLabels = [];
    PieChart<long>?       _formatsChart;
    List<long>            _formatsCounts = [];
    string[]              _formatsLabels = [];
    PieChart<long>?       _linuxChart;
    List<long>            _linuxCounts = [];
    string[]              _linuxLabels = [];
    PieChart<long>?       _macosChart;
    List<long>            _macosCounts = [];
    string[]              _macosLabels = [];
    Carousel?             _operatingSystemsCarousel;
    PieChart<long>?       _operatingSystemsChart;
    List<long>            _operatingSystemsCounts = [];
    string[]              _operatingSystemsLabels = [];
    PieChart<long>?       _partitionsChart;
    List<long>            _partitionsCounts = [];
    string[]              _partitionsLabels = [];
    PieChart<long>?       _realMediaChart;
    List<long>            _realMediaCounts = [];
    string[]              _realMediaLabels = [];
    PieChart<long>?       _versionsChart;
    List<long>            _versionsCounts = [];
    string[]              _versionsLabels = [];
    PieChart<long>?       _virtualMediaChart;
    List<long>            _virtualMediaCounts = [];
    string[]              _virtualMediaLabels = [];
    PieChart<long>?       _windowsChart;
    List<long>            _windowsCounts = [];
    string[]              _windowsLabels = [];
    List<NameValueStats>  OperatingSystems { get; set; } = [];
    List<NameValueStats>  Versions         { get; set; } = [];
    List<Command>         Commands         { get; set; } = [];
    List<Filter>          Filters          { get; set; } = [];
    List<MediaFormat>     MediaImages      { get; set; } = [];
    List<Partition>       Partitions       { get; set; } = [];
    List<Filesystem>      Filesystems      { get; set; } = [];
    List<MediaItem>       RealMedia        { get; set; } = [];
    List<MediaItem>       VirtualMedia     { get; set; } = [];
    List<DeviceItem>      Devices          { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        // TOOD: Cache real OS name in database, lookups would be much faster
        await using DbContext _ctx = await DbContextFactory.CreateDbContextAsync();

        OperatingSystems = (await _ctx.OperatingSystems.OrderBy(static os => os.Name)
                                      .ThenBy(static os => os.Version)
                                      .Select(static nvs => new NameValueStats
                                       {
                                           name =
                                               $"{GetPlatformName(nvs.Name, nvs.Version)}{(string.IsNullOrEmpty(nvs.Version) ? "" : " ")}{nvs.Version}",
                                           Value = nvs.Count
                                       })
                                      .ToListAsync()).OrderBy(static os => os.name)
                                                     .ToList();

        var osQuery = _ctx.OperatingSystems.GroupBy(static x => new
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

        _linuxLabels = await _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.Linux.ToString())
                                 .OrderByDescending(static o => o.Count)
                                 .Take(10)
                                 .Select(static x =>
                                             $"{DetectOS.GetPlatformName(PlatformID.Linux, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}")
                                 .ToArrayAsync();

        _linuxCounts = await _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.Linux.ToString())
                                 .OrderByDescending(static o => o.Count)
                                 .Take(10)
                                 .Select(static x => x.Count)
                                 .ToListAsync();


        if(_linuxLabels.Length >= 10)
        {
            _linuxLabels[9] = "Other";

            _linuxCounts[9] =
                _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.Linux.ToString())
                    .Sum(static o => o.Count) -
                _linuxCounts.Take(9).Sum();
        }

        _macosLabels = await _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.MacOSX.ToString())
                                 .OrderByDescending(static o => o.Count)
                                 .Take(10)
                                 .Select(static x =>
                                             $"{DetectOS.GetPlatformName(PlatformID.MacOSX, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}")
                                 .ToArrayAsync();

        _macosCounts = await _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.MacOSX.ToString())
                                 .OrderByDescending(static o => o.Count)
                                 .Take(10)
                                 .Select(static x => x.Count)
                                 .ToListAsync();


        if(_macosLabels.Length >= 10)
        {
            _macosLabels[9] = "Other";

            _macosCounts[9] =
                _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.MacOSX.ToString())
                    .Sum(static o => o.Count) -
                _macosCounts.Take(9).Sum();
        }

        _windowsLabels = await _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.Win32NT.ToString())
                                   .OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static x =>
                                               $"{DetectOS.GetPlatformName(PlatformID.Win32NT, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}")
                                   .ToArrayAsync();

        _windowsCounts = await _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.Win32NT.ToString())
                                   .OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static x => x.Count)
                                   .ToListAsync();


        if(_windowsLabels.Length >= 10)
        {
            _windowsLabels[9] = "Other";

            _windowsCounts[9] =
                _ctx.OperatingSystems.Where(static o => o.Name == PlatformID.Win32NT.ToString())
                    .Sum(static o => o.Count) -
                _windowsCounts.Take(9).Sum();
        }

        Versions = (await _ctx.Versions.Select(static nvs => new NameValueStats
                               {
                                   name  = nvs.Name == "previous" ? "Previous than 3.4.99.0" : nvs.Name,
                                   Value = nvs.Count
                               })
                              .ToListAsync()).OrderBy(static version => version.name)
                                             .ToList();

        _versionsLabels = await _ctx.Versions.OrderByDescending(static o => o.Count)
                                    .Take(10)
                                    .Select(static v => v.Name == "previous" ? "Previous than 3.4.99.0" : v.Name)
                                    .ToArrayAsync();

        _versionsCounts = await _ctx.Versions.OrderByDescending(static o => o.Count)
                                    .Take(10)
                                    .Select(static x => x.Count)
                                    .ToListAsync();

        if(_versionsLabels.Length >= 10)
        {
            _versionsLabels[9] = "Other";

            _versionsCounts[9] = _ctx.Versions.Sum(static o => o.Count) - _versionsCounts.Take(9).Sum();
        }

        Commands = await _ctx.Commands.OrderBy(static c => c.Name).ToListAsync();

        _commandsLabels = await _ctx.Commands.OrderByDescending(static o => o.Count)
                                    .Take(10)
                                    .Select(static v => v.Name)
                                    .ToArrayAsync();

        _commandsCounts = await _ctx.Commands.OrderByDescending(static o => o.Count)
                                    .Take(10)
                                    .Select(static x => x.Count)
                                    .ToListAsync();

        if(_commandsLabels.Length >= 10)
        {
            _commandsLabels[9] = "Other";

            _commandsCounts[9] = _ctx.Commands.Sum(static o => o.Count) - _commandsCounts.Take(9).Sum();
        }

        Filters = await _ctx.Filters.OrderBy(static filter => filter.Name).ToListAsync();

        _filtersLabels = await _ctx.Filters.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static v => v.Name)
                                   .ToArrayAsync();

        _filtersCounts = await _ctx.Filters.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static x => x.Count)
                                   .ToListAsync();

        if(_filtersLabels.Length >= 10)
        {
            _filtersLabels[9] = "Other";

            _filtersCounts[9] = _ctx.Filters.Sum(static o => o.Count) - _filtersCounts.Take(9).Sum();
        }

        MediaImages = await _ctx.MediaFormats.OrderBy(static format => format.Name).ToListAsync();

        _formatsLabels = await _ctx.MediaFormats.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static v => v.Name)
                                   .ToArrayAsync();

        _formatsCounts = await _ctx.MediaFormats.OrderByDescending(static o => o.Count)
                                   .Take(10)
                                   .Select(static x => x.Count)
                                   .ToListAsync();

        if(_formatsLabels.Length >= 10)
        {
            _formatsLabels[9] = "Other";

            _formatsCounts[9] = _ctx.MediaFormats.Sum(static o => o.Count) - _formatsCounts.Take(9).Sum();
        }

        Partitions = await _ctx.Partitions.OrderBy(static partition => partition.Name).ToListAsync();

        _partitionsLabels = await _ctx.Partitions.OrderByDescending(static o => o.Count)
                                      .Take(10)
                                      .Select(static v => v.Name)
                                      .ToArrayAsync();

        _partitionsCounts = await _ctx.Partitions.OrderByDescending(static o => o.Count)
                                      .Take(10)
                                      .Select(static x => x.Count)
                                      .ToListAsync();

        if(_partitionsLabels.Length >= 10)
        {
            _partitionsLabels[9] = "Other";

            _partitionsCounts[9] = _ctx.Partitions.Sum(static o => o.Count) - _partitionsCounts.Take(9).Sum();
        }

        Filesystems = await _ctx.Filesystems.OrderBy(static filesystem => filesystem.Name).ToListAsync();

        _filesystemsLabels = await _ctx.Filesystems.OrderByDescending(static o => o.Count)
                                       .Take(10)
                                       .Select(static v => v.Name)
                                       .ToArrayAsync();

        _filesystemsCounts = await _ctx.Filesystems.OrderByDescending(static o => o.Count)
                                       .Take(10)
                                       .Select(static x => x.Count)
                                       .ToListAsync();

        if(_filesystemsLabels.Length >= 10)
        {
            _filesystemsLabels[9] = "Other";

            _filesystemsCounts[9] = _ctx.Filesystems.Sum(static o => o.Count) - _filesystemsCounts.Take(9).Sum();
        }

        RealMedia    = [];
        VirtualMedia = [];

        await foreach(Media nvs in _ctx.Medias.AsAsyncEnumerable())
        {
            try
            {
                (string type, string subType) mediaType =
                    MediaType.MediaTypeToString((CommonTypes.MediaType)Enum.Parse(typeof(CommonTypes.MediaType),
                                                    nvs.Type));

                if(nvs.Real)
                {
                    RealMedia.Add(new MediaItem
                    {
                        Type    = mediaType.type,
                        SubType = mediaType.subType,
                        Count   = nvs.Count
                    });
                }
                else
                {
                    VirtualMedia.Add(new MediaItem
                    {
                        Type    = mediaType.type,
                        SubType = mediaType.subType,
                        Count   = nvs.Count
                    });
                }
            }
            catch
            {
                if(nvs.Real)
                {
                    RealMedia.Add(new MediaItem
                    {
                        Type    = nvs.Type,
                        SubType = null,
                        Count   = nvs.Count
                    });
                }
                else
                {
                    VirtualMedia.Add(new MediaItem
                    {
                        Type    = nvs.Type,
                        SubType = null,
                        Count   = nvs.Count
                    });
                }
            }
        }

        RealMedia    = RealMedia.OrderBy(static media => media.Type).ThenBy(static media => media.SubType).ToList();
        VirtualMedia = VirtualMedia.OrderBy(static media => media.Type).ThenBy(static media => media.SubType).ToList();

        Media[] virtualMedias = await _ctx.Medias.Where(static o => !o.Real)
                                          .OrderByDescending(static o => o.Count)
                                          .Take(10)
                                          .ToArrayAsync();

        foreach(Media media in virtualMedias)
        {
            try
            {
                (string type, string subType) mediaType =
                    MediaType.MediaTypeToString((CommonTypes.MediaType)Enum.Parse(typeof(CommonTypes.MediaType),
                                                    media.Type));

                media.Type = $"{mediaType.type} ({mediaType.subType})";
            }
            catch
            {
                // Could not get media type/subtype pair from type, so just leave it as is
            }
        }

        _virtualMediaLabels = virtualMedias.Select(static v => v.Type).ToArray();
        _virtualMediaCounts = virtualMedias.Select(static x => x.Count).ToList();

        if(_virtualMediaLabels.Length >= 10)
        {
            _virtualMediaLabels[9] = "Other";

            _virtualMediaCounts[9] = _ctx.Medias.Where(static o => !o.Real).Sum(static o => o.Count) -
                                     _virtualMediaCounts.Take(9).Sum();
        }

        Media[] realMedias = await _ctx.Medias.Where(static o => o.Real)
                                       .OrderByDescending(static o => o.Count)
                                       .Take(10)
                                       .ToArrayAsync();

        foreach(Media media in realMedias)
        {
            try
            {
                (string type, string subType) mediaType =
                    MediaType.MediaTypeToString((CommonTypes.MediaType)Enum.Parse(typeof(CommonTypes.MediaType),
                                                    media.Type));

                media.Type = $"{mediaType.type} ({mediaType.subType})";
            }
            catch
            {
                // Could not get media type/subtype pair from type, so just leave it as is
            }
        }

        _realMediaLabels = realMedias.Select(static v => v.Type).ToArray();
        _realMediaCounts = realMedias.Select(static x => x.Count).ToList();

        if(_realMediaLabels.Length >= 10)
        {
            _realMediaLabels[9] = "Other";

            _realMediaCounts[9] = _ctx.Medias.Where(static o => o.Real).Sum(static o => o.Count) -
                                  _realMediaCounts.Take(9).Sum();
        }

        Devices = await _ctx.DeviceStats.Include(static deviceStat => deviceStat.Report)
                            .Select(static device => new DeviceItem
                             {
                                 Manufacturer = device.Manufacturer,
                                 Model        = device.Model,
                                 Revision     = device.Revision,
                                 Bus          = device.Bus,
                                 ReportId     = device.Report != null && device.Report.Id != 0 ? device.Report.Id : 0
                             })
                            .ToListAsync();

        Devices = Devices.OrderBy(static device => device.Manufacturer)
                         .ThenBy(static device => device.Model)
                         .ThenBy(static device => device.Revision)
                         .ThenBy(static device => device.Bus)
                         .ToList();

        var data = await _ctx.DeviceStats.Select(static d => d.Bus)
                             .Distinct()
                             .Select(deviceBus => new
                              {
                                  deviceBus,
                                  deviceBusCount = _ctx.DeviceStats.LongCount(d => d.Bus == deviceBus)
                              })
                             .Select(static t => new
                              {
                                  Name  = t.deviceBus,
                                  Count = t.deviceBusCount
                              })
                             .ToListAsync();

        _devicesByBusLabels = data.OrderByDescending(static o => o.Count).Take(10).Select(static v => v.Name).ToArray();
        _devicesByBusCounts = data.OrderByDescending(static o => o.Count).Take(10).Select(static x => x.Count).ToList();

        if(_devicesByBusLabels.Length >= 10)
        {
            _devicesByBusLabels[9] = "Other";

            _devicesByBusCounts[9] = data.Sum(static o => o.Count) - _devicesByBusCounts.Take(9).Sum();
        }

        List<DeviceStat> devices = await _ctx.DeviceStats
                                             .Where(static d => d.Manufacturer != null && d.Manufacturer != "")
                                             .ToListAsync();

        data = devices.Select(static d => d.Manufacturer!.ToLowerInvariant())
                      .Distinct()
                      .Select(manufacturer => new
                       {
                           manufacturer,
                           manufacturerCount =
                               devices.LongCount(d => d.Manufacturer?.ToLowerInvariant() == manufacturer)
                       })
                      .Select(static t => new
                       {
                           Name  = t.manufacturer,
                           Count = t.manufacturerCount
                       })
                      .ToList();

        _devicesByManufacturerLabels =
            data.OrderByDescending(static o => o.Count).Take(10).Select(static v => v.Name).ToArray();

        _devicesByManufacturerCounts =
            data.OrderByDescending(static o => o.Count).Take(10).Select(static x => x.Count).ToList();

        if(_devicesByManufacturerLabels.Length < 10) return;

        _devicesByManufacturerLabels[9] = "Other";
        _devicesByManufacturerCounts[9] = data.Sum(static o => o.Count) - _devicesByManufacturerCounts.Take(9).Sum();

#pragma warning disable CS8604 // Possible null reference argument.

        await
            Task.WhenAll(HandleRedraw(_operatingSystemsChart, _operatingSystemsLabels, GetOperatingSystemsChartDataset),
                         HandleRedraw(_linuxChart,            _linuxLabels,            GetLinuxChartDataset),
                         HandleRedraw(_macosChart,            _macosLabels,            GetMacosChartDataset),
                         HandleRedraw(_windowsChart,          _windowsLabels,          GetWindowsChartDataset),
                         HandleRedraw(_versionsChart,         _versionsLabels,         GetVersionsChartDataset),
                         HandleRedraw(_commandsChart,         _commandsLabels,         GetCommandsChartDataset),
                         HandleRedraw(_filtersChart,          _filtersLabels,          GetFiltersChartDataset),
                         HandleRedraw(_formatsChart,          _formatsLabels,          GetFormatsChartDataset),
                         HandleRedraw(_partitionsChart,       _partitionsLabels,       GetPartitionsChartDataset),
                         HandleRedraw(_filesystemsChart,      _filesystemsLabels,      GetFilesystemsChartDataset),
                         HandleRedraw(_virtualMediaChart,     _virtualMediaLabels,     GetVirtualMediaChartDataset),
                         HandleRedraw(_realMediaChart,        _realMediaLabels,        GetRealMediaChartDataset),
                         HandleRedraw(_devicesByBusChart,     _devicesByBusLabels,     GetDevicesByBusChartDataset),
                         HandleRedraw(_devicesByManufacturerChart,
                                      _devicesByManufacturerLabels,
                                      GetDevicesByManufacturerChartDataset));
#pragma warning restore CS8604 // Possible null reference argument.

        // Upstream: https://github.com/Megabit/Blazorise/issues/5491
        _operatingSystemsCarousel.Interval = 5000;
        _devicesCarousel.Interval          = 5000;
    }

    static async Task HandleRedraw<TDataSet, TItem, TOptions, TModel>(
        BaseChart<TDataSet, TItem, TOptions, TModel> chart, string[] labels, Func<TDataSet> getDataSet)
        where TDataSet : ChartDataset<TItem> where TOptions : ChartOptions where TModel : ChartModel
    {
        await chart.Clear();

        await chart.AddLabelsDatasetsAndUpdate(labels, getDataSet());
    }

    PieChartDataset<long> GetOperatingSystemsChartDataset() => new()
    {
        Label           = "Operating systems",
        Data            = _operatingSystemsCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetLinuxChartDataset() => new()
    {
        Label           = $"Top {_linuxLabels.Length} Linux versions",
        Data            = _linuxCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetMacosChartDataset() => new()
    {
        Label           = $"Top {_macosLabels.Length} macOS versions",
        Data            = _macosCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetWindowsChartDataset() => new()
    {
        Label           = $"Top {_windowsLabels.Length} Windows versions",
        Data            = _windowsCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetVersionsChartDataset() => new()
    {
        Label           = $"Top {_versionsLabels.Length} Aaru versions",
        Data            = _versionsCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetCommandsChartDataset() => new()
    {
        Label           = $"Top {_commandsLabels.Length} used commands",
        Data            = _commandsCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetFiltersChartDataset() => new()
    {
        Label           = $"Top {_filtersLabels.Length} filters found",
        Data            = _filtersCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetFormatsChartDataset() => new()
    {
        Label           = $"Top {_formatsLabels.Length} media image formats found",
        Data            = _formatsCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetPartitionsChartDataset() => new()
    {
        Label           = $"Top {_partitionsLabels.Length} partitioning schemes found",
        Data            = _partitionsCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetFilesystemsChartDataset() => new()
    {
        Label           = $"Top {_filesystemsLabels.Length} filesystems found",
        Data            = _filesystemsCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetVirtualMediaChartDataset() => new()
    {
        Label           = $"Top {_virtualMediaLabels.Length} media types found in images",
        Data            = _virtualMediaCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetRealMediaChartDataset() => new()
    {
        Label           = $"Top {_realMediaLabels.Length} media types found in devices",
        Data            = _realMediaCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetDevicesByBusChartDataset() => new()
    {
        Label           = $"Top {_devicesByBusLabels.Length} devices by bus",
        Data            = _devicesByBusCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetDevicesByManufacturerChartDataset() => new()
    {
        Label           = $"Top {_devicesByManufacturerLabels.Length} devices by manufacturers",
        Data            = _devicesByManufacturerCounts,
        BackgroundColor = _backgroundColors,
        BorderColor     = _borderColors,
        BorderWidth     = 1
    };

    static string GetPlatformName(string name, string version) =>
        DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), name), version);
}