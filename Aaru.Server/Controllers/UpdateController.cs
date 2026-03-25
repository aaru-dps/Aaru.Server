// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : UpdateController.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
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
// Copyright © 2011-2026 Natalia Portillo
// ****************************************************************************/

using System.Net;
using Aaru.CommonTypes.Metadata;
using Aaru.Dto;
using Aaru.Helpers;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Controllers;

[Controller]
public sealed class UpdateController(DbContext ctx) : ControllerBase
{
    static          List<Device>? _deviceDbCacheSingleton;
    static readonly DateTime      _lastCache = DateTime.MinValue;

    IEnumerable<Device> DeviceDbCache
    {
        get
        {
            if(_deviceDbCacheSingleton is null || DateTime.UtcNow - _lastCache > TimeSpan.FromHours(1))
                _deviceDbCacheSingleton = ctx.Devices.ToList();

            return _deviceDbCacheSingleton;
        }
    }

    /// <summary>Receives a report from Aaru.Core, verifies it's in the correct format and stores it on the server</summary>
    /// <returns>HTTP response</returns>
    [Route("api/update")]
    [HttpGet]
    public ActionResult Update(long timestamp)
    {
        var      sync     = new SyncDto();
        DateTime lastSync = DateHandlers.UnixToDateTime(timestamp);

        sync.UsbVendors = [];

        foreach(UsbVendor vendor in ctx.UsbVendors.Where(v => v.ModifiedWhen > lastSync))
        {
            sync.UsbVendors.Add(new UsbVendorDto
            {
                VendorId = vendor.VendorId,
                Vendor   = vendor.Vendor
            });
        }

        sync.UsbProducts = [];

        foreach(UsbProduct product in ctx.UsbProducts.Where(p => p.ModifiedWhen > lastSync))
        {
            sync.UsbProducts.Add(new UsbProductDto
            {
                Id        = product.Id,
                Product   = product.Product,
                ProductId = product.ProductId,
                VendorId  = product.UsbVendorId
            });
        }

        sync.Offsets = [];

        foreach(CompactDiscOffset offset in ctx.CdOffsets.Where(o => o.ModifiedWhen > lastSync))
            sync.Offsets.Add(new CdOffsetDto(offset, offset.Id));

        sync.Devices = [];

        foreach(Device device in DeviceDbCache.Where(d => d.ModifiedWhen > lastSync).ToList())
        {
            sync.Devices.Add(new
                                 DeviceDto(JsonConvert
                                              .DeserializeObject<DeviceReport>(JsonConvert.SerializeObject(device,
                                                                                   Formatting.None,
                                                                                   new JsonSerializerSettings
                                                                                   {
                                                                                       ReferenceLoopHandling =
                                                                                           ReferenceLoopHandling
                                                                                              .Ignore
                                                                                   })),
                                           device.Id,
                                           device.OptimalMultipleSectorsRead,
                                           device.CanReadGdRomUsingSwapDisc));
        }

        sync.NesHeaders = [];

        foreach(NesHeaderInfo header in ctx.NesHeaders.Where(v => v.ModifiedWhen > lastSync))
        {
            sync.NesHeaders.Add(new NesHeaderDto
            {
                Id                     = header.Id,
                BatteryPresent         = header.BatteryPresent,
                ConsoleType            = header.ConsoleType,
                DefaultExpansionDevice = header.DefaultExpansionDevice,
                ExtendedConsoleType    = header.ExtendedConsoleType,
                FourScreenMode         = header.FourScreenMode,
                Mapper                 = header.Mapper,
                NametableMirroring     = header.NametableMirroring,
                Sha256                 = header.Sha256,
                Submapper              = header.Submapper,
                TimingMode             = header.TimingMode,
                VsHardwareType         = header.VsHardwareType,
                VsPpuType              = header.VsPpuType
            });
        }

        var js = JsonSerializer.Create();
        var sw = new StringWriter();
        js.Serialize(sw, sync);

        return new ContentResult
        {
            StatusCode  = (int)HttpStatusCode.OK,
            Content     = sw.ToString(),
            ContentType = "application/json"
        };
    }
}