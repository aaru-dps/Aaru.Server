using System.Net;
using Aaru.CommonTypes.Metadata;
using Aaru.Dto;
using Aaru.Helpers;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.New.Controllers;

[Controller]
public sealed class UpdateController(DbContext ctx) : ControllerBase
{
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

        foreach(UsbProduct product in ctx.UsbProducts.Include(static p => p.Vendor)
                                         .Where(p => p.ModifiedWhen > lastSync))
        {
            sync.UsbProducts.Add(new UsbProductDto
            {
                Id        = product.Id,
                Product   = product.Product,
                ProductId = product.ProductId,
                VendorId  = product.Vendor.VendorId
            });
        }

        sync.Offsets = [];

        foreach(CompactDiscOffset offset in ctx.CdOffsets.Where(o => o.ModifiedWhen > lastSync))
            sync.Offsets.Add(new CdOffsetDto(offset, offset.Id));

        sync.Devices = [];

        foreach(Device device in ctx.Devices.Where(d => d.ModifiedWhen > lastSync).ToList())
        {
            sync.Devices.Add(new
                                 DeviceDto(JsonConvert
                                              .DeserializeObject<DeviceReportV2>(JsonConvert.SerializeObject(device,
                                                   Formatting.None,
                                                   new JsonSerializerSettings
                                                   {
                                                       ReferenceLoopHandling =
                                                           ReferenceLoopHandling.Ignore
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