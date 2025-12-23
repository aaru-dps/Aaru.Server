// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : NesHeaderInfo.cs
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

using System.ComponentModel.DataAnnotations;
using Aaru.CommonTypes.Enums;

namespace Aaru.Server.Database.Models;

public class NesHeaderInfo : BaseModel<int>
{
    /// <summary>ROM hash</summary>
    [StringLength(64)]
    [Required]
    public string Sha256 { get; set; }

    /// <summary>If <c>true</c> vertical mirroring is hard-wired, horizontal or mapper defined otherwise</summary>
    public bool NametableMirroring { get; set; }

    /// <summary>If <c>true</c> a battery is present</summary>
    public bool BatteryPresent { get; set; }

    /// <summary>If <c>true</c> the four player screen mode is hardwired</summary>
    public bool FourScreenMode { get; set; }

    /// <summary>Mapper number (NES 2.0 when in conflict)</summary>
    public ushort Mapper { get; set; }

    /// <summary>Console type</summary>
    public NesConsoleType ConsoleType { get; set; }

    /// <summary>Submapper number</summary>
    public byte Submapper { get; set; }

    /// <summary>Timing mode</summary>
    public NesTimingMode TimingMode { get; set; }

    /// <summary>Vs. PPU type</summary>
    public NesVsPpuType VsPpuType { get; set; }

    /// <summary>Vs. hardware type</summary>
    public NesVsHardwareType VsHardwareType { get; set; }

    /// <summary>Extended console type</summary>
    public NesExtendedConsoleType ExtendedConsoleType { get; set; }

    /// <summary>Default expansion device</summary>
    public NesDefaultExpansionDevice DefaultExpansionDevice { get; set; }

    /// <summary>Date when model has been added to the database</summary>
    public DateTime AddedWhen { get; set; }

    /// <summary>Date when model was last modified</summary>
    public DateTime ModifiedWhen { get; set; }
}