using Aaru.CommonTypes.Metadata;
using Aaru.CommonTypes.Structs.Devices.SCSI;
using Aaru.Decoders.PCMCIA;
using Aaru.Decoders.SCSI;
using Aaru.Helpers;
using Aaru.Server.Database.Models;
using Aaru.Server.New.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Ata = Aaru.CommonTypes.Metadata.Ata;
using DbContext = Aaru.Server.Database.DbContext;
using Inquiry = Aaru.CommonTypes.Structs.Devices.SCSI.Inquiry;
using TestedMedia = Aaru.CommonTypes.Metadata.TestedMedia;
using Tuple = Aaru.Decoders.PCMCIA.Tuple;

namespace Aaru.Server.New.Components.Pages.Report;

public partial class View
{
    bool   _initialized;
    bool   _notFound;
    string _pageTitle { get; set; } = "Aaru Device Report";
    [CascadingParameter]
    HttpContext HttpContext { get; set; } = default!;
    [Parameter]
    public int Id { get;                                                       set; }
    public Item?                             UsbItem                    { get; set; }
    public Item?                             FireWireItem               { get; set; }
    public Dictionary<string, string>?       PcmciaTuples               { get; set; }
    public PcmciaItem?                       PcmciaItem                 { get; set; }
    public string?                           DeviceType                 { get; set; }
    public string?                           AtaItem                    { get; set; }
    public string?                           MaximumAtaRevision         { get; set; }
    public List<string>?                     SupportedAtaVersions       { get; set; }
    public Dictionary<string, string>?       DeviceIdentification       { get; set; }
    public Dictionary<string, string>?       DeviceInquiry              { get; set; }
    public string?                           AtaTransport               { get; set; }
    public List<string>?                     AtaTransportVersions       { get; set; }
    public List<string>?                     GeneralConfiguration       { get; set; }
    public List<string>?                     SpecificConfiguration      { get; set; }
    public List<string>?                     DeviceCapabilities         { get; set; }
    public List<string>?                     CommandSetAndFeatures      { get; set; }
    public List<string>?                     PioTransferModes           { get; set; }
    public List<string>?                     DmaTransferModes           { get; set; }
    public List<string>?                     MDmaTransferModes          { get; set; }
    public List<string>?                     UltraDmaTransferModes      { get; set; }
    public List<string>?                     NvCache                    { get; set; }
    public List<string>?                     SmartCommandTransport      { get; set; }
    public List<string>?                     Streaming                  { get; set; }
    public List<string>?                     Security                   { get; set; }
    public Dictionary<string, string>?       ReadCapabilitiesDictionary { get; set; }
    public List<string>?                     ReadCapabilitiesList       { get; set; }
    public string?                           Ocr                        { get; set; }
    public string?                           ExtendedCsd                { get; set; }
    public string?                           Csd                        { get; set; }
    public string?                           Cid                        { get; set; }
    public string?                           Scr                        { get; set; }
    public List<string>?                     InquiryCapabilities        { get; set; }
    public List<string>?                     ModeSenseCapabilities      { get; set; }
    public Dictionary<string, List<string>>? ModeSensePages             { get; set; }
    public List<string>?                     BlockDescriptors           { get; set; }
    public List<SscSupportedMedia>?          ScsiSscMedias              { get; set; }
    public List<SupportedDensity>?           ScsiSscDensities           { get; set; }
    public string?                           ScsiSscMinBlock            { get; set; }
    public string?                           ScsiSscMaxBlock            { get; set; }
    public string?                           ScsiSscGranularity         { get; set; }
    public bool                              ScsiSscVisible             { get; set; }
    public List<string>?                     MmcFeaturesList            { get; set; }
    public List<string>?                     MmcModeList                { get; set; }
    public Dictionary<string, List<string>>? EvpdPages                  { get; set; }

    public Dictionary<string, (Dictionary<string, string> Table, List<string> List)>? MediaInformation { get; set; }

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
                                  .Include(static deviceReportV2 => deviceReportV2.SecureDigital)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.ModeSense)
                                  .ThenInclude(static scsiMode => scsiMode.BlockDescriptors)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.ModeSense)
                                  .ThenInclude(static scsiMode => scsiMode.ModePages)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.EVPDPages)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.MultiMediaDevice)
                                  .ThenInclude(static mmc => mmc.Features)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.SequentialDevice)
                                  .ThenInclude(static ssc => ssc.SupportedDensities)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.SequentialDevice)
                                  .ThenInclude(static ssc => ssc.SupportedMediaTypes)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.ReadCapabilities)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.SequentialDevice)
                                  .ThenInclude(static ssc => ssc.TestedMedia)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.RemovableMedias)
                                  .Include(static deviceReportV2 => deviceReportV2.SCSI)
                                  .ThenInclude(static scsi => scsi.MultiMediaDevice)
                                  .ThenInclude(static mmc => mmc.TestedMedia)
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

        var                removable   = true;
        List<TestedMedia>? testedMedia = null;
        var                atapi       = false;
        var                sscMedia    = false;

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

            DeviceType = atapi switch
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
            DeviceType = "MultiMediaCard";

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

        if(report.SecureDigital != null)
        {
            DeviceType = "SecureDigital";

            if(report.SecureDigital.CID != null)
                Cid = Decoders.SecureDigital.Decoders.PrettifyCID(report.SecureDigital.CID).Replace("\n", "<br/>");

            if(report.SecureDigital.CSD != null)
                Csd = Decoders.SecureDigital.Decoders.PrettifyCSD(report.SecureDigital.CSD).Replace("\n", "<br/>");

            if(report.SecureDigital.SCR != null)
                Scr = Decoders.SecureDigital.Decoders.PrettifySCR(report.SecureDigital.SCR).Replace("\n", "<br/>");

            if(report.SecureDigital.OCR != null)
                Ocr = Decoders.SecureDigital.Decoders.PrettifyCSD(report.SecureDigital.OCR).Replace("\n", "<br/>");
        }


        if(report.SCSI != null)
        {
            var vendorId = "";

            if(report.SCSI.Inquiry != null)
            {
                Inquiry inq = report.SCSI.Inquiry.Value;
                vendorId = StringHandlers.CToString(report.SCSI.Inquiry?.VendorIdentification);

                DeviceInquiry = new Dictionary<string, string>
                {
                    {
                        "Vendor:",
                        VendorString.Prettify(vendorId) != vendorId
                            ? $"{vendorId} ({VendorString.Prettify(vendorId)})"
                            : vendorId
                    },
                    {
                        "Product:", StringHandlers.CToString(inq.ProductIdentification)
                    },
                    {
                        "Revision:", StringHandlers.CToString(inq.ProductRevisionLevel)
                    }
                };
            }

            List<string> inquiryCapabilities                      = ScsiInquiry.Report(report.SCSI.Inquiry);
            if(inquiryCapabilities.Count > 0) InquiryCapabilities = inquiryCapabilities;

            if(report.SCSI.SupportsModeSense6)
            {
                ModeSenseCapabilities ??= [];
                ModeSenseCapabilities.Add("Device supports MODE SENSE (6)");
            }

            if(report.SCSI.SupportsModeSense10)
            {
                ModeSenseCapabilities ??= [];
                ModeSenseCapabilities.Add("Device supports MODE SENSE (10)");
            }

            if(report.SCSI.SupportsModeSubpages)
            {
                ModeSenseCapabilities ??= [];
                ModeSenseCapabilities.Add("Device supports MODE SENSE subpages");
            }

            if(report.SCSI.ModeSense != null)
            {
                PeripheralDeviceTypes devType = PeripheralDeviceTypes.DirectAccess;

                if(report.SCSI.Inquiry != null)
                    devType = (PeripheralDeviceTypes)report.SCSI.Inquiry.Value.PeripheralDeviceType;

                ScsiModeSense.Report(report.SCSI.ModeSense,
                                     vendorId,
                                     devType,
                                     out List<string>? modeSenseCapabilities,
                                     out List<string>? blockDescriptors,
                                     out Dictionary<string, List<string>> modePages);

                if(modePages.Count > 0) ModeSensePages = modePages;

                if(modeSenseCapabilities is not null)
                {
                    ModeSenseCapabilities ??= [];
                    ModeSenseCapabilities.AddRange(modeSenseCapabilities);
                }

                if(blockDescriptors is not null) BlockDescriptors = blockDescriptors;
            }

            if(report.SCSI.EVPDPages != null)
            {
                ScsiEvpd.Report(report.SCSI.EVPDPages, vendorId, out Dictionary<string, List<string>> evpdPages);

                if(evpdPages.Count > 0) EvpdPages = evpdPages;
            }

            if(report.SCSI.MultiMediaDevice is not null)
            {
                testedMedia = report.SCSI.MultiMediaDevice.TestedMedia;

                if(report.SCSI.MultiMediaDevice.ModeSense2A != null)
                {
                    ScsiMmcMode.Report(report.SCSI.MultiMediaDevice.ModeSense2A, out List<string> mmcModeList);

                    if(mmcModeList.Count > 0) MmcModeList = mmcModeList;
                }

                if(report.SCSI.MultiMediaDevice.Features != null)
                {
                    ScsiMmcFeatures.Report(report.SCSI.MultiMediaDevice.Features, out List<string> mmcFeaturesList);

                    if(mmcFeaturesList.Count > 0) MmcFeaturesList = mmcFeaturesList;
                }
            }
            else if(report.SCSI.SequentialDevice != null)
            {
                ScsiSscVisible = true;

                ScsiSscGranularity = report.SCSI.SequentialDevice.BlockSizeGranularity?.ToString() ?? "Unspecified";

                ScsiSscMaxBlock = report.SCSI.SequentialDevice.MaxBlockLength?.ToString() ?? "Unspecified";

                ScsiSscMinBlock = report.SCSI.SequentialDevice.MinBlockLength?.ToString() ?? "Unspecified";

                if(report.SCSI.SequentialDevice.SupportedDensities != null)
                    ScsiSscDensities = report.SCSI.SequentialDevice.SupportedDensities;

                if(report.SCSI.SequentialDevice.SupportedMediaTypes != null)
                    ScsiSscMedias = report.SCSI.SequentialDevice.SupportedMediaTypes;

                if(report.SCSI.SequentialDevice.TestedMedia != null)
                {
                    sscMedia = true;

                    SscTestedMedia.Report(report.SCSI.SequentialDevice.TestedMedia,
                                          out Dictionary<string, (Dictionary<string, string> Table, List<string> List)>
                                                  mediaInformation);

                    if(mediaInformation.Count > 0) MediaInformation = mediaInformation;
                }
            }
            else if(report.SCSI.ReadCapabilities != null)
            {
                removable = false;
                List<string> readCapabilitiesList       = [];
                var          readCapabilitiesDictionary = new Dictionary<string, string>();

                if(report.SCSI.ReadCapabilities.Blocks.HasValue && report.SCSI.ReadCapabilities.BlockSize.HasValue)
                {
                    readCapabilitiesDictionary.Add("Device has",
                                                   $"{report.SCSI.ReadCapabilities.Blocks} blocks of {report.SCSI.ReadCapabilities.BlockSize} bytes each");

                    switch(report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize / 1024 / 1024)
                    {
                        case > 1000000:
                            readCapabilitiesDictionary.Add("Device size",
                                                           $"{report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize} bytes, {report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize / 1000 / 1000 / 1000 / 1000} Tb, {(double)(report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize) / 1024 / 1024 / 1024 / 1024:F2} TiB");

                            break;
                        case > 1000:
                            readCapabilitiesDictionary.Add("Device size",
                                                           $"{report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize} bytes, {report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize / 1000 / 1000 / 1000} Gb, {(double)(report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize) / 1024 / 1024 / 1024:F2} GiB");

                            break;
                        default:
                            readCapabilitiesDictionary.Add("Device size",
                                                           $"{report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize} bytes, {report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize / 1000 / 1000} Mb, {(double)(report.SCSI.ReadCapabilities.Blocks * report.SCSI.ReadCapabilities.BlockSize) / 1024 / 1024:F2} MiB");

                            break;
                    }
                }

                if(report.SCSI.ReadCapabilities.MediumType.HasValue)
                {
                    readCapabilitiesDictionary.Add("Medium type code",
                                                   $"{report.SCSI.ReadCapabilities.MediumType:X2}h");
                }

                if(report.SCSI.ReadCapabilities.Density.HasValue)
                    readCapabilitiesDictionary.Add("Density code", $"{report.SCSI.ReadCapabilities.Density:X2}h");

                if((report.SCSI.ReadCapabilities.SupportsReadLong   == true ||
                    report.SCSI.ReadCapabilities.SupportsReadLong16 == true) &&
                   report.SCSI.ReadCapabilities.LongBlockSize.HasValue)
                {
                    readCapabilitiesDictionary.Add("Long block size",
                                                   $"{report.SCSI.ReadCapabilities.LongBlockSize} bytes");
                }

                if(report.SCSI.ReadCapabilities.SupportsReadCapacity == true)
                    readCapabilitiesList.Add("Device supports READ CAPACITY (10) command.");

                if(report.SCSI.ReadCapabilities.SupportsReadCapacity16 == true)
                    readCapabilitiesList.Add("Device supports READ CAPACITY (16) command.");

                if(report.SCSI.ReadCapabilities.SupportsRead6 == true)
                    readCapabilitiesList.Add("Device supports READ (6) command.");

                if(report.SCSI.ReadCapabilities.SupportsRead10 == true)
                    readCapabilitiesList.Add("Device supports READ (10) command.");

                if(report.SCSI.ReadCapabilities.SupportsRead12 == true)
                    readCapabilitiesList.Add("Device supports READ (12) command.");

                if(report.SCSI.ReadCapabilities.SupportsRead16 == true)
                    readCapabilitiesList.Add("Device supports READ (16) command.");

                if(report.SCSI.ReadCapabilities.SupportsReadLong == true)
                    readCapabilitiesList.Add("Device supports READ LONG (10) command.");

                if(report.SCSI.ReadCapabilities.SupportsReadLong16 == true)
                    readCapabilitiesList.Add("Device supports READ LONG (16) command.");

                if(report.SCSI.ReadCapabilities.SupportsHLDTSTReadRawDVD == true)
                    readCapabilitiesList.Add("Device supports reading RAW DVD data using HL-DT-ST vendor command");

                if(report.SCSI.ReadCapabilities.SupportsLiteOnReadRawDVD == true)
                    readCapabilitiesList.Add("Device supports reading RAW DVD data using Lite-On READ BUFFER command");

                if(readCapabilitiesList.Count       > 0) ReadCapabilitiesList       = readCapabilitiesList;
                if(readCapabilitiesDictionary.Count > 0) ReadCapabilitiesDictionary = readCapabilitiesDictionary;
            }
            else
                testedMedia = report.SCSI.RemovableMedias;
        }

        if(removable && !sscMedia && testedMedia != null)
        {
            Core.TestedMedia.Report(testedMedia,
                                    out Dictionary<string, (Dictionary<string, string> Table, List<string> List)>
                                            mediaInformation);

            if(mediaInformation.Count > 0) MediaInformation = mediaInformation;
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