// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : StatsController.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Fetches statistics for Razor views.
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
// Copyright © 2011-2020 Natalia Portillo
// ****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Aaru.Server.Models;
using DiscImageChef.CommonTypes.Interop;
using DiscImageChef.CommonTypes.Metadata;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OperatingSystem = Aaru.Server.Models.OperatingSystem;
using PlatformID = DiscImageChef.CommonTypes.Interop.PlatformID;
using Version = Aaru.Server.Models.Version;

namespace Aaru.Server.Controllers
{
    /// <summary>Renders a page with statistics, list of media type, devices, etc</summary>
    public class StatsController : Controller
    {
        readonly DicServerContext    ctx;
        readonly IWebHostEnvironment env;

        public StatsController(IWebHostEnvironment environment, DicServerContext context)
        {
            env = environment;
            ctx = context;
        }

        public ActionResult Index()
        {
            ViewBag.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            try
            {
                if(System.IO.File.Exists(Path.Combine(env.ContentRootPath ?? throw new InvalidOperationException(),
                                                      "Statistics", "Statistics.xml")))
                    try
                    {
                        var statistics = new Stats();

                        var xs = new XmlSerializer(statistics.GetType());

                        FileStream fs =
                            WaitForFile(Path.Combine(env.ContentRootPath ?? throw new InvalidOperationException(), "Statistics", "Statistics.xml"),
                                        FileMode.Open, FileAccess.Read, FileShare.Read);

                        statistics = (Stats)xs.Deserialize(fs);
                        fs.Close();

                        StatsConverter.Convert(statistics);

                        System.IO.File.Delete(Path.Combine(env.ContentRootPath ?? throw new InvalidOperationException(),
                                                           "Statistics", "Statistics.xml"));
                    }
                    catch(XmlException)
                    {
                        // Do nothing
                    }

                if(ctx.OperatingSystems.Any())
                {
                    List<NameValueStats> operatingSystems = new List<NameValueStats>();

                    foreach(Models.OperatingSystem nvs in ctx.OperatingSystems)
                        operatingSystems.Add(new NameValueStats
                        {
                            name =
                                $"{DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), nvs.Name), nvs.Version)}{(string.IsNullOrEmpty(nvs.Version) ? "" : " ")}{nvs.Version}",
                            Value = nvs.Count
                        });

                    ViewBag.repOperatingSystems = operatingSystems.OrderBy(os => os.name).ToList();
                }

                if(ctx.Versions.Any())
                {
                    List<NameValueStats> versions = new List<NameValueStats>();

                    foreach(Models.Version nvs in ctx.Versions)
                        versions.Add(new NameValueStats
                        {
                            name = nvs.Name == "previous" ? "Previous than 3.4.99.0" : nvs.Name, Value = nvs.Count
                        });

                    ViewBag.repVersions = versions.OrderBy(ver => ver.name).ToList();
                }

                if(ctx.Commands.Any())
                    ViewBag.repCommands = ctx.Commands.OrderBy(c => c.Name).ToList();

                if(ctx.Filters.Any())
                    ViewBag.repFilters = ctx.Filters.OrderBy(filter => filter.Name).ToList();

                if(ctx.MediaFormats.Any())
                    ViewBag.repMediaImages = ctx.MediaFormats.OrderBy(filter => filter.Name).ToList();

                if(ctx.Partitions.Any())
                    ViewBag.repPartitions = ctx.Partitions.OrderBy(filter => filter.Name).ToList();

                if(ctx.Filesystems.Any())
                    ViewBag.repFilesystems = ctx.Filesystems.OrderBy(filter => filter.Name).ToList();

                if(ctx.Medias.Any())
                {
                    List<MediaItem> realMedia    = new List<MediaItem>();
                    List<MediaItem> virtualMedia = new List<MediaItem>();

                    foreach(Media nvs in ctx.Medias)
                        try
                        {
                            (string type, string subType) mediaType =
                                MediaType.MediaTypeToString((CommonTypes.MediaType)
                                                            Enum.Parse(typeof(CommonTypes.MediaType), nvs.Type));

                            if(nvs.Real)
                                realMedia.Add(new MediaItem
                                {
                                    Type = mediaType.type, SubType = mediaType.subType, Count = nvs.Count
                                });
                            else
                                virtualMedia.Add(new MediaItem
                                {
                                    Type = mediaType.type, SubType = mediaType.subType, Count = nvs.Count
                                });
                        }
                        catch
                        {
                            if(nvs.Real)
                                realMedia.Add(new MediaItem
                                {
                                    Type = nvs.Type, SubType = null, Count = nvs.Count
                                });
                            else
                                virtualMedia.Add(new MediaItem
                                {
                                    Type = nvs.Type, SubType = null, Count = nvs.Count
                                });
                        }

                    if(realMedia.Count > 0)
                        ViewBag.repRealMedia =
                            realMedia.OrderBy(media => media.Type).ThenBy(media => media.SubType).ToList();

                    if(virtualMedia.Count > 0)
                        ViewBag.repVirtualMedia =
                            virtualMedia.OrderBy(media => media.Type).ThenBy(media => media.SubType).ToList();
                }

                if(ctx.DeviceStats.Any())
                {
                    List<DeviceItem> devices = new List<DeviceItem>();

                    foreach(DeviceStat device in ctx.DeviceStats.ToList())
                    {
                        string xmlFile;

                        if(!string.IsNullOrWhiteSpace(device.Manufacturer) &&
                           !string.IsNullOrWhiteSpace(device.Model)        &&
                           !string.IsNullOrWhiteSpace(device.Revision))
                            xmlFile = device.Manufacturer + "_" + device.Model + "_" + device.Revision + ".xml";
                        else if(!string.IsNullOrWhiteSpace(device.Manufacturer) &&
                                !string.IsNullOrWhiteSpace(device.Model))
                            xmlFile = device.Manufacturer + "_" + device.Model + ".xml";
                        else if(!string.IsNullOrWhiteSpace(device.Model) &&
                                !string.IsNullOrWhiteSpace(device.Revision))
                            xmlFile = device.Model + "_" + device.Revision + ".xml";
                        else
                            xmlFile = device.Model + ".xml";

                        xmlFile = xmlFile.Replace('/', '_').Replace('\\', '_').Replace('?', '_');

                        if(System.IO.File.Exists(Path.Combine(env.ContentRootPath, "Reports", xmlFile)))
                        {
                            var deviceReport = new DeviceReport();

                            var xs = new XmlSerializer(deviceReport.GetType());

                            FileStream fs =
                                WaitForFile(Path.Combine(env.ContentRootPath ?? throw new InvalidOperationException(), "Reports", xmlFile),
                                            FileMode.Open, FileAccess.Read, FileShare.Read);

                            deviceReport = (DeviceReport)xs.Deserialize(fs);
                            fs.Close();

                            var deviceReportV2 = new DeviceReportV2(deviceReport);

                            device.Report = ctx.Devices.Add(new Device(deviceReportV2)).Entity;
                            ctx.SaveChanges();

                            System.IO.File.
                                   Delete(Path.Combine(env.ContentRootPath ?? throw new InvalidOperationException(),
                                                       "Reports", xmlFile));
                        }

                        devices.Add(new DeviceItem
                        {
                            Manufacturer = device.Manufacturer, Model = device.Model, Revision = device.Revision,
                            Bus          = device.Bus,
                            ReportId     = device.Report != null && device.Report.Id != 0 ? device.Report.Id : 0
                        });
                    }

                    ViewBag.repDevices = devices.OrderBy(device => device.Manufacturer).ThenBy(device => device.Model).
                                                 ThenBy(device => device.Revision).ThenBy(device => device.Bus).
                                                 ToList();
                }
            }
            catch(Exception)
            {
            #if DEBUG
                throw;
            #endif
                return Content("Could not read statistics");
            }

            return View();
        }

        static FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for(int numTries = 0; numTries < 100; numTries++)
            {
                FileStream fs = null;

                try
                {
                    fs = new FileStream(fullPath, mode, access, share);

                    return fs;
                }
                catch(IOException)
                {
                    fs?.Dispose();
                    Thread.Sleep(50);
                }
            }

            return null;
        }

        public IActionResult GetOsData()
        {
            var query = ctx.OperatingSystems.GroupBy(x => new
            {
                x.Name
            }, x => x.Count).Select(g => new
            {
                g.Key.Name, Count = g.Sum()
            });

            string[][] result = new string[2][];
            result[0] = query.Select(x => x.Name).ToArray();
            result[1] = query.Select(x => x.Count.ToString()).ToArray();

            for(int i = 0; i < result[0].Length; i++)
                result[0][i] = DetectOS.GetPlatformName((PlatformID)Enum.Parse(typeof(PlatformID), result[0][i]));

            return Json(result);
        }

        public IActionResult GetLinuxData()
        {
            string[][] result =
            {
                ctx.OperatingSystems.Where(o => o.Name == PlatformID.Linux.ToString()).OrderByDescending(o => o.Count).
                    Take(10).
                    Select(x =>
                               $"{DetectOS.GetPlatformName(PlatformID.Linux, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}").
                    ToArray(),
                ctx.OperatingSystems.Where(o => o.Name == PlatformID.Linux.ToString()).OrderByDescending(o => o.Count).
                    Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.OperatingSystems.Where(o => o.Name == PlatformID.Linux.ToString()).Sum(o => o.Count) -
                            result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetMacOsData()
        {
            string[][] result =
            {
                ctx.OperatingSystems.Where(o => o.Name == PlatformID.MacOSX.ToString()).OrderByDescending(o => o.Count).
                    Take(10).
                    Select(x =>
                               $"{DetectOS.GetPlatformName(PlatformID.MacOSX, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}").
                    ToArray(),
                ctx.OperatingSystems.Where(o => o.Name == PlatformID.MacOSX.ToString()).OrderByDescending(o => o.Count).
                    Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.OperatingSystems.Where(o => o.Name == PlatformID.MacOSX.ToString()).Sum(o => o.Count) -
                            result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetWindowsData()
        {
            string[][] result =
            {
                ctx.OperatingSystems.Where(o => o.Name == PlatformID.Win32NT.ToString()).
                    OrderByDescending(o => o.Count).Take(10).
                    Select(x =>
                               $"{DetectOS.GetPlatformName(PlatformID.Win32NT, x.Version)}{(string.IsNullOrEmpty(x.Version) ? "" : " ")}{x.Version}").
                    ToArray(),
                ctx.OperatingSystems.Where(o => o.Name == PlatformID.Win32NT.ToString()).
                    OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.OperatingSystems.Where(o => o.Name == PlatformID.Win32NT.ToString()).Sum(o => o.Count) -
                            result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetVersionsData()
        {
            string[][] result =
            {
                ctx.Versions.OrderByDescending(o => o.Count).Take(10).
                    Select(v => v.Name == "previous" ? "Previous than 3.4.99.0" : v.Name).ToArray(),
                ctx.Versions.OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.Versions.Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetCommandsData()
        {
            string[][] result =
            {
                ctx.Commands.OrderByDescending(o => o.Count).Take(10).Select(v => v.Name).ToArray(),
                ctx.Commands.OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.Commands.Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetFiltersData()
        {
            string[][] result =
            {
                ctx.Filters.OrderByDescending(o => o.Count).Take(10).Select(v => v.Name).ToArray(),
                ctx.Filters.OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.Filters.Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetFormatsData()
        {
            string[][] result =
            {
                ctx.MediaFormats.OrderByDescending(o => o.Count).Take(10).Select(v => v.Name).ToArray(),
                ctx.MediaFormats.OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.MediaFormats.Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetPartitionsData()
        {
            string[][] result =
            {
                ctx.Partitions.OrderByDescending(o => o.Count).Take(10).Select(v => v.Name).ToArray(),
                ctx.Partitions.OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.Partitions.Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetFilesystemsData()
        {
            string[][] result =
            {
                ctx.Filesystems.OrderByDescending(o => o.Count).Take(10).Select(v => v.Name).ToArray(),
                ctx.Filesystems.OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.Filesystems.Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetVirtualMediaData()
        {
            Media[] virtualMedias = ctx.Medias.Where(o => !o.Real).OrderByDescending(o => o.Count).Take(10).ToArray();

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

            string[][] result =
            {
                virtualMedias.Select(v => v.Type).ToArray(), virtualMedias.Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.Medias.Where(o => !o.Real).Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).
                ToString();

            return Json(result);
        }

        public IActionResult GetRealMediaData()
        {
            Media[] realMedias = ctx.Medias.Where(o => o.Real).OrderByDescending(o => o.Count).Take(10).ToArray();

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

            string[][] result =
            {
                realMedias.Select(v => v.Type).ToArray(), realMedias.Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (ctx.Medias.Where(o => o.Real).Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).
                ToString();

            return Json(result);
        }

        public IActionResult GetDevicesByBusData()
        {
            var data = ctx.DeviceStats.Select(d => d.Bus).Distinct().Select(deviceBus => new
            {
                deviceBus, deviceBusCount = ctx.DeviceStats.Count(d => d.Bus == deviceBus)
            }).Select(t => new
            {
                Name = t.deviceBus, Count = t.deviceBusCount
            }).ToList();

            string[][] result =
            {
                data.OrderByDescending(o => o.Count).Take(10).Select(v => v.Name).ToArray(),
                data.OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "Other";

            result[1][9] = (data.Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }

        public IActionResult GetDevicesByManufacturerData()
        {
            List<Device> devices = ctx.Devices.Where(d => d.Manufacturer != null && d.Manufacturer != "").ToList();

            var data = devices.Select(d => d.Manufacturer.ToLowerInvariant()).Distinct().Select(manufacturer => new
            {
                manufacturer, manufacturerCount = devices.Count(d => d.Manufacturer?.ToLowerInvariant() == manufacturer)
            }).Select(t => new
            {
                Name = t.manufacturer, Count = t.manufacturerCount
            }).ToList();

            string[][] result =
            {
                data.OrderByDescending(o => o.Count).Take(10).Select(v => v.Name).ToArray(),
                data.OrderByDescending(o => o.Count).Take(10).Select(x => x.Count.ToString()).ToArray()
            };

            if(result[0].Length < 10)
                return Json(result);

            result[0][9] = "other";

            result[1][9] = (data.Sum(o => o.Count) - result[1].Take(9).Sum(long.Parse)).ToString();

            return Json(result);
        }
    }
}