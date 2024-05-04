using Aaru.CommonTypes.Interop;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using PlatformID = Aaru.CommonTypes.Interop.PlatformID;

namespace Aaru.Server.New.Components.Pages;

public partial class Stats
{
    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        // TOOD: Cache real OS name in database, lookups would be much faster
        await using DbContext _ctx = await DbContextFactory.CreateDbContextAsync();

        var operatingSystems = (await _ctx.OperatingSystems.OrderBy(static os => os.Name)
                                          .ThenBy(static os => os.Version)
                                          .Select(static nvs => new NameValueStats
                                           {
                                               name =
                                                   $"{GetPlatformName(nvs.Name, nvs.Version)}{(string.IsNullOrEmpty(nvs.Version) ? "" : " ")}{nvs.Version}",
                                               Value = nvs.Count
                                           })
                                          .ToListAsync()).OrderBy(static os => os.name)
                                                         .ToList();

        var versions = (await _ctx.Versions.Select(static nvs => new NameValueStats
                                   {
                                       name  = nvs.Name == "previous" ? "Previous than 3.4.99.0" : nvs.Name,
                                       Value = nvs.Count
                                   })
                                  .ToListAsync()).OrderBy(static version => version.name)
                                                 .ToList();

        List<Command> commands = await _ctx.Commands.OrderBy(static c => c.Name).ToListAsync();

        List<Filter> filters = await _ctx.Filters.OrderBy(static filter => filter.Name).ToListAsync();

        List<MediaFormat> mediaImages = await _ctx.MediaFormats.OrderBy(static format => format.Name).ToListAsync();

        List<Partition> partitions = await _ctx.Partitions.OrderBy(static partition => partition.Name).ToListAsync();

        List<Filesystem> filesystems =
            await _ctx.Filesystems.OrderBy(static filesystem => filesystem.Name).ToListAsync();

        List<MediaItem> realMedia    = [];
        List<MediaItem> virtualMedia = [];

        await foreach(Media nvs in _ctx.Medias.AsAsyncEnumerable())
        {
            try
            {
                (string type, string subType) mediaType =
                    MediaType.MediaTypeToString((CommonTypes.MediaType)Enum.Parse(typeof(CommonTypes.MediaType),
                                                    nvs.Type));

                if(nvs.Real)
                {
                    realMedia.Add(new MediaItem
                    {
                        Type    = mediaType.type,
                        SubType = mediaType.subType,
                        Count   = nvs.Count
                    });
                }
                else
                {
                    virtualMedia.Add(new MediaItem
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
                    realMedia.Add(new MediaItem
                    {
                        Type    = nvs.Type,
                        SubType = null,
                        Count   = nvs.Count
                    });
                }
                else
                {
                    virtualMedia.Add(new MediaItem
                    {
                        Type    = nvs.Type,
                        SubType = null,
                        Count   = nvs.Count
                    });
                }
            }
        }

        realMedia    = realMedia.OrderBy(static media => media.Type).ThenBy(static media => media.SubType).ToList();
        virtualMedia = virtualMedia.OrderBy(static media => media.Type).ThenBy(static media => media.SubType).ToList();


        List<DeviceItem> devices = await _ctx.DeviceStats.Include(static deviceStat => deviceStat.Report)
                                             .Select(static device => new DeviceItem
                                              {
                                                  Manufacturer = device.Manufacturer,
                                                  Model        = device.Model,
                                                  Revision     = device.Revision,
                                                  Bus          = device.Bus,
                                                  ReportId = device.Report != null && device.Report.Id != 0
                                                                 ? device.Report.Id
                                                                 : 0
                                              })
                                             .ToListAsync();

        devices = devices.OrderBy(static device => device.Manufacturer)
                         .ThenBy(static device => device.Model)
                         .ThenBy(static device => device.Revision)
                         .ThenBy(static device => device.Bus)
                         .ToList();
    }

    static string GetPlatformName(string name, string version) =>
        DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), name), version);
}