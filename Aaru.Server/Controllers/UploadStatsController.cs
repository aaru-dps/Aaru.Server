// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : UploadStatsController.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Handles statistics uploads.
//
// --[ License ] --------------------------------------------------------------
//
//     This library is free software; you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as
//     published by the Free Software Foundation; either version 2.1 of the
//     License, or (at your option) any later version.
//
//     This library is distributed in the hope that it will be useful, but
//     WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//     Lesser General Public License for more details.
//
//     You should have received a copy of the GNU Lesser General Public
//     License along with this library; if not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright © 2011-2024 Natalia Portillo
// ****************************************************************************/

using System.Diagnostics;
using System.Net;
using System.Xml.Serialization;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using Aaru.Server.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DbContext = Aaru.Server.Database.DbContext;
using OperatingSystem = Aaru.Server.Database.Models.OperatingSystem;
using Version = Aaru.Server.Database.Models.Version;

namespace Aaru.Server.Controllers;

public sealed class UploadStatsController : ControllerBase
{
    readonly DbContext _ctx;

    public UploadStatsController(DbContext ctx) => _ctx = ctx;

    /// <summary>Receives statistics from Aaru.Core, processes them and adds them to a server-side global statistics XML</summary>
    /// <returns>HTTP response</returns>
    [Route("api/uploadstats")]
    [HttpPost]
    public async Task<IActionResult> UploadStats()
    {
        var response = new ContentResult
        {
            StatusCode  = (int)HttpStatusCode.OK,
            ContentType = "text/plain"
        };

        try
        {
            var         newStats = new Stats();
            HttpRequest request  = HttpContext.Request;

            var xs = new XmlSerializer(newStats.GetType());

            newStats = (Stats)xs.Deserialize(new StringReader(await new StreamReader(request.Body).ReadToEndAsync()));

            if(newStats == null)
            {
                response.Content = "notstats";

                return response;
            }

            await StatsConverter.ConvertAsync(newStats);

            response.Content = "ok";

            return response;
        }
        catch(Exception)
        {
#if DEBUG
            if(Debugger.IsAttached) throw;
#endif
            response.Content = "error";

            return response;
        }
    }

    /// <summary>Receives a report from Aaru.Core, verifies it's in the correct format and stores it on the server</summary>
    /// <returns>HTTP response</returns>
    [Route("api/uploadstatsv2")]
    [HttpPost]
    public async Task<IActionResult> UploadStatsV2()
    {
        var response = new ContentResult
        {
            StatusCode  = (int)HttpStatusCode.OK,
            ContentType = "text/plain"
        };

        try
        {
            HttpRequest request = HttpContext.Request;

            var      sr          = new StreamReader(request.Body);
            string   statsString = await sr.ReadToEndAsync();
            StatsDto newstats    = JsonConvert.DeserializeObject<StatsDto>(statsString);

            if(newstats == null)
            {
                response.Content = "notstats";

                return response;
            }

            if(newstats.Commands != null)
            {
                foreach(NameValueStats nvs in newstats.Commands)
                {
                    if(nvs.name == "analyze") nvs.name = "fs-info";

                    Command? existing = await _ctx.Commands.FirstOrDefaultAsync(c => c.Name == nvs.name);

                    if(existing == null)
                    {
                        _ctx.Commands.Add(new Command
                        {
                            Name  = nvs.name,
                            Count = nvs.Value
                        });
                    }
                    else
                        existing.Count += nvs.Value;
                }
            }

            if(newstats.Versions != null)
            {
                foreach(NameValueStats nvs in newstats.Versions)
                {
                    Version? existing = await _ctx.Versions.FirstOrDefaultAsync(c => c.Name == nvs.name);

                    if(existing == null)
                    {
                        _ctx.Versions.Add(new Version
                        {
                            Name  = nvs.name,
                            Count = nvs.Value
                        });
                    }
                    else
                        existing.Count += nvs.Value;
                }
            }

            if(newstats.Filesystems != null)
            {
                foreach(NameValueStats nvs in newstats.Filesystems)
                {
                    Filesystem? existing = await _ctx.Filesystems.FirstOrDefaultAsync(c => c.Name == nvs.name);

                    if(existing == null)
                    {
                        _ctx.Filesystems.Add(new Filesystem
                        {
                            Name  = nvs.name,
                            Count = nvs.Value
                        });
                    }
                    else
                        existing.Count += nvs.Value;
                }
            }

            if(newstats.Partitions != null)
            {
                foreach(NameValueStats nvs in newstats.Partitions)
                {
                    Partition? existing = await _ctx.Partitions.FirstOrDefaultAsync(c => c.Name == nvs.name);

                    if(existing == null)
                    {
                        _ctx.Partitions.Add(new Partition
                        {
                            Name  = nvs.name,
                            Count = nvs.Value
                        });
                    }
                    else
                        existing.Count += nvs.Value;
                }
            }

            if(newstats.MediaFormats != null)
            {
                foreach(NameValueStats nvs in newstats.MediaFormats)
                {
                    MediaFormat? existing = await _ctx.MediaFormats.FirstOrDefaultAsync(c => c.Name == nvs.name);

                    if(existing == null)
                    {
                        _ctx.MediaFormats.Add(new MediaFormat
                        {
                            Name  = nvs.name,
                            Count = nvs.Value
                        });
                    }
                    else
                        existing.Count += nvs.Value;
                }
            }

            if(newstats.Filters != null)
            {
                foreach(NameValueStats nvs in newstats.Filters)
                {
                    Filter? existing = await _ctx.Filters.FirstOrDefaultAsync(c => c.Name == nvs.name);

                    if(existing == null)
                    {
                        _ctx.Filters.Add(new Filter
                        {
                            Name  = nvs.name,
                            Count = nvs.Value
                        });
                    }
                    else
                        existing.Count += nvs.Value;
                }
            }

            if(newstats.OperatingSystems != null)
            {
                foreach(OsStats operatingSystem in newstats.OperatingSystems)
                {
                    OperatingSystem? existing =
                        await _ctx.OperatingSystems.FirstOrDefaultAsync(c => c.Name    == operatingSystem.name &&
                                                                             c.Version == operatingSystem.version);

                    if(existing == null)
                    {
                        _ctx.OperatingSystems.Add(new OperatingSystem
                        {
                            Name    = operatingSystem.name,
                            Version = operatingSystem.version,
                            Count   = operatingSystem.Value
                        });
                    }
                    else
                        existing.Count += operatingSystem.Value;
                }
            }

            if(newstats.Medias != null)
            {
                foreach(MediaStats media in newstats.Medias)
                {
                    Media? existing =
                        await _ctx.Medias.FirstOrDefaultAsync(c => c.Type == media.MediaType && c.Real == media.real);

                    if(existing == null)
                    {
                        _ctx.Medias.Add(new Media
                        {
                            Type  = media.MediaType,
                            Real  = media.real,
                            Count = media.Value
                        });
                    }
                    else
                        existing.Count += media.Value;
                }
            }

            if(newstats.Devices != null)
            {
                foreach(DeviceStats device in newstats.Devices)
                {
                    DeviceStat? existing =
                        await _ctx.DeviceStats.FirstOrDefaultAsync(c => c.Bus          == device.Bus          &&
                                                                        c.Manufacturer == device.Manufacturer &&
                                                                        c.Model        == device.Model        &&
                                                                        c.Revision     == device.Revision);

                    if(existing == null)
                    {
                        _ctx.DeviceStats.Add(new DeviceStat
                        {
                            Bus          = device.Bus,
                            Manufacturer = device.Manufacturer,
                            Model        = device.Model,
                            Revision     = device.Revision
                        });
                    }
                }
            }

            if(newstats.RemoteApplications != null)
            {
                foreach(OsStats application in newstats.RemoteApplications)
                {
                    RemoteApplication? existing =
                        await _ctx.RemoteApplications.FirstOrDefaultAsync(c => c.Name    == application.name &&
                                                                               c.Version == application.version);

                    if(existing == null)
                    {
                        _ctx.RemoteApplications.Add(new RemoteApplication
                        {
                            Name    = application.name,
                            Version = application.version,
                            Count   = application.Value
                        });
                    }
                    else
                        existing.Count += application.Value;
                }
            }

            if(newstats.RemoteArchitectures != null)
            {
                foreach(NameValueStats nvs in newstats.RemoteArchitectures)
                {
                    RemoteArchitecture? existing =
                        await _ctx.RemoteArchitectures.FirstOrDefaultAsync(c => c.Name == nvs.name);

                    if(existing == null)
                    {
                        _ctx.RemoteArchitectures.Add(new RemoteArchitecture
                        {
                            Name  = nvs.name,
                            Count = nvs.Value
                        });
                    }
                    else
                        existing.Count += nvs.Value;
                }
            }

            if(newstats.RemoteOperatingSystems != null)
            {
                foreach(OsStats remoteOperatingSystem in newstats.RemoteOperatingSystems)
                {
                    RemoteOperatingSystem? existing =
                        await _ctx.RemoteOperatingSystems.FirstOrDefaultAsync(c =>
                                                                                  c.Name ==
                                                                                  remoteOperatingSystem.name &&
                                                                                  c.Version ==
                                                                                  remoteOperatingSystem.version);

                    if(existing == null)
                    {
                        _ctx.RemoteOperatingSystems.Add(new RemoteOperatingSystem
                        {
                            Name    = remoteOperatingSystem.name,
                            Version = remoteOperatingSystem.version,
                            Count   = remoteOperatingSystem.Value
                        });
                    }
                    else
                        existing.Count += remoteOperatingSystem.Value;
                }
            }

            await _ctx.SaveChangesAsync();

            response.Content = "ok";

            return response;
        }

        // ReSharper disable once RedundantCatchClause
        catch
        {
#if DEBUG
            if(Debugger.IsAttached) throw;
#endif
            response.Content = "error";

            return response;
        }
    }
}