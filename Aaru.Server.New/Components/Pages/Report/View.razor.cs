using Aaru.CommonTypes.Metadata;
using Aaru.Decoders.PCMCIA;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Tuple = Aaru.Decoders.PCMCIA.Tuple;

namespace Aaru.Server.New.Components.Pages.Report;

public partial class View
{
    bool   _initialized;
    bool   _notFound;
    bool   accordionItem1Visible;
    string _pageTitle { get; set; } = "Aaru Device Report";
    [CascadingParameter]
    HttpContext HttpContext { get; set; } = default!;
    [Parameter]
    public int Id { get;                                                 set; }
    public Item?                       UsbItem                    { get; set; }
    public Item?                       FireWireItem               { get; set; }
    public Dictionary<string, string>? PcmciaTuples               { get; set; }
    public PcmciaItem?                 PcmciaItem                 { get; set; }
    public string?                     lblDeviceType              { get; set; }
    public string?                     AtaItem                    { get; set; }
    public string?                     MaximumAtaRevision         { get; set; }
    public List<string>?               SupportedAtaVersions       { get; set; }
    public Dictionary<string, string>? DeviceIdentification       { get; set; }
    public string?                     AtaTransport               { get; set; }
    public List<string>?               AtaTransportVersions       { get; set; }
    public List<string>?               GeneralConfiguration       { get; set; }
    public List<string>?               SpecificConfiguration      { get; set; }
    public List<string>?               DeviceCapabilities         { get; set; }
    public List<string>?               CommandSetAndFeatures      { get; set; }
    public List<string>?               PioTransferModes           { get; set; }
    public List<string>?               DmaTransferModes           { get; set; }
    public List<string>?               MDmaTransferModes          { get; set; }
    public List<string>?               UltraDmaTransferModes      { get; set; }
    public List<string>?               NvCache                    { get; set; }
    public List<string>?               SmartCommandTransport      { get; set; }
    public List<string>?               Streaming                  { get; set; }
    public List<string>?               Security                   { get; set; }
    public Dictionary<string, string>? ReadCapabilitiesDictionary { get; set; }
    public List<string>?               ReadCapabilitiesList       { get; set; }
    public string?                     Ocr                        { get; set; }
    public string?                     ExtendedCsd                { get; set; }
    public string?                     Csd                        { get; set; }
    public string?                     Cid                        { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if(Id <= 0)
        {
            _notFound                       = true;
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            return;
        }

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        Device? report = await ctx.Devices.Include(static deviceReportV2 => deviceReportV2.USB)
                                  .Include(static deviceReportV2 => deviceReportV2.FireWire)
                                  .Include(static deviceReportV2 => deviceReportV2.PCMCIA)
                                  .Include(static deviceReportV2 => deviceReportV2.ATAPI)
                                  .Include(static deviceReportV2 => deviceReportV2.ATA)
                                  .Include(static deviceReportV2 => deviceReportV2.MultiMediaCard)
                                  .FirstOrDefaultAsync(d => d.Id == Id);

        if(report is null)
        {
            _notFound                       = true;
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            return;
        }

        _pageTitle = $"Aaru Device Report for {report.Manufacturer} {report.Model} {report.Revision}";

        if(report.USB != null)
        {
            string? usbVendorDescription  = null;
            string? usbProductDescription = null;

            UsbProduct? dbProduct = await ctx.UsbProducts.Include(static usbProduct => usbProduct.Vendor)
                                             .FirstOrDefaultAsync(p => p.ProductId       == report.USB.ProductID &&
                                                                       p.Vendor.VendorId == report.USB.VendorID);

            if(dbProduct is null)
            {
                UsbVendor? dbVendor = await ctx.UsbVendors.FirstOrDefaultAsync(v => v.VendorId == report.USB.VendorID);

                if(dbVendor is not null) usbVendorDescription = dbVendor.Vendor;
            }
            else
            {
                usbProductDescription = dbProduct.Product;
                usbVendorDescription  = dbProduct.Vendor.Vendor;
            }

            UsbItem = new Item
            {
                Manufacturer = report.USB.Manufacturer,
                Product      = report.USB.Product,
                VendorDescription =
                    usbVendorDescription != null
                        ? $"0x{report.USB.VendorID:x4} ({usbVendorDescription})"
                        : $"0x{report.USB.VendorID:x4}",
                ProductDescription = usbProductDescription != null
                                         ? $"0x{report.USB.ProductID:x4} ({usbProductDescription})"
                                         : $"0x{report.USB.ProductID:x4}"
            };
        }

        if(report.FireWire != null)
        {
            FireWireItem = new Item
            {
                Manufacturer       = report.FireWire.Manufacturer,
                Product            = report.FireWire.Product,
                VendorDescription  = $"0x{report.FireWire.VendorID:x8}",
                ProductDescription = $"0x{report.FireWire.ProductID:x8}"
            };
        }

        if(report.PCMCIA != null)
        {
            PcmciaItem = new PcmciaItem
            {
                Manufacturer       = report.PCMCIA.Manufacturer,
                Product            = report.PCMCIA.ProductName,
                VendorDescription  = $"0x{report.PCMCIA.ManufacturerCode:x4}",
                ProductDescription = $"0x{report.PCMCIA.CardCode:x4}",
                Compliance         = report.PCMCIA.Compliance
            };

            Tuple[]? tuples = CIS.GetTuples(report.PCMCIA.CIS);

            if(tuples != null)
            {
                Dictionary<string, string> decodedTuples = new();

                foreach(Tuple tuple in tuples)
                {
                    switch(tuple.Code)
                    {
                        case TupleCodes.CISTPL_NULL:
                        case TupleCodes.CISTPL_END:
                        case TupleCodes.CISTPL_MANFID:
                        case TupleCodes.CISTPL_VERS_1:
                            break;
                        case TupleCodes.CISTPL_DEVICEGEO:
                        case TupleCodes.CISTPL_DEVICEGEO_A:
                            DeviceGeometryTuple geom = CIS.DecodeDeviceGeometryTuple(tuple.Data);

                            if(geom?.Geometries != null)
                            {
                                foreach(DeviceGeometry geometry in geom.Geometries)
                                {
                                    decodedTuples.Add("Device width", $"{(1 << geometry.CardInterface - 1) * 8} bits");

                                    decodedTuples.Add("Erase block",
                                                      $"{(1 << geometry.EraseBlockSize - 1) * (1 << geometry.Interleaving - 1)} bytes");

                                    decodedTuples.Add("Read block",
                                                      $"{(1 << geometry.ReadBlockSize - 1) * (1 << geometry.Interleaving - 1)} bytes");

                                    decodedTuples.Add("Write block",
                                                      $"{(1 << geometry.WriteBlockSize - 1) * (1 << geometry.Interleaving - 1)} bytes");

                                    decodedTuples.Add("Partition alignment",
                                                      $"{(1 << geometry.EraseBlockSize - 1) * (1 << geometry.Interleaving - 1) * (1 << geometry.Partitions - 1)} bytes");
                                }
                            }

                            break;
                        case TupleCodes.CISTPL_ALTSTR:
                        case TupleCodes.CISTPL_BAR:
                        case TupleCodes.CISTPL_BATTERY:
                        case TupleCodes.CISTPL_BYTEORDER:
                        case TupleCodes.CISTPL_CFTABLE_ENTRY:
                        case TupleCodes.CISTPL_CFTABLE_ENTRY_CB:
                        case TupleCodes.CISTPL_CHECKSUM:
                        case TupleCodes.CISTPL_CONFIG:
                        case TupleCodes.CISTPL_CONFIG_CB:
                        case TupleCodes.CISTPL_DATE:
                        case TupleCodes.CISTPL_DEVICE:
                        case TupleCodes.CISTPL_DEVICE_A:
                        case TupleCodes.CISTPL_DEVICE_OA:
                        case TupleCodes.CISTPL_DEVICE_OC:
                        case TupleCodes.CISTPL_EXTDEVIC:
                        case TupleCodes.CISTPL_FORMAT:
                        case TupleCodes.CISTPL_FORMAT_A:
                        case TupleCodes.CISTPL_FUNCE:
                        case TupleCodes.CISTPL_FUNCID:
                        case TupleCodes.CISTPL_GEOMETRY:
                        case TupleCodes.CISTPL_INDIRECT:
                        case TupleCodes.CISTPL_JEDEC_A:
                        case TupleCodes.CISTPL_JEDEC_C:
                        case TupleCodes.CISTPL_LINKTARGET:
                        case TupleCodes.CISTPL_LONGLINK_A:
                        case TupleCodes.CISTPL_LONGLINK_C:
                        case TupleCodes.CISTPL_LONGLINK_CB:
                        case TupleCodes.CISTPL_LONGLINK_MFC:
                        case TupleCodes.CISTPL_NO_LINK:
                        case TupleCodes.CISTPL_ORG:
                        case TupleCodes.CISTPL_PWR_MGMNT:
                        case TupleCodes.CISTPL_SPCL:
                        case TupleCodes.CISTPL_SWIL:
                        case TupleCodes.CISTPL_VERS_2:
                            decodedTuples.Add("Undecoded tuple ID", tuple.Code.ToString());

                            break;
                        default:
                            decodedTuples.Add("Unknown tuple ID", $"0x{(byte)tuple.Code:X2}");

                            break;
                    }
                }

                if(decodedTuples.Count > 0) PcmciaTuples = decodedTuples;
            }
        }

        var                removable = true;
        List<TestedMedia>? testedMedia;
        var                atapi = false;

        if(report.ATA != null || report.ATAPI != null)
        {
            Ata? ataReport;

            if(report.ATAPI != null)
            {
                AtaItem   = "ATAPI";
                ataReport = report.ATAPI;
                atapi     = true;
            }
            else
            {
                AtaItem   = "ATA";
                ataReport = report.ATA;
            }

            bool cfa = report.CompactFlash;

            lblDeviceType = atapi switch
                            {
                                true when !cfa => "ATAPI device",
                                false when cfa => "CompactFlash device",
                                _              => "ATA device"
                            };

            Core.Ata.Report(ataReport!,
                            cfa,
                            atapi,
                            ref removable,
                            out testedMedia,
                            out Dictionary<string, string>? ataDeviceIdentification,
                            out List<string> supportedAtaVersions,
                            out string? maximumAtaRevision,
                            out string? transport,
                            out List<string>? transportVersions,
                            out List<string>? generalConfiguration,
                            out List<string>? specificConfiguration,
                            out List<string> deviceCapabilities,
                            out List<string> pioTransferModes,
                            out List<string> dmaTransferModes,
                            out List<string> mdmaTransferModes,
                            out List<string> ultraDmaTransferModes,
                            out List<string> commandSetAndFeatures,
                            out List<string>? security,
                            out List<string>? streaming,
                            out List<string>? smartCommandTransport,
                            out List<string>? nvCache,
                            out Dictionary<string, string> readCapabilitiesDictionary,
                            out List<string> readCapabilitiesList);

            DeviceIdentification  = ataDeviceIdentification;
            SupportedAtaVersions  = supportedAtaVersions;
            MaximumAtaRevision    = maximumAtaRevision;
            AtaTransport          = transport;
            AtaTransportVersions  = transportVersions;
            GeneralConfiguration  = generalConfiguration;
            SpecificConfiguration = specificConfiguration;
            if(deviceCapabilities.Count    > 0) DeviceCapabilities    = deviceCapabilities;
            if(pioTransferModes.Count      > 0) PioTransferModes      = pioTransferModes;
            if(dmaTransferModes.Count      > 0) DmaTransferModes      = dmaTransferModes;
            if(mdmaTransferModes.Count     > 0) MDmaTransferModes     = mdmaTransferModes;
            if(ultraDmaTransferModes.Count > 0) UltraDmaTransferModes = ultraDmaTransferModes;
            if(commandSetAndFeatures.Count > 0) CommandSetAndFeatures = commandSetAndFeatures;
            Security              = security;
            Streaming             = streaming;
            SmartCommandTransport = smartCommandTransport;
            NvCache               = nvCache;
            if(readCapabilitiesDictionary.Count > 0) ReadCapabilitiesDictionary = readCapabilitiesDictionary;
            if(readCapabilitiesList.Count       > 0) ReadCapabilitiesList       = readCapabilitiesList;
        }

        if(report.MultiMediaCard != null)
        {
            lblDeviceType = "MultiMediaCard";

            if(report.MultiMediaCard.CID != null)
                Cid = Decoders.MMC.Decoders.PrettifyCID(report.MultiMediaCard.CID).Replace("\n", "<br/>");

            if(report.MultiMediaCard.CSD != null)
                Csd = Decoders.MMC.Decoders.PrettifyCSD(report.MultiMediaCard.CSD).Replace("\n", "<br/>");

            if(report.MultiMediaCard.ExtendedCSD != null)
            {
                ExtendedCsd = Decoders.MMC.Decoders.PrettifyExtendedCSD(report.MultiMediaCard.ExtendedCSD)
                                      .Replace("\n", "<br/>");
            }

            if(report.MultiMediaCard.OCR != null)
                Ocr = Decoders.MMC.Decoders.PrettifyCSD(report.MultiMediaCard.OCR).Replace("\n", "<br/>");
        }

        _initialized = true;

        StateHasChanged();
    }
}

public class Item
{
    public string? Manufacturer;
    public string? Product;
    public string? ProductDescription;
    public string? VendorDescription;
}

public sealed class PcmciaItem : Item
{
    public string? Compliance;
}