// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : ScsiModeSense.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Decodes SCSI MODE PAGEs from reports.
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

using Aaru.CommonTypes.Metadata;
using Aaru.CommonTypes.Structs.Devices.SCSI;
using Aaru.Decoders.SCSI;

namespace Aaru.Server.Core;

public static class ScsiModeSense
{
    /// <summary>
    ///     Takes the MODE PAGEs part of a device report and prints it as a list of values and another list of key=value
    ///     pairs to be sequenced by ASP.NET in the rendering
    /// </summary>
    /// <param name="modeSense">MODE PAGEs part of a device report</param>
    /// <param name="vendor">SCSI vendor string</param>
    /// <param name="deviceType">SCSI peripheral device type</param>
    /// <param name="scsiOneValue">List to put values on</param>
    /// <param name="modePages">List to put key=value pairs on</param>
    public static void Report(ScsiMode modeSense, string vendor, PeripheralDeviceTypes deviceType,
                              out List<string>? modeSenseCapabilities, out List<string>? blockDescriptors,
                              out Dictionary<string, List<string>> modePages)
    {
        modeSenseCapabilities = null;
        blockDescriptors      = null;
        modePages             = [];

        if(modeSense.MediumType.HasValue)
        {
            modeSenseCapabilities ??= [];
            modeSenseCapabilities.Add($"Medium type is {modeSense.MediumType:X2}h");
        }

        if(modeSense.WriteProtected)
        {
            modeSenseCapabilities ??= [];
            modeSenseCapabilities.Add("Device is write protected.");
        }

        if(modeSense.BlockDescriptors?.Count > 0)
        {
            blockDescriptors ??= [];

            foreach(BlockDescriptor descriptor in modeSense.BlockDescriptors)
            {
                if(descriptor.Blocks.HasValue && descriptor.BlockLength.HasValue)
                {
                    blockDescriptors
                       .Add($"Density code {descriptor.Density:X2}h has {descriptor.Blocks} blocks of {descriptor.BlockLength} bytes each");
                }
                else
                    blockDescriptors.Add($"Density code {descriptor.Density:X2}h");
            }
        }

        if(modeSense.DPOandFUA)
        {
            modeSenseCapabilities ??= [];
            modeSenseCapabilities.Add("Drive supports DPO and FUA bits");
        }

        if(modeSense.BlankCheckEnabled)
        {
            modeSenseCapabilities ??= [];
            modeSenseCapabilities.Add("Blank checking during write is enabled");
        }

        if(modeSense.BufferedMode.HasValue)
        {
            modeSenseCapabilities ??= [];

            switch(modeSense.BufferedMode)
            {
                case 0:
                    modeSenseCapabilities.Add("Device writes directly to media");

                    break;
                case 1:
                    modeSenseCapabilities.Add("Device uses a write cache");

                    break;
                case 2:
                    modeSenseCapabilities.Add("Device uses a write cache but doesn't return until cache is flushed");

                    break;
                default:
                    modeSenseCapabilities.Add($"Unknown buffered mode code 0x{modeSense.BufferedMode:X2}");

                    break;
            }
        }

        if(modeSense.ModePages == null) return;

        foreach(ScsiPage page in modeSense.ModePages)
        {
            switch(page.page)
            {
                case 0x00:
                {
                    if(deviceType == PeripheralDeviceTypes.MultiMediaDevice && page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_00_SFF(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add($"MODE page {page.page:X2}h",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                    {
                        modePages.Add(page.subpage != 0
                                          ? $"MODE page {page.page:X2}h subpage {page.subpage:X2}h"
                                          : $"MODE page {page.page:X2}h",
                                      ["Unknown vendor mode page"]);
                    }

                    break;
                }
                case 0x01:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = deviceType == PeripheralDeviceTypes.MultiMediaDevice
                                            ? Modes.PrettifyModePage_01_MMC(page.value)
                                            : Modes.PrettifyModePage_01(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add(deviceType == PeripheralDeviceTypes.MultiMediaDevice
                                              ? "MODE page 01h: Read error recovery page for MultiMedia Devices"
                                              : "MODE page 01h: Read-write error recovery page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x02:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_02(page.value);

                        if(pretty is not null)

                        {
                            modePages.Add("MODE page 02h: Disconnect-Reconnect page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x03:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_03(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 03h: Format device page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x04:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_04(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 04h: Rigid disk drive geometry page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x05:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_05(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 05h: Flexible disk page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x06:
                {
                    if(page.subpage == 0)

                    {
                        string pretty = Modes.PrettifyModePage_06(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 06h: Optical memory page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x07:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = deviceType == PeripheralDeviceTypes.MultiMediaDevice
                                            ? Modes.PrettifyModePage_07_MMC(page.value)
                                            : Modes.PrettifyModePage_07(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add(deviceType == PeripheralDeviceTypes.MultiMediaDevice
                                              ? "MODE page 07h: Verify error recovery page for MultiMedia Devices"
                                              : "MODE page 07h: Verify error recovery page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x08:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_08(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add($"MODE page {page.page:X2}h",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x0A:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_0A(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 0Ah: Control mode page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else if(page.subpage == 1)
                    {
                        string pretty = Modes.PrettifyModePage_0A_S01(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 0Ah subpage 01h: Control extension page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x0B:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_0B(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 0Bh: Medium types supported page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x0D:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_0D(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 0Dh: CD-ROM parameters page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x0E:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_0E(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 0Eh: CD-ROM audio control parameters page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x0F:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_0F(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 0Fh: Data compression page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x10:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = deviceType == PeripheralDeviceTypes.SequentialAccess
                                            ? Modes.PrettifyModePage_10_SSC(page.value)
                                            : Modes.PrettifyModePage_10(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add(deviceType == PeripheralDeviceTypes.SequentialAccess
                                              ? "MODE page 10h: Device configuration page"
                                              : "MODE page 10h: XOR control mode page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x11:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_11(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 11h: Medium partition page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x12:
                case 0x13:
                case 0x14:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_12_13_14(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add($"MODE page {page.page:X2}h: Medium partition page (extra)",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x1A:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_1A(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 1Ah: Power condition page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else if(page.subpage == 1)
                    {
                        string pretty = Modes.PrettifyModePage_1A_S01(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 1Ah subpage 01h: Power Consumption page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x1B:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_1B(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 1Bh: Removable Block Access Capabilities page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x1C:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = deviceType == PeripheralDeviceTypes.MultiMediaDevice
                                            ? Modes.PrettifyModePage_1C_SFF(page.value)
                                            : Modes.PrettifyModePage_1C(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add(deviceType == PeripheralDeviceTypes.MultiMediaDevice
                                              ? "MODE page 1Ch: Timer & Protect page"
                                              : "MODE page 1Ch: Informational exceptions control page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else if(page.subpage == 1)
                    {
                        string pretty = Modes.PrettifyModePage_1C_S01(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 1Ch subpage 01h: Background Control page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x1D:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_1D(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 1Dh: Medium Configuration page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x21:
                {
                    if(vendor == "CERTANCE")
                    {
                        string pretty = Modes.PrettifyCertanceModePage_21(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 21h: Certance Drive Capabilities Control page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x22:
                {
                    if(vendor == "CERTANCE")
                    {
                        string pretty = Modes.PrettifyCertanceModePage_22(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 22h: Certance Interface Control page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x24:
                {
                    if(vendor == "IBM")
                    {
                        string pretty = Modes.PrettifyIBMModePage_24(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 24h: IBM Vendor-Specific Control page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x2A:
                {
                    if(page.subpage == 0)
                    {
                        string pretty = Modes.PrettifyModePage_2A(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 2Ah: CD-ROM capabilities page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x2F:
                {
                    if(vendor == "IBM")
                    {
                        string pretty = Modes.PrettifyIBMModePage_2F(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 2Fh: IBM Behaviour Configuration page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x30:
                {
                    if(Modes.IsAppleModePage_30(page.value))
                        modePages.Add("MODE page 30h", ["Drive identifies as an Apple OEM drive"]);
                    else
                        goto default;

                    break;
                }
                case 0x3B:
                {
                    if(vendor == "HP")
                    {
                        string pretty = Modes.PrettifyHPModePage_3B(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 3Bh: HP Serial Number Override page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x3C:
                {
                    if(vendor == "HP")
                    {
                        string pretty = Modes.PrettifyHPModePage_3C(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 3Ch: HP Device Time page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x3D:
                {
                    if(vendor == "IBM")
                    {
                        string pretty = Modes.PrettifyIBMModePage_3D(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 3Dh: IBM LEOT page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else if(vendor == "HP")
                    {
                        string pretty = Modes.PrettifyHPModePage_3D(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 3Dh: HP Extended Reset page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                case 0x3E:
                {
                    if(vendor == "FUJITSU")
                    {
                        string pretty = Modes.PrettifyFujitsuModePage_3E(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 3Eh: Fujitsu Verify Control page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else if(vendor == "HP")
                    {
                        string pretty = Modes.PrettifyHPModePage_3E(page.value);

                        if(pretty is not null)
                        {
                            modePages.Add("MODE page 3Eh: HP CD-ROM Emulation/Disaster Recovery page",
                                          pretty.Replace("\t", "")
                                                .Split(Environment.NewLine,
                                                       StringSplitOptions.TrimEntries |
                                                       StringSplitOptions.RemoveEmptyEntries)
                                                .Skip(1)
                                                .ToList());
                        }
                        else
                            goto default;
                    }
                    else
                        goto default;

                    break;
                }
                default:
                {
                    modePages.Add(page.subpage != 0
                                      ? $"MODE page {page.page:X2}h subpage {page.subpage:X2}h: Unknown page"
                                      : $"MODE page {page.page:X2}h: Unknown page",
                                  []);
                }

                    break;
            }
        }
    }
}