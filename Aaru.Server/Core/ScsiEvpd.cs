// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : ScsiEvpd.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Decodes SCSI EVPDs from reports.
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
using Aaru.Decoders.SCSI;

namespace Aaru.Server.Core;

public static class ScsiEvpd
{
    /// <summary>
    ///     Takes the SCSI EVPD part of a device report and prints it as a list key=value pairs to be sequenced by ASP.NET
    ///     in the rendering
    /// </summary>
    /// <param name="pages">EVPD pages</param>
    /// <param name="vendor">SCSI vendor string</param>
    /// <param name="evpdPages">List to put the key=value pairs on</param>
    public static void Report(IEnumerable<ScsiPage>                pages, string vendor,
                              out Dictionary<string, List<string>> evpdPages)
    {
        evpdPages = new Dictionary<string, List<string>>();
        vendor    = vendor.ToLowerInvariant();

        foreach(ScsiPage evpd in pages)
        {
            string header = evpd.page switch
                            {
                                >= 0x01 and <= 0x7F => $"EVPD page {evpd.page:X2}h: ASCII information",
                                0x81                => "EVPD page 81h: Implemented operating definitions",
                                0x82                => "EVPD page 82h: ASCII implemented operating definition",
                                0x83                => "EVPD page 83h: Device identification",
                                0x84                => "EVPD page 84h: Software Interface Identifiers",
                                0x85                => "EVPD page 85h: Management Network Addresses",
                                0x86                => "EVPD page 86h: Extended INQUIRY Data",
                                0x89                => "EVPD page 89h: SCSI to ATA Translation Layer Data",
                                0xB0                => "EVPD page B0h: Sequential-access Device Capabilities",
                                0xB2                => "EVPD page B2h: TapeAlert Supported Flags Bitmap",
                                0xB4                => "EVPD page B4h: Unknown",
                                0xC0 when vendor == "quantum" =>
                                    "EVPD page C0h: Quantum Firmware Build Information page",
                                0xC0 when vendor == "seagate" => "EVPD page C0h: Seagate Firmware Numbers page",
                                0xC0 when vendor == "ibm" => "EVPD page C0h: IBM Drive Component Revision Levels page",
                                0xC1 when vendor == "ibm" => "EVPD page C1h: IBM Drive Serial Numbers page",
                                0xC0 or 0xC1 when vendor == "certance" =>
                                    $"EVPD page {evpd.page:X2}h: Certance Drive Component Revision Levels page",
                                0xC2 or 0xC3 or 0xC4 or 0xC5 or 0xC6 when vendor == "certance" =>
                                    $"EVPD page {evpd.page:X2}h: Certance Drive Component Serial Number page",
                                0xC0 when vendor == "hp" => "EVPD page C0h: HP Drive Firmware Revision Levels page:",
                                0xC1 when vendor == "hp" => "EVPD page C1h: HP Drive Hardware Revision Levels page",
                                0xC2 when vendor == "hp" => "EVPD page C2h: HP Drive PCA Revision Levels page",
                                0xC3 when vendor == "hp" => "EVPD page C3h: HP Drive Mechanism Revision Levels page",
                                0xC4 when vendor == "hp" =>
                                    "EVPD page C4h: HP Drive Head Assembly Revision Levels page",
                                0xC5 when vendor == "hp"       => "EVPD page C5h: HP Drive ACI Revision Levels page",
                                0xDF when vendor == "certance" => "EVPD page DFh: Certance drive status page",
                                _                              => $"EVPD page {evpd.page:X2}h: Undecoded"
                            };

            List<string> decoded;
            string?      ascii;

            switch(evpd.page)
            {
                case >= 0x01 and <= 0x7F:
                    ascii = EVPD.DecodeASCIIPage(evpd.value);

                    if(ascii is not null)
                        decoded = [ascii];
                    else
                        decoded = [];

                    break;
                case 0x81:
                    ascii = EVPD.PrettifyPage_81(evpd.value);

                    if(ascii is not null)
                        decoded = [ascii];
                    else
                        decoded = [];

                    break;
                case 0x82:
                    ascii = EVPD.DecodePage82(evpd.value);

                    if(ascii is not null)
                        decoded = [ascii];
                    else
                        decoded = [];

                    break;
                case 0x83:
                    decoded = EVPD.PrettifyPage_83(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0x84:
                    decoded = EVPD.PrettifyPage_84(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0x85:
                    decoded = EVPD.PrettifyPage_85(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0x86:
                    decoded = EVPD.PrettifyPage_86(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0x89:
                    decoded = EVPD.PrettifyPage_89(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xB0:
                    decoded = EVPD.PrettifyPage_B0(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xB2:
                    decoded = [$"0x{EVPD.DecodePageB2(evpd.value):X16}"];

                    break;
                case 0xB4:
                    ascii = EVPD.DecodePageB4(evpd.value);

                    if(ascii is not null)
                        decoded = [ascii];
                    else
                        decoded = [];

                    break;
                case 0xC0 when vendor == "quantum":
                    decoded = EVPD.PrettifyPage_C0_Quantum(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xC0 when vendor == "seagate":
                    decoded = EVPD.PrettifyPage_C0_Seagate(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xC0 when vendor == "ibm":
                    decoded = EVPD.PrettifyPage_C0_IBM(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xC1 when vendor == "ibm":
                    decoded = EVPD.PrettifyPage_C1_IBM(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xC0 or 0xC1 when vendor == "certance":
                    decoded = EVPD.PrettifyPage_C0_C1_Certance(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xC2 or 0xC3 or 0xC4 or 0xC5 or 0xC6 when vendor == "certance":
                    decoded = EVPD.PrettifyPage_C2_C3_C4_C5_C6_Certance(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xC0 or 0xC1 or 0xC2 or 0xC3 or 0xC4 or 0xC5 when vendor == "hp":
                    decoded = EVPD.PrettifyPage_C0_to_C5_HP(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                case 0xDF when vendor == "certance":
                    decoded = EVPD.PrettifyPage_DF_Certance(evpd.value)
                                 ?.Replace("\t", "")
                                  .Split(Environment.NewLine,
                                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .ToList() ?? [];

                    break;
                default:
                    decoded = [];

                    break;
            }

            evpdPages.Add(header, decoded);
        }
    }
}