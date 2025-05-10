using Aaru.Server.Database.Models;
using Blazorise;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class Devices
{
    PieChart<long>?  _devicesByBusChart;
    List<long>       _devicesByBusCounts = [];
    string[]         _devicesByBusLabels = [];
    PieChart<long>?  _devicesByManufacturerChart;
    List<long>       _devicesByManufacturerCounts = [];
    string[]         _devicesByManufacturerLabels = [];
    Carousel?        _devicesCarousel;
    bool             _isAlreadyInitialized;
    List<DeviceItem> DevicesList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        DevicesList = DevicesList.OrderBy(static device => device.Manufacturer)
                                 .ThenBy(static device => device.Model)
                                 .ThenBy(static device => device.Revision)
                                 .ThenBy(static device => device.Bus)
                                 .ToList();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        var data = await ctx.DeviceStats.Select(static d => d.Bus)
                            .Distinct()
                            .Select(deviceBus => new
                             {
                                 deviceBus,
                                 deviceBusCount = ctx.DeviceStats.LongCount(d => d.Bus == deviceBus)
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

        List<DeviceStat> devices = await ctx.DeviceStats
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
        await Common.HandleRedrawAsync(_devicesByBusChart, _devicesByBusLabels, GetDevicesByBusChartDataset);

        await Common.HandleRedrawAsync(_devicesByManufacturerChart,
                                       _devicesByManufacturerLabels,
                                       GetDevicesByManufacturerChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.

        // Upstream: https://github.com/Megabit/Blazorise/issues/5491
        _devicesCarousel.Interval = 5000;
    }

    PieChartDataset<long> GetDevicesByBusChartDataset() => new()
    {
        Label           = $"Top {_devicesByBusLabels.Length} devices by bus",
        Data            = _devicesByBusCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };

    PieChartDataset<long> GetDevicesByManufacturerChartDataset() => new()
    {
        Label           = $"Top {_devicesByManufacturerLabels.Length} devices by manufacturers",
        Data            = _devicesByManufacturerCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };
}