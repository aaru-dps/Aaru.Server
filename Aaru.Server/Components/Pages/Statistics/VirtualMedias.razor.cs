using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Media = Aaru.Server.Database.Models.Media;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class VirtualMedias
{
    PieChart<long>? _virtualMediaChart;
    List<long>      _virtualMediaCounts = [];
    string[]        _virtualMediaLabels = [];
    List<MediaItem> VirtualMedia { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

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

        _virtualMediaLabels = virtualMedias.Select(static v => v.Type).ToArray();
        _virtualMediaCounts = virtualMedias.Select(static x => x.Count).ToList();

        if(_virtualMediaLabels.Length >= 10)
        {
            _virtualMediaLabels[9] = "Other";

            _virtualMediaCounts[9] = ctx.Medias.Where(static o => !o.Real).Sum(static o => o.Count) -
                                     _virtualMediaCounts.Take(9).Sum();
        }

#pragma warning disable CS8604 // Possible null reference argument.
        await Common.HandleRedraw(_virtualMediaChart, _virtualMediaLabels, GetVirtualMediaChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    PieChartDataset<long> GetVirtualMediaChartDataset() => new()
    {
        Label           = $"Top {_virtualMediaLabels.Length} media types found in images",
        Data            = _virtualMediaCounts,
        BackgroundColor = Common._backgroundColors,
        BorderColor     = Common._borderColors,
        BorderWidth     = 1
    };
}