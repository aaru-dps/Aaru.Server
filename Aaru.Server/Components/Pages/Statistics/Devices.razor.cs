using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class Devices
{
    PieChart      _devicesByBusChart;
    List<double?> _devicesByBusCounts = [];
    List<string>  _devicesByBusLabels = [];
    PieChart      _devicesByManufacturerChart;
    List<double?> _devicesByManufacturerCounts = [];
    List<string>  _devicesByManufacturerLabels = [];

    //Carousel?        _devicesCarousel;
    bool             _isAlreadyInitialized;
    List<DeviceItem> DevicesList { get; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        DevicesList.AddRange(ctx.DeviceStats.OrderBy(static device => device.Manufacturer)
                                .ThenBy(static device => device.Model)
                                .ThenBy(static device => device.Revision)
                                .ThenBy(static device => device.Bus)
                                .Select(static dev => new DeviceItem
                                 {
                                     Manufacturer = dev.Manufacturer,
                                     Model        = dev.Model,
                                     Revision     = dev.Revision,
                                     Bus          = dev.Bus,
                                     ReportId     = dev.Report != null && dev.Report.Id != 0 ? dev.Report.Id : 0
                                 }));

        await base.OnInitializedAsync();
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

        _devicesByBusLabels = data.OrderByDescending(static o => o.Count).Take(10).Select(static v => v.Name).ToList();

        _devicesByBusCounts = data.OrderByDescending(static o => o.Count)
                                  .Take(10)
                                  .Select(static x => (double?)x.Count)
                                  .ToList();

        if(_devicesByBusLabels.Count >= 10)
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
            data.OrderByDescending(static o => o.Count).Take(10).Select(static v => v.Name).ToList();

        _devicesByManufacturerCounts = data.OrderByDescending(static o => o.Count)
                                           .Take(10)
                                           .Select(static x => (double?)x.Count)
                                           .ToList();

        if(_devicesByManufacturerLabels.Count < 10) return;

        _devicesByManufacturerLabels[9] = "Other";
        _devicesByManufacturerCounts[9] = data.Sum(static o => o.Count) - _devicesByManufacturerCounts.Take(9).Sum();

        PieChartOptions devicesByBusChartOptions = new()
        {
            Responsive = true
        };

        devicesByBusChartOptions.Plugins.Title.Text    = $"Top {_devicesByBusLabels.Count} devices by bus";
        devicesByBusChartOptions.Plugins.Title.Display = true;

        var devicesByBusChartData = new ChartData
        {
            Labels = _devicesByBusLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _devicesByBusCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _devicesByBusChart.InitializeAsync(devicesByBusChartData, devicesByBusChartOptions);

        PieChartOptions devicesByManufacturerChartOptions = new()
        {
            Responsive = true
        };

        devicesByManufacturerChartOptions.Plugins.Title.Text =
            $"Top {_devicesByManufacturerLabels.Count} devices by manufacturers";

        devicesByManufacturerChartOptions.Plugins.Title.Display = true;

        var devicesByManufacturerChartData = new ChartData
        {
            Labels = _devicesByManufacturerLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _devicesByManufacturerCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _devicesByManufacturerChart.InitializeAsync(devicesByManufacturerChartData,
                                                          devicesByManufacturerChartOptions);
    }
}