// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Ata.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Decodes ATA information from reports.
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

using Aaru.CommonTypes.Structs.Devices.ATA;
using Aaru.CommonTypes.Structs.Devices.SCSI;

namespace Aaru.Server.Core;

// TODO: Use Humanizer
// TODO: Better use HTML and CSS
public static class Ata
{
    /// <summary>
    ///     Takes the ATA part of a device report and prepares it to be rendered
    /// </summary>
    /// <param name="ataReport">ATA part of a device report</param>
    /// <param name="cfa"><c>true</c> if compact flash device</param>
    /// <param name="atapi"><c>true</c> if atapi device</param>
    /// <param name="removable"><c>true</c> if removable device</param>
    /// <param name="testedMedia">List of tested media</param>
    /// <param name="deviceIdentification">List of device identification strings</param>
    /// <param name="supportedAtaVersions">Supported ATA versions</param>
    /// <param name="maximumAtaRevision">Maximum supported ATA revision</param>
    /// <param name="transport">ATA transport layer</param>
    /// <param name="transportVersions">Supported transport versions</param>
    /// <param name="generalConfiguration">Device general configuration</param>
    /// <param name="specificConfiguration">Device specific configuration</param>
    /// <param name="deviceCapabilities">Device capabilities</param>
    /// <param name="pioTransferModes">PIO transfer modes</param>
    /// <param name="dmaTransferModes">Single-word DMA transfer modes</param>
    /// <param name="mdmaTransferModes">Multi-word DMA transfer modes</param>
    /// <param name="ultraDmaTransferModes">Ultra DMA transfer modes</param>
    /// <param name="commandSetAndFeatures">Device command set and features</param>
    /// <param name="security">Security parameters</param>
    /// <param name="streaming">Streaming parameters</param>
    /// <param name="smartCommandTransport">S.M.A.R.T. command transport</param>
    /// <param name="nvCache">Non-volatile cache</param>
    /// <param name="readCapabilitiesDictionary">Dictionary of read capabilities for non-removable media</param>
    /// <param name="readCapabilitiesList">List of read capabilities for non-removable media</param>
    public static void Report(CommonTypes.Metadata.Ata ataReport, bool cfa, bool atapi, ref bool removable,
                              out List<CommonTypes.Metadata.TestedMedia>? testedMedia,
                              out Dictionary<string, string>? deviceIdentification,
                              out List<string> supportedAtaVersions, out string? maximumAtaRevision,
                              out string? transport, out List<string>? transportVersions,
                              out List<string> generalConfiguration, out List<string>? specificConfiguration,
                              out List<string> deviceCapabilities, out List<string> pioTransferModes,
                              out List<string> dmaTransferModes, out List<string> mdmaTransferModes,
                              out List<string> ultraDmaTransferModes, out List<string> commandSetAndFeatures,
                              out List<string>? security, out List<string>? streaming,
                              out List<string>? smartCommandTransport, out List<string>? nvCache,
                              out Dictionary<string, string> readCapabilitiesDictionary,
                              out List<string> readCapabilitiesList)
    {
        uint logicalSectorSize = 0;
        testedMedia                = null;
        deviceIdentification       = null;
        supportedAtaVersions       = [];
        maximumAtaRevision         = null;
        transport                  = null;
        transportVersions          = null;
        generalConfiguration       = [];
        specificConfiguration      = null;
        deviceCapabilities         = [];
        pioTransferModes           = [];
        dmaTransferModes           = [];
        mdmaTransferModes          = [];
        ultraDmaTransferModes      = [];
        commandSetAndFeatures      = [];
        security                   = null;
        streaming                  = null;
        smartCommandTransport      = null;
        nvCache                    = null;
        readCapabilitiesDictionary = new Dictionary<string, string>();
        readCapabilitiesList       = [];

        Identify.IdentifyDevice? ataIdentifyNullable = Identify.Decode(ataReport.Identify);

        if(!ataIdentifyNullable.HasValue) return;

        Identify.IdentifyDevice ataIdentify = ataIdentifyNullable.Value;

        if(!string.IsNullOrEmpty(ataIdentify.Model))
        {
            deviceIdentification          ??= new Dictionary<string, string>();
            deviceIdentification["Model"] =   ataIdentify.Model;
        }

        if(!string.IsNullOrEmpty(ataIdentify.FirmwareRevision))
        {
            deviceIdentification                      ??= new Dictionary<string, string>();
            deviceIdentification["Firmware revision"] =   ataIdentify.FirmwareRevision;
        }

        if(!string.IsNullOrEmpty(ataIdentify.AdditionalPID))
        {
            deviceIdentification                          ??= new Dictionary<string, string>();
            deviceIdentification["Additional product ID"] =   ataIdentify.AdditionalPID;
        }

        if(!atapi)
        {
            logicalSectorSize = ataIdentify.LogicalSectorWords * 2;

            if(logicalSectorSize is 0) logicalSectorSize = 512;

            // Multiple logical sectors per physical sector
            if((ataIdentify.PhysLogSectorSize & 0xD000) == 0x5000)
                logicalSectorSize *= (uint)(1 << (ataIdentify.PhysLogSectorSize & 0x0F));
        }

        bool ata1 = false,
             ata2 = false,
             ata3 = false,
             ata4 = false,
             ata5 = false,
             ata6 = false,
             ata7 = false,
             acs  = false,
             acs2 = false,
             acs3 = false,
             acs4 = false;

        if((ushort)ataIdentify.MajorVersion == 0x0000 || (ushort)ataIdentify.MajorVersion == 0xFFFF)
        {
            // Obsolete in ATA-2, if present, device supports ATA-1
            ata1 |= ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.FastIDE) ||
                    ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.SlowIDE) ||
                    ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.UltraFastIDE);

            ata2 |= ataIdentify.ExtendedIdentify.HasFlag(Identify.ExtendedIdentifyBit.Words54to58Valid) ||
                    ataIdentify.ExtendedIdentify.HasFlag(Identify.ExtendedIdentifyBit.Words64to70Valid) ||
                    ataIdentify.ExtendedIdentify.HasFlag(Identify.ExtendedIdentifyBit.Word88Valid);

            if(!ata1 && !ata2 && !atapi && !cfa) ata2 = true;

            ata4 |= atapi;
            ata3 |= cfa;

            if(cfa && ata1) ata1 = false;

            if(cfa && ata2) ata2 = false;
        }
        else
        {
            ata1 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.Ata1);
            ata2 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.Ata2);
            ata3 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.Ata3);
            ata4 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.AtaAtapi4);
            ata5 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.AtaAtapi5);
            ata6 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.AtaAtapi6);
            ata7 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.AtaAtapi7);
            acs  |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.Ata8ACS);
            acs2 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.ACS2);
            acs3 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.ACS3);
            acs4 |= ataIdentify.MajorVersion.HasFlag(Identify.MajorVersionBit.ACS4);
        }

        var maxAtaLevel = 0;
        var minAtaLevel = 255;
        var tmpString   = "";

        if(ata1)
        {
            supportedAtaVersions.Add("ATA-1");
            maxAtaLevel = 1;
            minAtaLevel = 1;
        }

        if(ata2)
        {
            supportedAtaVersions.Add("ATA-2");
            maxAtaLevel = 2;

            if(minAtaLevel > 2) minAtaLevel = 2;
        }

        if(ata3)
        {
            supportedAtaVersions.Add("ATA-3");
            maxAtaLevel = 3;

            if(minAtaLevel > 3) minAtaLevel = 3;
        }

        if(ata4)
        {
            supportedAtaVersions.Add("ATA/ATAPI-4");
            maxAtaLevel = 4;

            if(minAtaLevel > 4) minAtaLevel = 4;
        }

        if(ata5)
        {
            supportedAtaVersions.Add("ATA/ATAPI-5");
            maxAtaLevel = 5;

            if(minAtaLevel > 5) minAtaLevel = 5;
        }

        if(ata6)
        {
            supportedAtaVersions.Add("ATA/ATAPI-6");
            maxAtaLevel = 6;

            if(minAtaLevel > 6) minAtaLevel = 6;
        }

        if(ata7)
        {
            supportedAtaVersions.Add("ATA/ATAPI-7");
            maxAtaLevel = 7;

            if(minAtaLevel > 7) minAtaLevel = 7;
        }

        if(acs)
        {
            supportedAtaVersions.Add("ATA8-ACS");
            maxAtaLevel = 8;

            if(minAtaLevel > 8) minAtaLevel = 8;
        }

        if(acs2)
        {
            supportedAtaVersions.Add("ATA8-ACS2");
            maxAtaLevel = 9;

            if(minAtaLevel > 9) minAtaLevel = 9;
        }

        if(acs3)
        {
            supportedAtaVersions.Add("ATA8-ACS3");
            maxAtaLevel = 10;

            if(minAtaLevel > 10) minAtaLevel = 10;
        }

        if(acs4)
        {
            supportedAtaVersions.Add("ATA8-ACS4");
            maxAtaLevel = 11;

            if(minAtaLevel > 11) minAtaLevel = 11;
        }

        if(maxAtaLevel >= 3)
        {
            maximumAtaRevision = ataIdentify.MinorVersion switch
                                 {
                                     0x0000 or 0xFFFF => "Minor ATA version not specified",
                                     0x0001           => "ATA (ATA-1) X3T9.2 781D prior to revision 4",
                                     0x0002           => "ATA-1 published, ANSI X3.221-1994",
                                     0x0003           => "ATA (ATA-1) X3T9.2 781D revision 4",
                                     0x0004           => "ATA-2 published, ANSI X3.279-1996",
                                     0x0005           => "ATA-2 X3T10 948D prior to revision 2k",
                                     0x0006           => "ATA-3 X3T10 2008D revision 1",
                                     0x0007           => "ATA-2 X3T10 948D revision 2k",
                                     0x0008           => "ATA-3 X3T10 2008D revision 0",
                                     0x0009           => "ATA-2 X3T10 948D revision 3",
                                     0x000A           => "ATA-3 published, ANSI X3.298-1997",
                                     0x000B           => "ATA-3 X3T10 2008D revision 6",
                                     0x000C           => "ATA-3 X3T13 2008D revision 7",
                                     0x000D           => "ATA/ATAPI-4 X3T13 1153D revision 6",
                                     0x000E           => "ATA/ATAPI-4 T13 1153D revision 13",
                                     0x000F           => "ATA/ATAPI-4 X3T13 1153D revision 7",
                                     0x0010           => "ATA/ATAPI-4 T13 1153D revision 18",
                                     0x0011           => "ATA/ATAPI-4 T13 1153D revision 15",
                                     0x0012           => "ATA/ATAPI-4 published, ANSI INCITS 317-1998",
                                     0x0013           => "ATA/ATAPI-5 T13 1321D revision 3",
                                     0x0014           => "ATA/ATAPI-4 T13 1153D revision 14",
                                     0x0015           => "ATA/ATAPI-5 T13 1321D revision 1",
                                     0x0016           => "ATA/ATAPI-5 published, ANSI INCITS 340-2000",
                                     0x0017           => "ATA/ATAPI-4 T13 1153D revision 17",
                                     0x0018           => "ATA/ATAPI-6 T13 1410D revision 0",
                                     0x0019           => "ATA/ATAPI-6 T13 1410D revision 3a",
                                     0x001A           => "ATA/ATAPI-7 T13 1532D revision 1",
                                     0x001B           => "ATA/ATAPI-6 T13 1410D revision 2",
                                     0x001C           => "ATA/ATAPI-6 T13 1410D revision 1",
                                     0x001D           => "ATA/ATAPI-7 published ANSI INCITS 397-2005",
                                     0x001E           => "ATA/ATAPI-7 T13 1532D revision 0",
                                     0x001F           => "ACS-3 Revision 3b",
                                     0x0021           => "ATA/ATAPI-7 T13 1532D revision 4a",
                                     0x0022           => "ATA/ATAPI-6 published, ANSI INCITS 361-2002",
                                     0x0027           => "ATA8-ACS revision 3c",
                                     0x0028           => "ATA8-ACS revision 6",
                                     0x0029           => "ATA8-ACS revision 4",
                                     0x0031           => "ACS-2 Revision 2",
                                     0x0033           => "ATA8-ACS Revision 3e",
                                     0x0039           => "ATA8-ACS Revision 4c",
                                     0x0042           => "ATA8-ACS Revision 3f",
                                     0x0052           => "ATA8-ACS revision 3b",
                                     0x006D           => "ACS-3 Revision 5",
                                     0x0082           => "ACS-2 published, ANSI INCITS 482-2012",
                                     0x0107           => "ATA8-ACS revision 2d",
                                     0x0110           => "ACS-2 Revision 3",
                                     0x011B           => "ACS-3 Revision 4",
                                     _                => $"Unknown ATA revision 0x{ataIdentify.MinorVersion:X4}"
                                 };
        }

        switch((ataIdentify.TransportMajorVersion & 0xF000) >> 12)
        {
            case 0x0:
                if((ataIdentify.TransportMajorVersion & 0xFFF) > 0)
                {
                    transportVersions = [];
                    if((ataIdentify.TransportMajorVersion & 0x0002) == 0x0002) transportVersions.Add("ATA/ATAPI-7");

                    if((ataIdentify.TransportMajorVersion & 0x0001) == 0x0001) transportVersions.Add("ATA8-APT");
                }

                transport = "Parallel ATA device";

                break;
            case 0x1:
                if((ataIdentify.TransportMajorVersion & 0xFFF) > 0)
                {
                    transportVersions = [];
                    if((ataIdentify.TransportMajorVersion & 0x0001) == 0x0001) transportVersions.Add("ATA8-AST");

                    if((ataIdentify.TransportMajorVersion & 0x0002) == 0x0002) transportVersions.Add("SATA 1.0a");

                    if((ataIdentify.TransportMajorVersion & 0x0004) == 0x0004)
                        transportVersions.Add("SATA II Extensions");

                    if((ataIdentify.TransportMajorVersion & 0x0008) == 0x0008) transportVersions.Add("SATA 2.5");

                    if((ataIdentify.TransportMajorVersion & 0x0010) == 0x0010) transportVersions.Add("SATA 2.6");

                    if((ataIdentify.TransportMajorVersion & 0x0020) == 0x0020) transportVersions.Add("SATA 3.0");

                    if((ataIdentify.TransportMajorVersion & 0x0040) == 0x0040) transportVersions.Add("SATA 3.1");
                }

                transport = "Serial ATA device";

                break;
            case 0xE:
                transport = "SATA Express device";

                break;
            default:
                transport = $"Unknown transport type 0x{(ataIdentify.TransportMajorVersion & 0xF000) >> 12:X1}";

                break;
        }

        if(atapi)
        {
            // Bits 12 to 8, SCSI Peripheral Device Type
            switch((PeripheralDeviceTypes)(((ushort)ataIdentify.GeneralConfiguration & 0x1F00) >> 8))
            {
                case PeripheralDeviceTypes.DirectAccess: //0x00,
                    generalConfiguration.Add("ATAPI Direct-access device");

                    break;
                case PeripheralDeviceTypes.SequentialAccess: //0x01,
                    generalConfiguration.Add("ATAPI Sequential-access device");

                    break;
                case PeripheralDeviceTypes.PrinterDevice: //0x02,
                    generalConfiguration.Add("ATAPI Printer device");

                    break;
                case PeripheralDeviceTypes.ProcessorDevice: //0x03,
                    generalConfiguration.Add("ATAPI Processor device");

                    break;
                case PeripheralDeviceTypes.WriteOnceDevice: //0x04,
                    generalConfiguration.Add("ATAPI Write-once device");

                    break;
                case PeripheralDeviceTypes.MultiMediaDevice: //0x05,
                    generalConfiguration.Add("ATAPI CD-ROM/DVD/etc device");

                    break;
                case PeripheralDeviceTypes.ScannerDevice: //0x06,
                    generalConfiguration.Add("ATAPI Scanner device");

                    break;
                case PeripheralDeviceTypes.OpticalDevice: //0x07,
                    generalConfiguration.Add("ATAPI Optical memory device");

                    break;
                case PeripheralDeviceTypes.MediumChangerDevice: //0x08,
                    generalConfiguration.Add("ATAPI Medium change device");

                    break;
                case PeripheralDeviceTypes.CommsDevice: //0x09,
                    generalConfiguration.Add("ATAPI Communications device");

                    break;
                case PeripheralDeviceTypes.PrePressDevice1: //0x0A,
                    generalConfiguration.Add("ATAPI Graphics arts pre-press device (defined in ASC IT8)");

                    break;
                case PeripheralDeviceTypes.PrePressDevice2: //0x0B,
                    generalConfiguration.Add("ATAPI Graphics arts pre-press device (defined in ASC IT8)");

                    break;
                case PeripheralDeviceTypes.ArrayControllerDevice: //0x0C,
                    generalConfiguration.Add("ATAPI Array controller device");

                    break;
                case PeripheralDeviceTypes.EnclosureServiceDevice: //0x0D,
                    generalConfiguration.Add("ATAPI Enclosure services device");

                    break;
                case PeripheralDeviceTypes.SimplifiedDevice: //0x0E,
                    generalConfiguration.Add("ATAPI Simplified direct-access device");

                    break;
                case PeripheralDeviceTypes.OCRWDevice: //0x0F,
                    generalConfiguration.Add("ATAPI Optical card reader/writer device");

                    break;
                case PeripheralDeviceTypes.BridgingExpander: //0x10,
                    generalConfiguration.Add("ATAPI Bridging Expanders");

                    break;
                case PeripheralDeviceTypes.ObjectDevice: //0x11,
                    generalConfiguration.Add("ATAPI Object-based Storage Device");

                    break;
                case PeripheralDeviceTypes.ADCDevice: //0x12,
                    generalConfiguration.Add("ATAPI Automation/Drive Interface");

                    break;
                case PeripheralDeviceTypes.WellKnownDevice: //0x1E,
                    generalConfiguration.Add("ATAPI Well known logical unit");

                    break;
                case PeripheralDeviceTypes.UnknownDevice: //0x1F
                    generalConfiguration.Add("ATAPI Unknown or no device type");

                    break;
                default:
                    generalConfiguration
                       .Add($"ATAPI Unknown device type field value 0x{((ushort)ataIdentify.GeneralConfiguration & 0x1F00) >> 8:X2}");

                    break;
            }

            // ATAPI DRQ behaviour
            switch(((ushort)ataIdentify.GeneralConfiguration & 0x60) >> 5)
            {
                case 0:
                    generalConfiguration.Add("Device shall set DRQ within 3 ms of receiving PACKET");

                    break;
                case 1:
                    generalConfiguration.Add("Device shall assert INTRQ when DRQ is set to one");

                    break;
                case 2:
                    generalConfiguration.Add("Device shall set DRQ within 50 µs of receiving PACKET");

                    break;
                default:
                    generalConfiguration
                       .Add($"Unknown ATAPI DRQ behaviour code {((ushort)ataIdentify.GeneralConfiguration & 0x60) >> 5}");

                    break;
            }

            // ATAPI PACKET size
            switch((ushort)ataIdentify.GeneralConfiguration & 0x03)
            {
                case 0:
                    generalConfiguration.Add("ATAPI device uses 12 byte command packet");

                    break;
                case 1:
                    generalConfiguration.Add("ATAPI device uses 16 byte command packet");

                    break;
                default:
                    generalConfiguration
                       .Add($"Unknown ATAPI packet size code {(ushort)ataIdentify.GeneralConfiguration & 0x03}");

                    break;
            }
        }
        else if(!cfa)
        {
            if(minAtaLevel >= 5)
            {
                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.IncompleteResponse))
                    generalConfiguration.Add("Incomplete identify response");
            }

            if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.NonMagnetic))
                generalConfiguration.Add("Device uses non-magnetic media");

            if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.Removable))
                generalConfiguration.Add("Device is removable");

            if(minAtaLevel <= 5)
            {
                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.Fixed))
                    generalConfiguration.Add("Device is fixed");
            }

            if(ata1)
            {
                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.SlowIDE))
                    generalConfiguration.Add("Device transfer rate is <= 5 Mb/s");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.FastIDE))
                    generalConfiguration.Add("Device transfer rate is > 5 Mb/s but <= 10 Mb/s");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.UltraFastIDE))
                    generalConfiguration.Add("Device transfer rate is > 10 Mb/s");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.SoftSector))
                    generalConfiguration.Add("Device is soft sectored");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.HardSector))
                    generalConfiguration.Add("Device is hard sectored");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.NotMFM))
                    generalConfiguration.Add("Device is not MFM encoded");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.FormatGapReq))
                    generalConfiguration.Add("Format speed tolerance gap is required");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.TrackOffset))
                    generalConfiguration.Add("Track offset option is available");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.DataStrobeOffset))
                    generalConfiguration.Add("Data strobe offset option is available");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.RotationalSpeedTolerance))
                    generalConfiguration.Add("Rotational speed tolerance is higher than 0,5%");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.SpindleControl))
                    generalConfiguration.Add("Spindle motor control is implemented");

                if(ataIdentify.GeneralConfiguration.HasFlag(Identify.GeneralConfigurationBit.HighHeadSwitch))
                    generalConfiguration.Add("Head switch time is bigger than 15 µs.");
            }
        }

        if((ushort)ataIdentify.SpecificConfiguration != 0x0000 && (ushort)ataIdentify.SpecificConfiguration != 0xFFFF)
        {
            specificConfiguration ??= [];

            switch(ataIdentify.SpecificConfiguration)
            {
                case Identify.SpecificConfigurationEnum.RequiresSetIncompleteResponse:
                    specificConfiguration
                       .Add("Device requires SET FEATURES to spin up and IDENTIFY DEVICE response is incomplete.");

                    break;
                case Identify.SpecificConfigurationEnum.RequiresSetCompleteResponse:
                    specificConfiguration
                       .Add("Device requires SET FEATURES to spin up and IDENTIFY DEVICE response is complete.");

                    break;
                case Identify.SpecificConfigurationEnum.NotRequiresSetIncompleteResponse:
                    specificConfiguration
                       .Add("Device does not require SET FEATURES to spin up and IDENTIFY DEVICE response is incomplete.");

                    break;
                case Identify.SpecificConfigurationEnum.NotRequiresSetCompleteResponse:
                    specificConfiguration
                       .Add("Device does not require SET FEATURES to spin up and IDENTIFY DEVICE response is complete.");

                    break;
                default:
                    specificConfiguration
                       .Add($"Unknown device specific configuration 0x{(ushort)ataIdentify.SpecificConfiguration:X4}");

                    break;
            }
        }

        // Obsolete since ATA-2, however, it is yet used in ATA-8 devices
        if(ataIdentify.BufferSize != 0x0000 &&
           ataIdentify.BufferSize != 0xFFFF &&
           ataIdentify.BufferType != 0x0000 &&
           ataIdentify.BufferType != 0xFFFF)
        {
            switch(ataIdentify.BufferType)
            {
                case 1:
                    deviceCapabilities
                       .Add($"{ataIdentify.BufferSize * logicalSectorSize / 1024} KiB of single ported single sector buffer");

                    break;
                case 2:
                    deviceCapabilities
                       .Add($"{ataIdentify.BufferSize * logicalSectorSize / 1024} KiB of dual ported multi sector buffer");

                    break;
                case 3:
                    deviceCapabilities
                       .Add($"{ataIdentify.BufferSize * logicalSectorSize / 1024} KiB of dual ported multi sector buffer with read caching");

                    break;
                default:
                    deviceCapabilities
                       .Add($"{ataIdentify.BufferSize * logicalSectorSize / 1024} KiB of unknown type {ataIdentify.BufferType} buffer");

                    break;
            }
        }

        if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.StandardStandbyTimer))
            deviceCapabilities.Add("Standby time values are standard");

        if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.IORDY))
        {
            deviceCapabilities.Add(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.CanDisableIORDY)
                                       ? "IORDY is supported and can be disabled"
                                       : "IORDY is supported");
        }

        if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.DMASupport))
            deviceCapabilities.Add("DMA is supported");

        if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.PhysicalAlignment1) ||
           ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.PhysicalAlignment0))
            deviceCapabilities.Add($"Long Physical Alignment setting is {(ushort)ataIdentify.Capabilities & 0x03}");

        if(atapi)
        {
            if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.InterleavedDMA))
                deviceCapabilities.Add("ATAPI device supports interleaved DMA");

            if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.CommandQueue))
                deviceCapabilities.Add("ATAPI device supports command queueing");

            if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.OverlapOperation))
                deviceCapabilities.Add("ATAPI device supports overlapped operations");

            if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.RequiresATASoftReset))
                deviceCapabilities.Add("ATAPI device requires ATA software reset");
        }

        if(ataIdentify.Capabilities2.HasFlag(Identify.CapabilitiesBit2.MustBeSet) &&
           !ataIdentify.Capabilities2.HasFlag(Identify.CapabilitiesBit2.MustBeClear))
        {
            if(ataIdentify.Capabilities2.HasFlag(Identify.CapabilitiesBit2.SpecificStandbyTimer))
                deviceCapabilities.Add("Device indicates a specific minimum standby timer value");
        }

        if(ataIdentify.Capabilities3.HasFlag(Identify.CapabilitiesBit3.MultipleValid))
        {
            deviceCapabilities
               .Add($"A maximum of {ataIdentify.MultipleSectorNumber} sectors can be transferred per interrupt on READ/WRITE MULTIPLE");

            deviceCapabilities.Add($"Device supports setting a maximum of {ataIdentify.MultipleMaxSectors} sectors");
        }

        if(ata1)
        {
            if(ataIdentify.TrustedComputing.HasFlag(Identify.TrustedComputingBit.TrustedComputing))
                deviceCapabilities.Add("Device supports doubleword I/O");
        }

        if(minAtaLevel <= 3)
        {
            if(ataIdentify.PIOTransferTimingMode > 0)
                pioTransferModes.Add($"PIO timing mode {ataIdentify.PIOTransferTimingMode}");

            if(ataIdentify.DMATransferTimingMode > 0)
                dmaTransferModes.Add($"DMA timing mode {ataIdentify.DMATransferTimingMode}");
        }

        if(ataIdentify.APIOSupported.HasFlag(Identify.TransferMode.Mode0)) pioTransferModes.Add("PIO0");

        if(ataIdentify.APIOSupported.HasFlag(Identify.TransferMode.Mode1)) pioTransferModes.Add("PIO1");

        if(ataIdentify.APIOSupported.HasFlag(Identify.TransferMode.Mode2)) pioTransferModes.Add("PIO2");

        if(ataIdentify.APIOSupported.HasFlag(Identify.TransferMode.Mode3)) pioTransferModes.Add("PIO3");

        if(ataIdentify.APIOSupported.HasFlag(Identify.TransferMode.Mode4)) pioTransferModes.Add("PIO4");

        if(ataIdentify.APIOSupported.HasFlag(Identify.TransferMode.Mode5)) pioTransferModes.Add("PIO5");

        if(ataIdentify.APIOSupported.HasFlag(Identify.TransferMode.Mode6)) pioTransferModes.Add("PIO6");

        if(ataIdentify.APIOSupported.HasFlag(Identify.TransferMode.Mode7)) pioTransferModes.Add("PIO7");

        if(minAtaLevel <= 3 && !atapi)
        {
            if(ataIdentify.DMASupported.HasFlag(Identify.TransferMode.Mode0))
            {
                dmaTransferModes.Add(ataIdentify.DMAActive.HasFlag(Identify.TransferMode.Mode0)
                                         ? "DMA0 (active)"
                                         : "DMA0");
            }

            if(ataIdentify.DMASupported.HasFlag(Identify.TransferMode.Mode1))
            {
                dmaTransferModes.Add(ataIdentify.DMAActive.HasFlag(Identify.TransferMode.Mode1)
                                         ? "DMA1 (active)"
                                         : "DMA1");
            }

            if(ataIdentify.DMASupported.HasFlag(Identify.TransferMode.Mode2))
            {
                dmaTransferModes.Add(ataIdentify.DMAActive.HasFlag(Identify.TransferMode.Mode2)
                                         ? "DMA2 (active)"
                                         : "DMA2");
            }

            if(ataIdentify.DMASupported.HasFlag(Identify.TransferMode.Mode3))
            {
                dmaTransferModes.Add(ataIdentify.DMAActive.HasFlag(Identify.TransferMode.Mode3)
                                         ? "DMA3 (active)"
                                         : "DMA3");
            }

            if(ataIdentify.DMASupported.HasFlag(Identify.TransferMode.Mode4))
            {
                dmaTransferModes.Add(ataIdentify.DMAActive.HasFlag(Identify.TransferMode.Mode4)
                                         ? "DMA4 (active)"
                                         : "DMA4");
            }

            if(ataIdentify.DMASupported.HasFlag(Identify.TransferMode.Mode5))
            {
                dmaTransferModes.Add(ataIdentify.DMAActive.HasFlag(Identify.TransferMode.Mode5)
                                         ? "DMA5 (active)"
                                         : "DMA5");
            }

            if(ataIdentify.DMASupported.HasFlag(Identify.TransferMode.Mode6))
            {
                dmaTransferModes.Add(ataIdentify.DMAActive.HasFlag(Identify.TransferMode.Mode6)
                                         ? "DMA6 (active)"
                                         : "DMA6");
            }

            if(ataIdentify.DMASupported.HasFlag(Identify.TransferMode.Mode7))
            {
                dmaTransferModes.Add(ataIdentify.DMAActive.HasFlag(Identify.TransferMode.Mode7)
                                         ? "DMA7 (active)"
                                         : "DMA7");
            }
        }

        if(ataIdentify.MDMASupported.HasFlag(Identify.TransferMode.Mode0))
        {
            mdmaTransferModes.Add(ataIdentify.MDMAActive.HasFlag(Identify.TransferMode.Mode0)
                                      ? "MDMA0 (active)"
                                      : "MDMA0");
        }

        if(ataIdentify.MDMASupported.HasFlag(Identify.TransferMode.Mode1))
        {
            mdmaTransferModes.Add(ataIdentify.MDMAActive.HasFlag(Identify.TransferMode.Mode1)
                                      ? "MDMA1 (active)"
                                      : "MDMA1");
        }

        if(ataIdentify.MDMASupported.HasFlag(Identify.TransferMode.Mode2))
        {
            mdmaTransferModes.Add(ataIdentify.MDMAActive.HasFlag(Identify.TransferMode.Mode2)
                                      ? "MDMA2 (active)"
                                      : "MDMA2");
        }

        if(ataIdentify.MDMASupported.HasFlag(Identify.TransferMode.Mode3))
        {
            mdmaTransferModes.Add(ataIdentify.MDMAActive.HasFlag(Identify.TransferMode.Mode3)
                                      ? "MDMA3 (active)"
                                      : "MDMA3");
        }

        if(ataIdentify.MDMASupported.HasFlag(Identify.TransferMode.Mode4))
        {
            mdmaTransferModes.Add(ataIdentify.MDMAActive.HasFlag(Identify.TransferMode.Mode4)
                                      ? "MDMA4 (active)"
                                      : "MDMA4");
        }

        if(ataIdentify.MDMASupported.HasFlag(Identify.TransferMode.Mode5))
        {
            mdmaTransferModes.Add(ataIdentify.MDMAActive.HasFlag(Identify.TransferMode.Mode5)
                                      ? "MDMA5 (active)"
                                      : "MDMA5");
        }

        if(ataIdentify.MDMASupported.HasFlag(Identify.TransferMode.Mode6))
        {
            mdmaTransferModes.Add(ataIdentify.MDMAActive.HasFlag(Identify.TransferMode.Mode6)
                                      ? "MDMA6 (active)"
                                      : "MDMA6");
        }

        if(ataIdentify.MDMASupported.HasFlag(Identify.TransferMode.Mode7))
        {
            mdmaTransferModes.Add(ataIdentify.MDMAActive.HasFlag(Identify.TransferMode.Mode7)
                                      ? "MDMA7 (active)"
                                      : "MDMA7");
        }

        if(ataIdentify.UDMASupported.HasFlag(Identify.TransferMode.Mode0))
        {
            ultraDmaTransferModes.Add(ataIdentify.UDMAActive.HasFlag(Identify.TransferMode.Mode0)
                                          ? "UDMA0 (active)"
                                          : "UDMA0");
        }

        if(ataIdentify.UDMASupported.HasFlag(Identify.TransferMode.Mode1))
        {
            ultraDmaTransferModes.Add(ataIdentify.UDMAActive.HasFlag(Identify.TransferMode.Mode1)
                                          ? "UDMA1 (active)"
                                          : "UDMA1");
        }

        if(ataIdentify.UDMASupported.HasFlag(Identify.TransferMode.Mode2))
        {
            ultraDmaTransferModes.Add(ataIdentify.UDMAActive.HasFlag(Identify.TransferMode.Mode2)
                                          ? "UDMA2 (active)"
                                          : "UDMA2");
        }

        if(ataIdentify.UDMASupported.HasFlag(Identify.TransferMode.Mode3))
        {
            ultraDmaTransferModes.Add(ataIdentify.UDMAActive.HasFlag(Identify.TransferMode.Mode3)
                                          ? "UDMA3 (active)"
                                          : "UDMA3");
        }

        if(ataIdentify.UDMASupported.HasFlag(Identify.TransferMode.Mode4))
        {
            ultraDmaTransferModes.Add(ataIdentify.UDMAActive.HasFlag(Identify.TransferMode.Mode4)
                                          ? "UDMA4 (active)"
                                          : "UDMA4");
        }

        if(ataIdentify.UDMASupported.HasFlag(Identify.TransferMode.Mode5))
        {
            ultraDmaTransferModes.Add(ataIdentify.UDMAActive.HasFlag(Identify.TransferMode.Mode5)
                                          ? "UDMA5 (active)"
                                          : "UDMA5");
        }

        if(ataIdentify.UDMASupported.HasFlag(Identify.TransferMode.Mode6))
        {
            ultraDmaTransferModes.Add(ataIdentify.UDMAActive.HasFlag(Identify.TransferMode.Mode6)
                                          ? "UDMA6 (active)"
                                          : "UDMA6");
        }

        if(ataIdentify.UDMASupported.HasFlag(Identify.TransferMode.Mode7))
        {
            ultraDmaTransferModes.Add(ataIdentify.UDMAActive.HasFlag(Identify.TransferMode.Mode7)
                                          ? "UDMA7 (active)"
                                          : "UDMA7");
        }

        if(ataIdentify.MinMDMACycleTime != 0 && ataIdentify.RecMDMACycleTime != 0)
        {
            specificConfiguration ??= [];

            specificConfiguration
               .Add($"At minimum {ataIdentify.MinMDMACycleTime} ns. transfer cycle time per word in MDMA, " +
                    $"{ataIdentify.RecMDMACycleTime} ns. recommended");
        }

        if(ataIdentify.MinPIOCycleTimeNoFlow != 0)
        {
            specificConfiguration ??= [];

            specificConfiguration
               .Add($"At minimum {ataIdentify.MinPIOCycleTimeNoFlow} ns. transfer cycle time per word in PIO, " +
                    "without flow control");
        }

        if(ataIdentify.MinPIOCycleTimeFlow != 0)
        {
            specificConfiguration ??= [];

            specificConfiguration
               .Add($"At minimum {ataIdentify.MinPIOCycleTimeFlow} ns. transfer cycle time per word in PIO, " +
                    "with IORDY flow control");
        }

        if(ataIdentify.MaxQueueDepth != 0)
        {
            specificConfiguration ??= [];
            specificConfiguration.Add($"{ataIdentify.MaxQueueDepth + 1} depth of queue maximum");
        }

        if(atapi)
        {
            if(ataIdentify.PacketBusRelease != 0)
            {
                specificConfiguration ??= [];

                specificConfiguration
                   .Add($"{ataIdentify.PacketBusRelease} ns. typical to release bus from receipt of PACKET");
            }

            if(ataIdentify.ServiceBusyClear != 0)
            {
                specificConfiguration ??= [];

                specificConfiguration
                   .Add($"{ataIdentify.ServiceBusyClear} ns. typical to clear BSY bit from receipt of SERVICE");
            }
        }

        if((ataIdentify.TransportMajorVersion & 0xF000) >> 12 == 0x1 ||
           (ataIdentify.TransportMajorVersion & 0xF000) >> 12 == 0xE)
        {
            if(!ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.Clear))
            {
                if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.Gen1Speed))
                    deviceCapabilities.Add("SATA 1.5Gb/s is supported");

                if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.Gen2Speed))
                    deviceCapabilities.Add("SATA 3.0Gb/s is supported");

                if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.Gen3Speed))
                    deviceCapabilities.Add("SATA 6.0Gb/s is supported");

                if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.PowerReceipt))
                    deviceCapabilities.Add("Receipt of host initiated power management requests is supported");

                if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.PHYEventCounter))
                    deviceCapabilities.Add("PHY Event counters are supported");

                if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.HostSlumbTrans))
                    deviceCapabilities.Add("Supports host automatic partial to slumber transitions is supported");

                if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.DevSlumbTrans))
                    deviceCapabilities.Add("Supports device automatic partial to slumber transitions is supported");

                if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.NCQ))
                {
                    deviceCapabilities.Add("NCQ is supported");

                    if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.NCQPriority))
                        deviceCapabilities.Add("NCQ priority is supported");

                    if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.UnloadNCQ))
                        deviceCapabilities.Add("Unload is supported with outstanding NCQ commands");
                }
            }

            if(!ataIdentify.SATACapabilities2.HasFlag(Identify.SATACapabilitiesBit2.Clear))
            {
                if(!ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.Clear) &&
                   ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.NCQ))
                {
                    if(ataIdentify.SATACapabilities2.HasFlag(Identify.SATACapabilitiesBit2.NCQMgmt))
                        deviceCapabilities.Add("NCQ queue management is supported");

                    if(ataIdentify.SATACapabilities2.HasFlag(Identify.SATACapabilitiesBit2.NCQStream))
                        deviceCapabilities.Add("NCQ streaming is supported");
                }

                if(atapi)
                {
                    if(ataIdentify.SATACapabilities2.HasFlag(Identify.SATACapabilitiesBit2.HostEnvDetect))
                        deviceCapabilities.Add("ATAPI device supports host environment detection");

                    if(ataIdentify.SATACapabilities2.HasFlag(Identify.SATACapabilitiesBit2.DevAttSlimline))
                        deviceCapabilities.Add("ATAPI device supports attention on slimline connected devices");
                }
            }
        }

        if(ataIdentify.InterseekDelay != 0x0000 && ataIdentify.InterseekDelay != 0xFFFF)
        {
            specificConfiguration ??= [];

            specificConfiguration
               .Add($"{ataIdentify.InterseekDelay} microseconds of interseek delay for ISO-7779 acoustic testing");
        }

        if((ushort)ataIdentify.DeviceFormFactor != 0x0000 && (ushort)ataIdentify.DeviceFormFactor != 0xFFFF)
        {
            specificConfiguration ??= [];

            switch(ataIdentify.DeviceFormFactor)
            {
                case Identify.DeviceFormFactorEnum.FiveAndQuarter:
                    specificConfiguration.Add("Device nominal size is 5.25\"");

                    break;
                case Identify.DeviceFormFactorEnum.ThreeAndHalf:
                    specificConfiguration.Add("Device nominal size is 3.5\"");

                    break;
                case Identify.DeviceFormFactorEnum.TwoAndHalf:
                    specificConfiguration.Add("Device nominal size is 2.5\"");

                    break;
                case Identify.DeviceFormFactorEnum.OnePointEight:
                    specificConfiguration.Add("Device nominal size is 1.8\"");

                    break;
                case Identify.DeviceFormFactorEnum.LessThanOnePointEight:
                    specificConfiguration.Add("Device nominal size is smaller than 1.8\"");

                    break;
                default:
                    specificConfiguration
                       .Add($"Device nominal size field value {ataIdentify.DeviceFormFactor} is unknown");

                    break;
            }
        }

        if(atapi && ataIdentify.ATAPIByteCount > 0)
        {
            specificConfiguration ??= [];
            specificConfiguration.Add($"{ataIdentify.ATAPIByteCount} bytes count limit for ATAPI");
        }

        if(cfa)
        {
            if((ataIdentify.CFAPowerMode & 0x8000) == 0x8000)
            {
                specificConfiguration ??= [];

                specificConfiguration.Add("CompactFlash device supports power mode 1");

                if((ataIdentify.CFAPowerMode & 0x2000) == 0x2000)
                    specificConfiguration.Add("CompactFlash power mode 1 required for one or more commands");

                if((ataIdentify.CFAPowerMode & 0x1000) == 0x1000)
                    specificConfiguration.Add("CompactFlash power mode 1 is disabled");

                specificConfiguration
                   .Add($"CompactFlash device uses a maximum of {ataIdentify.CFAPowerMode & 0x0FFF} mA");
            }
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.Nop))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.Nop)
                                          ? "NOP is supported and enabled"
                                          : "NOP is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.ReadBuffer))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.ReadBuffer)
                                          ? "READ BUFFER is supported and enabled"
                                          : "READ BUFFER is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.WriteBuffer))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.WriteBuffer)
                                          ? "WRITE BUFFER is supported and enabled"
                                          : "WRITE BUFFER is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.HPA))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.HPA)
                                          ? "Host Protected Area is supported and enabled"
                                          : "Host Protected Area is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.DeviceReset))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.DeviceReset)
                                          ? "DEVICE RESET is supported and enabled"
                                          : "DEVICE RESET is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.Service))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.Service)
                                          ? "SERVICE interrupt is supported and enabled"
                                          : "SERVICE interrupt is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.Release))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.Release)
                                          ? "Release is supported and enabled"
                                          : "Release is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.LookAhead))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.LookAhead)
                                          ? "Look-ahead read is supported and enabled"
                                          : "Look-ahead read is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.WriteCache))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.WriteCache)
                                          ? "Write cache is supported and enabled"
                                          : "Write cache is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.Packet))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.Packet)
                                          ? "PACKET is supported and enabled"
                                          : "PACKET is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.PowerManagement))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.PowerManagement)
                                          ? "Power management is supported and enabled"
                                          : "Power management is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.RemovableMedia))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.RemovableMedia)
                                          ? "Removable media feature set is supported and enabled"
                                          : "Removable media feature set is supported");
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.SecurityMode))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.SecurityMode)
                                          ? "Security mode is supported and enabled"
                                          : "Security mode is supported");
        }

        if(ataIdentify.Capabilities.HasFlag(Identify.CapabilitiesBit.LBASupport))
            commandSetAndFeatures.Add("28-bit LBA is supported");

        if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.MustBeSet) &&
           !ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.MustBeClear))
        {
            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.LBA48))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.LBA48)
                                              ? "48-bit LBA is supported and enabled"
                                              : "48-bit LBA is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.FlushCache))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.FlushCache)
                                              ? "FLUSH CACHE is supported and enabled"
                                              : "FLUSH CACHE is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.FlushCacheExt))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.FlushCacheExt)
                                              ? "FLUSH CACHE EXT is supported and enabled"
                                              : "FLUSH CACHE EXT is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.DCO))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.DCO)
                                              ? "Device Configuration Overlay feature set is supported and enabled"
                                              : "Device Configuration Overlay feature set is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.AAM))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.AAM)
                                              ? $"Automatic Acoustic Management is supported and enabled with value {ataIdentify.CurrentAAM} (vendor recommends {ataIdentify.RecommendedAAM}"
                                              : "Automatic Acoustic Management is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.SetMax))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.SetMax)
                                              ? "SET MAX security extension is supported and enabled"
                                              : "SET MAX security extension is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.AddressOffsetReservedAreaBoot))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2
                                             .AddressOffsetReservedAreaBoot)
                                              ? "Address Offset Reserved Area Boot is supported and enabled"
                                              : "Address Offset Reserved Area Boot is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.SetFeaturesRequired))
                commandSetAndFeatures.Add("SET FEATURES is required before spin-up");

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.PowerUpInStandby))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2
                                             .PowerUpInStandby)
                                              ? "Power-up in standby is supported and enabled"
                                              : "Power-up in standby is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.RemovableNotification))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2
                                             .RemovableNotification)
                                              ? "Removable Media Status Notification is supported and enabled"
                                              : "Removable Media Status Notification is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.APM))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.APM)
                                              ? $"Advanced Power Management is supported and enabled with value {ataIdentify.CurrentAPM}"
                                              : "Advanced Power Management is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.CompactFlash))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.CompactFlash)
                                              ? "CompactFlash feature set is supported and enabled"
                                              : "CompactFlash feature set is supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.RWQueuedDMA))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2.RWQueuedDMA)
                                              ? "READ DMA QUEUED and WRITE DMA QUEUED are supported and enabled"
                                              : "READ DMA QUEUED and WRITE DMA QUEUED are supported");
            }

            if(ataIdentify.CommandSet2.HasFlag(Identify.CommandSetBit2.DownloadMicrocode))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet2.HasFlag(Identify.CommandSetBit2
                                             .DownloadMicrocode)
                                              ? "DOWNLOAD MICROCODE is supported and enabled"
                                              : "DOWNLOAD MICROCODE is supported");
            }
        }

        if(ataIdentify.CommandSet.HasFlag(Identify.CommandSetBit.SMART))
        {
            commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet.HasFlag(Identify.CommandSetBit.SMART)
                                          ? "S.M.A.R.T. is supported and enabled"
                                          : "S.M.A.R.T. is supported");
        }

        if(ataIdentify.SCTCommandTransport.HasFlag(Identify.SCTCommandTransportBit.Supported))
            commandSetAndFeatures.Add("S.M.A.R.T. Command Transport is supported");

        if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.MustBeSet) &&
           !ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.MustBeClear))
        {
            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.SMARTSelfTest))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.SMARTSelfTest)
                                              ? "S.M.A.R.T. self-testing is supported and enabled"
                                              : "S.M.A.R.T. self-testing is supported");
            }

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.SMARTLog))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.SMARTLog)
                                              ? "S.M.A.R.T. error logging is supported and enabled"
                                              : "S.M.A.R.T. error logging is supported");
            }

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.IdleImmediate))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.IdleImmediate)
                                              ? "IDLE IMMEDIATE with UNLOAD FEATURE is supported and enabled"
                                              : "IDLE IMMEDIATE with UNLOAD FEATURE is supported");
            }

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.WriteURG))
                commandSetAndFeatures.Add("URG bit is supported in WRITE STREAM DMA EXT and WRITE STREAM EXT");

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.ReadURG))
                commandSetAndFeatures.Add("URG bit is supported in READ STREAM DMA EXT and READ STREAM EXT");

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.WWN))
                commandSetAndFeatures.Add("Device has a World Wide Name");

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.FUAWriteQ))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.FUAWriteQ)
                                              ? "WRITE DMA QUEUED FUA EXT is supported and enabled"
                                              : "WRITE DMA QUEUED FUA EXT is supported");
            }

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.FUAWrite))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.FUAWrite)
                                              ? "WRITE DMA FUA EXT and WRITE MULTIPLE FUA EXT are supported and enabled"
                                              : "WRITE DMA FUA EXT and WRITE MULTIPLE FUA EXT are supported");
            }

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.GPL))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.GPL)
                                              ? "General Purpose Logging is supported and enabled"
                                              : "General Purpose Logging is supported");
            }

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.Streaming))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.Streaming)
                                              ? "Streaming feature set is supported and enabled"
                                              : "Streaming feature set is supported");
            }

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.MCPT))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.MCPT)
                                              ? "Media Card Pass Through command set is supported and enabled"
                                              : "Media Card Pass Through command set is supported");
            }

            if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.MediaSerial))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet3.HasFlag(Identify.CommandSetBit3.MediaSerial)
                                              ? "Media Serial is supported and valid"
                                              : "Media Serial is supported");
            }
        }

        if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.MustBeSet) &&
           !ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.MustBeClear))
        {
            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.DSN))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4.DSN)
                                              ? "DSN feature set is supported and enabled"
                                              : "DSN feature set is supported");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.AMAC))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4.AMAC)
                                              ? "Accessible Max Address Configuration is supported and enabled"
                                              : "Accessible Max Address Configuration is supported");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.ExtPowerCond))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4.ExtPowerCond)
                                              ? "Extended Power Conditions are supported and enabled"
                                              : "Extended Power Conditions are supported");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.ExtStatusReport))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4
                                             .ExtStatusReport)
                                              ? "Extended Status Reporting is supported and enabled"
                                              : "Extended Status Reporting is supported");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.FreeFallControl))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4
                                             .FreeFallControl)
                                              ? "Free-fall control feature set is supported and enabled"
                                              : "Free-fall control feature set is supported");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.SegmentedDownloadMicrocode))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4
                                             .SegmentedDownloadMicrocode)
                                              ? "Segmented feature in DOWNLOAD MICROCODE is supported and enabled"
                                              : "Segmented feature in DOWNLOAD MICROCODE is supported");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.RWDMAExtGpl))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4.RWDMAExtGpl)
                                              ? "READ/WRITE DMA EXT GPL are supported and enabled"
                                              : "READ/WRITE DMA EXT GPL are supported");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.WriteUnc))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4.WriteUnc)
                                              ? "WRITE UNCORRECTABLE is supported and enabled"
                                              : "WRITE UNCORRECTABLE is supported");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.WRV))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4.WRV)
                                              ? "Write/Read/Verify is supported and enabled"
                                              : "Write/Read/Verify is supported");

                commandSetAndFeatures.Add($"{ataIdentify.WRVSectorCountMode2} sectors for Write/Read/Verify mode 2");
                commandSetAndFeatures.Add($"{ataIdentify.WRVSectorCountMode3} sectors for Write/Read/Verify mode 3");

                if(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4.WRV))
                    commandSetAndFeatures.Add($"Current Write/Read/Verify mode: {ataIdentify.WRVMode}");
            }

            if(ataIdentify.CommandSet4.HasFlag(Identify.CommandSetBit4.DT1825))
            {
                commandSetAndFeatures.Add(ataIdentify.EnabledCommandSet4.HasFlag(Identify.CommandSetBit4.DT1825)
                                              ? "DT1825 is supported and enabled"
                                              : "DT1825 is supported");
            }
        }

        if(ataIdentify.Capabilities3.HasFlag(Identify.CapabilitiesBit3.BlockErase))
            commandSetAndFeatures.Add("BLOCK ERASE EXT is supported");

        if(ataIdentify.Capabilities3.HasFlag(Identify.CapabilitiesBit3.Overwrite))
            commandSetAndFeatures.Add("OVERWRITE EXT is supported");

        if(ataIdentify.Capabilities3.HasFlag(Identify.CapabilitiesBit3.CryptoScramble))
            commandSetAndFeatures.Add("CRYPTO SCRAMBLE EXT is supported");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.DeviceConfDMA))
        {
            commandSetAndFeatures
               .Add("DEVICE CONFIGURATION IDENTIFY DMA and DEVICE CONFIGURATION SET DMA are supported");
        }

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.ReadBufferDMA))
            commandSetAndFeatures.Add("READ BUFFER DMA is supported");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.WriteBufferDMA))
            commandSetAndFeatures.Add("WRITE BUFFER DMA is supported");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.DownloadMicroCodeDMA))
            commandSetAndFeatures.Add("DOWNLOAD MICROCODE DMA is supported");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.SetMaxDMA))
            commandSetAndFeatures.Add("SET PASSWORD DMA and SET UNLOCK DMA are supported");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.Ata28))
            commandSetAndFeatures.Add("Not all 28-bit commands are supported");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.CFast))
            commandSetAndFeatures.Add("Device follows CFast specification");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.IEEE1667))
            commandSetAndFeatures.Add("Device follows IEEE-1667");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.DeterministicTrim))
        {
            commandSetAndFeatures.Add("Read after TRIM is deterministic");

            if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.ReadZeroTrim))
                commandSetAndFeatures.Add("Read after TRIM returns empty data");
        }

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.LongPhysSectorAligError))
            commandSetAndFeatures.Add("Device supports Long Physical Sector Alignment Error Reporting Control");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.Encrypted))
            commandSetAndFeatures.Add("Device encrypts all user data");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.AllCacheNV))
            commandSetAndFeatures.Add("Device's write cache is non-volatile");

        if(ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.ZonedBit0) ||
           ataIdentify.CommandSet5.HasFlag(Identify.CommandSetBit5.ZonedBit1))
            commandSetAndFeatures.Add("Device is zoned");

        if(ataIdentify.Capabilities3.HasFlag(Identify.CapabilitiesBit3.Sanitize))
        {
            commandSetAndFeatures.Add("Sanitize feature set is supported");

            commandSetAndFeatures.Add(ataIdentify.Capabilities3.HasFlag(Identify.CapabilitiesBit3.SanitizeCommands)
                                          ? "Sanitize commands are specified by ACS-3 or higher"
                                          : "Sanitize commands are specified by ACS-2");

            if(ataIdentify.Capabilities3.HasFlag(Identify.CapabilitiesBit3.SanitizeAntifreeze))
                commandSetAndFeatures.Add("SANITIZE ANTIFREEZE LOCK EXT is supported");
        }


        if(!ata1 && maxAtaLevel >= 8)
        {
            if(ataIdentify.TrustedComputing.HasFlag(Identify.TrustedComputingBit.Set)    &&
               !ataIdentify.TrustedComputing.HasFlag(Identify.TrustedComputingBit.Clear) &&
               ataIdentify.TrustedComputing.HasFlag(Identify.TrustedComputingBit.TrustedComputing))
                commandSetAndFeatures.Add("Trusted Computing feature set is supported");
        }

        if((ataIdentify.TransportMajorVersion & 0xF000) >> 12 == 0x1 ||
           (ataIdentify.TransportMajorVersion & 0xF000) >> 12 == 0xE)
        {
            if(true)
            {
                if(!ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.Clear))
                {
                    if(ataIdentify.SATACapabilities.HasFlag(Identify.SATACapabilitiesBit.ReadLogDMAExt))
                        commandSetAndFeatures.Add("READ LOG DMA EXT is supported");
                }
            }

            if(true)
            {
                if(!ataIdentify.SATACapabilities2.HasFlag(Identify.SATACapabilitiesBit2.Clear))
                {
                    if(ataIdentify.SATACapabilities2.HasFlag(Identify.SATACapabilitiesBit2.FPDMAQ))
                        commandSetAndFeatures.Add("RECEIVE FPDMA QUEUED and SEND FPDMA QUEUED are supported");
                }
            }

            if(true)
            {
                if(!ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.Clear))
                {
                    if(ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.NonZeroBufferOffset))
                    {
                        commandSetAndFeatures.Add(ataIdentify.EnabledSATAFeatures.HasFlag(Identify.SATAFeaturesBit
                                                     .NonZeroBufferOffset)
                                                      ? "Non-zero buffer offsets are supported and enabled"
                                                      : "Non-zero buffer offsets are supported");
                    }

                    if(ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.DMASetup))
                    {
                        commandSetAndFeatures.Add(ataIdentify.EnabledSATAFeatures.HasFlag(Identify.SATAFeaturesBit
                                                     .DMASetup)
                                                      ? "DMA Setup auto-activation is supported and enabled"
                                                      : "DMA Setup auto-activation is supported");
                    }

                    if(ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.InitPowerMgmt))
                    {
                        commandSetAndFeatures.Add(ataIdentify.EnabledSATAFeatures.HasFlag(Identify.SATAFeaturesBit
                                                     .InitPowerMgmt)
                                                      ? "Device-initiated power management is supported and enabled"
                                                      : "Device-initiated power management is supported");
                    }

                    if(ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.InOrderData))
                    {
                        commandSetAndFeatures.Add(ataIdentify.EnabledSATAFeatures.HasFlag(Identify.SATAFeaturesBit
                                                     .InOrderData)
                                                      ? "In-order data delivery is supported and enabled"
                                                      : "In-order data delivery is supported");
                    }

                    switch(atapi)
                    {
                        case false:
                        {
                            if(ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.HardwareFeatureControl))
                            {
                                commandSetAndFeatures.Add(ataIdentify.EnabledSATAFeatures.HasFlag(Identify
                                                             .SATAFeaturesBit.HardwareFeatureControl)
                                                              ? "Hardware Feature Control is supported and enabled"
                                                              : "Hardware Feature Control is supported");
                            }

                            break;
                        }
                        case true:
                        {
                            if(ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.AsyncNotification))
                            {
                                commandSetAndFeatures.Add(ataIdentify.EnabledSATAFeatures.HasFlag(Identify
                                                             .SATAFeaturesBit.AsyncNotification)
                                                              ? "Asynchronous notification is supported and enabled"
                                                              : "Asynchronous notification is supported");
                            }

                            break;
                        }
                    }

                    if(ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.SettingsPreserve))
                    {
                        commandSetAndFeatures.Add(ataIdentify.EnabledSATAFeatures.HasFlag(Identify.SATAFeaturesBit
                                                     .SettingsPreserve)
                                                      ? "Software Settings Preservation is supported and enabled"
                                                      : "Software Settings Preservation is supported");
                    }

                    if(ataIdentify.SATAFeatures.HasFlag(Identify.SATAFeaturesBit.NCQAutoSense))
                        commandSetAndFeatures.Add("NCQ Autosense is supported");

                    if(ataIdentify.EnabledSATAFeatures.HasFlag(Identify.SATAFeaturesBit.EnabledSlumber))
                        commandSetAndFeatures.Add("Automatic Partial to Slumber transitions are enabled");
                }
            }
        }

        if((ataIdentify.RemovableStatusSet & 0x03) > 0)
            commandSetAndFeatures.Add("Removable Media Status Notification feature set is supported");

        if(ataIdentify.FreeFallSensitivity != 0x00 && ataIdentify.FreeFallSensitivity != 0xFF)
        {
            specificConfiguration ??= [];
            specificConfiguration.Add($"Free-fall sensitivity set to {ataIdentify.FreeFallSensitivity}");
        }

        if(ataIdentify.DataSetMgmt.HasFlag(Identify.DataSetMgmtBit.Trim))
            commandSetAndFeatures.Add("TRIM is supported");

        if(ataIdentify.DataSetMgmtSize > 0)
        {
            commandSetAndFeatures
               .Add($"DATA SET MANAGEMENT can receive a maximum of {ataIdentify.DataSetMgmtSize} blocks of 512 bytes");
        }

        if(ataIdentify.SecurityStatus.HasFlag(Identify.SecurityStatusBit.Supported))
        {
            security = [];

            if(ataIdentify.SecurityStatus.HasFlag(Identify.SecurityStatusBit.Enabled))
            {
                security.Add("Security is enabled");

                security.Add(ataIdentify.SecurityStatus.HasFlag(Identify.SecurityStatusBit.Locked)
                                 ? "Security is locked"
                                 : "Security is not locked");

                security.Add(ataIdentify.SecurityStatus.HasFlag(Identify.SecurityStatusBit.Frozen)
                                 ? "Security is frozen"
                                 : "Security is not frozen");

                security.Add(ataIdentify.SecurityStatus.HasFlag(Identify.SecurityStatusBit.Expired)
                                 ? "Security count has expired"
                                 : "Security count has not expired");

                security.Add(ataIdentify.SecurityStatus.HasFlag(Identify.SecurityStatusBit.Maximum)
                                 ? "Security level is maximum"
                                 : "Security level is high");
            }
            else
                security.Add("Security is not enabled");

            if(ataIdentify.SecurityStatus.HasFlag(Identify.SecurityStatusBit.Enhanced))
                security.Add("Supports enhanced security erase");

            security.Add($"{ataIdentify.SecurityEraseTime * 2} minutes to complete secure erase");

            if(ataIdentify.SecurityStatus.HasFlag(Identify.SecurityStatusBit.Enhanced))
                security.Add($"{ataIdentify.EnhancedSecurityEraseTime * 2} minutes to complete enhanced secure erase");

            security.Add($"Master password revision code: {ataIdentify.MasterPasswordRevisionCode}");
        }

        if(ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.MustBeSet)    &&
           !ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.MustBeClear) &&
           ataIdentify.CommandSet3.HasFlag(Identify.CommandSetBit3.Streaming))
        {
            streaming =
            [
                $"Minimum request size is {ataIdentify.StreamMinReqSize}",
                $"Streaming transfer time in PIO is {ataIdentify.StreamTransferTimePIO}",
                $"Streaming transfer time in DMA is {ataIdentify.StreamTransferTimeDMA}",
                $"Streaming access latency is {ataIdentify.StreamAccessLatency}",
                $"Streaming performance granularity is {ataIdentify.StreamPerformanceGranularity}"
            ];
        }

        if(ataIdentify.SCTCommandTransport.HasFlag(Identify.SCTCommandTransportBit.Supported))
        {
            smartCommandTransport = [];

            if(ataIdentify.SCTCommandTransport.HasFlag(Identify.SCTCommandTransportBit.LongSectorAccess))
                smartCommandTransport.Add("SCT Long Sector Address is supported");

            if(ataIdentify.SCTCommandTransport.HasFlag(Identify.SCTCommandTransportBit.WriteSame))
                smartCommandTransport.Add("SCT Write Same is supported");

            if(ataIdentify.SCTCommandTransport.HasFlag(Identify.SCTCommandTransportBit.ErrorRecoveryControl))
                smartCommandTransport.Add("SCT Error Recovery Control is supported");

            if(ataIdentify.SCTCommandTransport.HasFlag(Identify.SCTCommandTransportBit.FeaturesControl))
                smartCommandTransport.Add("SCT Features Control is supported");

            if(ataIdentify.SCTCommandTransport.HasFlag(Identify.SCTCommandTransportBit.DataTables))
                smartCommandTransport.Add("SCT Data Tables are supported");
        }

        if((ataIdentify.NVCacheCaps & 0x0010) == 0x0010)
        {
            nvCache = [$"Version {(ataIdentify.NVCacheCaps & 0xF000) >> 12}"];

            if((ataIdentify.NVCacheCaps & 0x0001) == 0x0001)
            {
                nvCache.Add((ataIdentify.NVCacheCaps & 0x0002) == 0x0002
                                ? "Power mode feature set is supported and enabled"
                                : "Power mode feature set is supported");

                nvCache.Add($"Version {(ataIdentify.NVCacheCaps & 0x0F00) >> 8}");
            }

            nvCache.Add($"Non-Volatile Cache is {ataIdentify.NVCacheSize * logicalSectorSize} bytes");
        }

        if(ataReport.ReadCapabilities != null)
        {
            removable = false;

            if(ataReport.ReadCapabilities.NominalRotationRate != null   &&
               ataReport.ReadCapabilities.NominalRotationRate != 0x0000 &&
               ataReport.ReadCapabilities.NominalRotationRate != 0xFFFF)
            {
                readCapabilitiesList.Add(ataReport.ReadCapabilities.NominalRotationRate == 0x0001
                                             ? "Device does not rotate."
                                             : $"Device rotates at {ataReport.ReadCapabilities.NominalRotationRate} rpm");
            }

            if(!atapi)
            {
                if(ataReport.ReadCapabilities.BlockSize != null)
                {
                    readCapabilitiesDictionary.Add("Logical sector size",
                                                   $"{ataReport.ReadCapabilities.BlockSize} bytes");

                    logicalSectorSize = ataReport.ReadCapabilities.BlockSize.Value;
                }

                if(ataReport.ReadCapabilities.PhysicalBlockSize != null)
                {
                    readCapabilitiesDictionary.Add("Physical sector size",
                                                   $"{ataReport.ReadCapabilities.PhysicalBlockSize} bytes");
                }

                if(ataReport.ReadCapabilities.LongBlockSize != null)
                {
                    readCapabilitiesDictionary.Add("READ LONG sector size",
                                                   $"{ataReport.ReadCapabilities.LongBlockSize} bytes");
                }

                if(ataReport.ReadCapabilities.BlockSize != null &&
                   ataReport.ReadCapabilities.PhysicalBlockSize != null &&
                   ataReport.ReadCapabilities.BlockSize.Value != ataReport.ReadCapabilities.PhysicalBlockSize.Value &&
                   (ataReport.ReadCapabilities.LogicalAlignment & 0x8000) == 0x0000 &&
                   (ataReport.ReadCapabilities.LogicalAlignment & 0x4000) == 0x4000)
                {
                    readCapabilitiesList
                       .Add($"Logical sector starts at offset {ataReport.ReadCapabilities.LogicalAlignment & 0x3FFF} from physical sector");
                }

                if(ataReport.ReadCapabilities.CHS != null && ataReport.ReadCapabilities.CurrentCHS != null)
                {
                    int currentSectors = ataReport.ReadCapabilities.CurrentCHS.Cylinders *
                                         ataReport.ReadCapabilities.CurrentCHS.Heads     *
                                         ataReport.ReadCapabilities.CurrentCHS.Sectors;

                    readCapabilitiesDictionary.Add("Cylinders",
                                                   $"{ataReport.ReadCapabilities.CHS.Cylinders} max., {ataReport.ReadCapabilities.CurrentCHS.Cylinders} current");

                    readCapabilitiesDictionary.Add("Heads",
                                                   $"{ataReport.ReadCapabilities.CHS.Heads} max., {ataReport.ReadCapabilities.CurrentCHS.Heads} current");

                    readCapabilitiesDictionary.Add("Sectors per track",
                                                   $"{ataReport.ReadCapabilities.CHS.Sectors} max., {ataReport.ReadCapabilities.CurrentCHS.Sectors} current");

                    readCapabilitiesDictionary.Add("Sectors addressable in CHS mode",
                                                   $"{ataReport.ReadCapabilities.CHS.Cylinders * ataReport.ReadCapabilities.CHS.Heads * ataReport.ReadCapabilities.CHS.Sectors} max., {currentSectors} current");

                    readCapabilitiesDictionary.Add("Device size in CHS mode",
                                                   $"{(ulong)currentSectors * logicalSectorSize} bytes, {(ulong)currentSectors * logicalSectorSize / 1000 / 1000} Mb, {(double)((ulong)currentSectors * logicalSectorSize) / 1024 / 1024:F2} MiB");
                }
                else if(ataReport.ReadCapabilities.CHS != null)
                {
                    int currentSectors = ataReport.ReadCapabilities.CHS.Cylinders *
                                         ataReport.ReadCapabilities.CHS.Heads     *
                                         ataReport.ReadCapabilities.CHS.Sectors;

                    readCapabilitiesDictionary.Add("Cylinders", $"{ataReport.ReadCapabilities.CHS.Cylinders}");
                    readCapabilitiesDictionary.Add("Heads", $"{ataReport.ReadCapabilities.CHS.Heads}");
                    readCapabilitiesDictionary.Add("Sectors per track", $"{ataReport.ReadCapabilities.CHS.Sectors}");
                    readCapabilitiesDictionary.Add("Sectors addressable in CHS mode", $"{currentSectors}");

                    readCapabilitiesDictionary.Add("Device size in CHS mode",
                                                   $"{(ulong)currentSectors * logicalSectorSize} bytes, {(ulong)currentSectors * logicalSectorSize / 1000 / 1000} Mb, {(double)((ulong)currentSectors * logicalSectorSize) / 1024 / 1024:F2} MiB");
                }

                if(ataReport.ReadCapabilities.LBASectors != null)
                {
                    readCapabilitiesDictionary.Add("Sectors addressable in sectors in 28-bit LBA mode",
                                                   $"{ataReport.ReadCapabilities.LBASectors}");

                    switch((ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize / 1024 / 1024)
                    {
                        case > 1000000:
                            readCapabilitiesDictionary.Add("Device size in 28-bit LBA mode",
                                                           $"{(ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize} bytes, {(ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize / 1000 / 1000 / 1000 / 1000} Tb, {(double)((ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize) / 1024 / 1024 / 1024 / 1024:F2} TiB");

                            break;
                        case > 1000:
                            readCapabilitiesDictionary.Add("Device size in 28-bit LBA mode",
                                                           $"{(ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize} bytes, {(ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize / 1000 / 1000 / 1000} Gb, {(double)((ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize) / 1024 / 1024 / 1024:F2} GiB");

                            break;
                        default:
                            readCapabilitiesDictionary.Add("Device size in 28-bit LBA mode",
                                                           $"{(ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize} bytes, {(ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize / 1000 / 1000} Mb, {(double)((ulong)ataReport.ReadCapabilities.LBASectors * logicalSectorSize) / 1024 / 1024:F2} MiB");

                            break;
                    }
                }

                if(ataReport.ReadCapabilities.LBA48Sectors != null)
                {
                    readCapabilitiesDictionary.Add("Sectors addressable in sectors in 48-bit LBA mode",
                                                   $"{ataReport.ReadCapabilities.LBA48Sectors}");

                    switch(ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize / 1024 / 1024)
                    {
                        case > 1000000:
                            readCapabilitiesDictionary.Add("Device size in 48-bit LBA mode",
                                                           $"{ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize} bytes, {ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize / 1000 / 1000 / 1000 / 1000} Tb, {(double)(ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize) / 1024 / 1024 / 1024 / 1024:F2} TiB");

                            break;
                        case > 1000:
                            readCapabilitiesDictionary.Add("Device size in 48-bit LBA mode",
                                                           $"{ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize} bytes, {ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize / 1000 / 1000 / 1000} Gb, {(double)(ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize) / 1024 / 1024 / 1024:F2} GiB");

                            break;
                        default:
                            readCapabilitiesDictionary.Add("Device size in 48-bit LBA mode",
                                                           $"{ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize} bytes, {ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize / 1000 / 1000} Mb, {(double)(ataReport.ReadCapabilities.LBA48Sectors * logicalSectorSize) / 1024 / 1024:F2} MiB");

                            break;
                    }
                }

                if(ata1 || cfa)
                {
                    if(ataReport.ReadCapabilities.UnformattedBPT > 0)
                    {
                        readCapabilitiesDictionary.Add("Bytes per unformatted track",
                                                       $"{ataReport.ReadCapabilities.UnformattedBPT}");
                    }

                    if(ataReport.ReadCapabilities.UnformattedBPS > 0)
                    {
                        readCapabilitiesDictionary.Add("Bytes per unformatted sector",
                                                       $"{ataReport.ReadCapabilities.UnformattedBPS}");
                    }
                }
            }

            if(ataReport.ReadCapabilities.SupportsReadSectors == true)
                readCapabilitiesList.Add("Device supports READ SECTOR(S) command in CHS mode");

            if(ataReport.ReadCapabilities.SupportsReadRetry == true)
                readCapabilitiesList.Add("Device supports READ SECTOR(S) RETRY command in CHS mode");

            if(ataReport.ReadCapabilities.SupportsReadDma == true)
                readCapabilitiesList.Add("Device supports READ DMA command in CHS mode");

            if(ataReport.ReadCapabilities.SupportsReadDmaRetry == true)
                readCapabilitiesList.Add("Device supports READ DMA RETRY command in CHS mode");

            if(ataReport.ReadCapabilities.SupportsReadLong == true)
                readCapabilitiesList.Add("Device supports READ LONG command in CHS mode");

            if(ataReport.ReadCapabilities.SupportsReadLongRetry == true)
                readCapabilitiesList.Add("Device supports READ LONG RETRY command in CHS mode");

            if(ataReport.ReadCapabilities.SupportsReadLba == true)
                readCapabilitiesList.Add("Device supports READ SECTOR(S) command in 28-bit LBA mode");

            if(ataReport.ReadCapabilities.SupportsReadRetryLba == true)
                readCapabilitiesList.Add("Device supports READ SECTOR(S) RETRY command in 28-bit LBA mode");

            if(ataReport.ReadCapabilities.SupportsReadDmaLba == true)
                readCapabilitiesList.Add("Device supports READ DMA command in 28-bit LBA mode");

            if(ataReport.ReadCapabilities.SupportsReadDmaRetryLba == true)
                readCapabilitiesList.Add("Device supports READ DMA RETRY command in 28-bit LBA mode");

            if(ataReport.ReadCapabilities.SupportsReadLongLba == true)
                readCapabilitiesList.Add("Device supports READ LONG command in 28-bit LBA mode");

            if(ataReport.ReadCapabilities.SupportsReadLongRetryLba == true)
                readCapabilitiesList.Add("Device supports READ LONG RETRY command in 28-bit LBA mode");

            if(ataReport.ReadCapabilities.SupportsReadLba48 == true)
                readCapabilitiesList.Add("Device supports READ SECTOR(S) command in 48-bit LBA mode");

            if(ataReport.ReadCapabilities.SupportsReadDmaLba48 == true)
                readCapabilitiesList.Add("Device supports READ DMA command in 48-bit LBA mode");

            if(ataReport.ReadCapabilities.SupportsSeek == true)
                readCapabilitiesList.Add("Device supports SEEK command in CHS mode");

            if(ataReport.ReadCapabilities.SupportsSeekLba == true)
                readCapabilitiesList.Add("Device supports SEEK command in 28-bit LBA mode");
        }
        else
            testedMedia = ataReport.RemovableMedias;
    }
}