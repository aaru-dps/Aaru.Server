using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Media = Aaru.Server.Database.Models.Media;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class RealMedias
{
    bool            _isAlreadyInitialized;
    PieChart<long>? _realMediaChart;
    List<long>      _realMediaCounts = [];
    string[]        _realMediaLabels = [];
    List<MediaItem> RealMedia { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        RealMedia = [];

        foreach(Media media in ctx.Medias.Where(static o => o.Real).OrderByDescending(static o => o.Count))
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

        _realMediaLabels = realMedias.Select(static v => v.Type).ToArray();
        _realMediaCounts = realMedias.Select(static x => x.Count).ToList();

        if(_realMediaLabels.Length >= 10)
        {
            _realMediaLabels[9] = "Other";

            _realMediaCounts[9] = ctx.Medias.Where(static o => o.Real).Sum(static o => o.Count) -
                                  _realMediaCounts.Take(9).Sum();
        }


#pragma warning disable CS8604 // Possible null reference argument.
        await Common.HandleRedrawAsync(_realMediaChart, _realMediaLabels, GetRealMediaChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    PieChartDataset<long> GetRealMediaChartDataset() => new()
    {
        Label           = $"Top {_realMediaLabels.Length} media types found in devices",
        Data            = _realMediaCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };
}