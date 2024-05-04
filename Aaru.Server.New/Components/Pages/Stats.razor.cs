using Aaru.CommonTypes.Interop;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using PlatformID = Aaru.CommonTypes.Interop.PlatformID;

namespace Aaru.Server.New.Components.Pages;

public partial class Stats
{
    List<NameValueStats> OperatingSystems { get; set; } = [];

    List<NameValueStats> Versions { get; set; } = [];

    List<Command> Commands { get; set; } = [];

    List<Filter> Filters { get; set; } = [];

    List<MediaFormat> MediaImages { get; set; } = [];

    List<Partition> Partitions { get; set; } = [];

    List<Filesystem> Filesystems  { get; set; } = [];
    List<MediaItem>  RealMedia    { get; set; } = [];
    List<MediaItem>  VirtualMedia { get; set; } = [];
    List<DeviceItem> Devices      { get; set; } = [];

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

        Versions = (await _ctx.Versions.Select(static nvs => new NameValueStats
                               {
                                   name  = nvs.Name == "previous" ? "Previous than 3.4.99.0" : nvs.Name,
                                   Value = nvs.Count
                               })
                              .ToListAsync()).OrderBy(static version => version.name)
                                             .ToList();

        Commands = await _ctx.Commands.OrderBy(static c => c.Name).ToListAsync();

        Filters = await _ctx.Filters.OrderBy(static filter => filter.Name).ToListAsync();

        MediaImages = await _ctx.MediaFormats.OrderBy(static format => format.Name).ToListAsync();

        Partitions = await _ctx.Partitions.OrderBy(static partition => partition.Name).ToListAsync();

        Filesystems = await _ctx.Filesystems.OrderBy(static filesystem => filesystem.Name).ToListAsync();

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
    }

    static string GetPlatformName(string name, string version) =>
        DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), name), version);
}