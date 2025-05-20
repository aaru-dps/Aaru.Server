using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Media = Aaru.Server.Database.Models.Media;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class VirtualMedias
{
    bool            _isAlreadyInitialized;
    PieChart        _virtualMediaChart;
    List<double?>   _virtualMediaCounts = [];
    List<string>    _virtualMediaLabels = [];
    List<MediaItem> VirtualMedia { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        VirtualMedia = [];

        foreach(Media media in ctx.Medias.Where(static o => !o.Real).OrderByDescending(static o => o.Count))
        {
            try
            {
                (string type, string subType) mediaType =
                    MediaType.MediaTypeToString((CommonTypes.MediaType)Enum.Parse(typeof(CommonTypes.MediaType),
                                                    media.Type));

                VirtualMedia.Add(new MediaItem
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

        Media[] virtualMedias = await ctx.Medias.Where(static o => !o.Real)
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

        _virtualMediaLabels = virtualMedias.Select(static v => v.Type).ToList();
        _virtualMediaCounts = virtualMedias.Select(static x => (double?)x.Count).ToList();

        if(_virtualMediaLabels.Count >= 10)
        {
            _virtualMediaLabels[9] = "Other";

            _virtualMediaCounts[9] = ctx.Medias.Where(static o => !o.Real).Sum(static o => o.Count) -
                                     _virtualMediaCounts.Take(9).Sum();
        }

        PieChartOptions pieChartOptions = new()
        {
            Responsive = true
        };

        pieChartOptions.Plugins.Title.Text    = $"Top {_virtualMediaLabels.Count} media types found in images";
        pieChartOptions.Plugins.Title.Display = true;

        var chartData = new ChartData
        {
            Labels = _virtualMediaLabels,
            Datasets =
            [
                new PieChartDataset
                {
                    Data = _virtualMediaCounts
                }
            ]
        };

        await _virtualMediaChart.InitializeAsync(chartData, pieChartOptions);
    }
}