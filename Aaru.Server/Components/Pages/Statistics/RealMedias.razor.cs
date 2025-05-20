using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Media = Aaru.Server.Database.Models.Media;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class RealMedias
{
    bool            _isAlreadyInitialized;
    PieChart        _realMediaChart;
    List<double?>   _realMediaCounts = [];
    List<string>    _realMediaLabels = [];
    List<MediaItem> RealMedia { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        RealMedia = [];

        foreach(Media media in ctx.Medias.Where(static o => o.Real).OrderByDescending(static o => o.Count))
        {
            try
            {
                (string type, string subType) mediaType =
                    MediaType.MediaTypeToString((CommonTypes.MediaType)Enum.Parse(typeof(CommonTypes.MediaType),
                                                    media.Type));

                RealMedia.Add(new MediaItem
                {
                    Type    = mediaType.type,
                    SubType = mediaType.subType,
                    Count   = media.Count
                });
            }
            catch {}
        }

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        Media[] realMedias = await ctx.Medias.Where(static o => o.Real)
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

        _realMediaLabels = realMedias.Select(static v => v.Type).ToList();
        _realMediaCounts = realMedias.Select(static x => (double?)x.Count).ToList();

        if(_realMediaLabels.Count >= 10)
        {
            _realMediaLabels[9] = "Other";

            _realMediaCounts[9] = ctx.Medias.Where(static o => o.Real).Sum(static o => o.Count) -
                                  _realMediaCounts.Take(9).Sum();
        }

        PieChartOptions pieChartOptions = new()
        {
            Responsive = true
        };

        pieChartOptions.Plugins.Title.Text    = $"Top {_realMediaLabels.Count} media types found in devices";
        pieChartOptions.Plugins.Title.Display = true;

        var chartData = new ChartData
        {
            Labels = _realMediaLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data            = _realMediaCounts,
                    BackgroundColor = Common.BackgroundColors,
                    BorderColor     = Common.BorderColors,
                    BorderWidth     = [1]
                }
            ]
        };

        await _realMediaChart.InitializeAsync(chartData, pieChartOptions);
    }
}