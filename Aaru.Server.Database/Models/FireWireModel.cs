// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : FireWireModel.cs
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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aaru.Server.Database.Models;

public class FireWireModel
{
    [DisplayName("Vendor ID")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0x{0:X8}")]
    public uint VendorID { get; set; }

    [DisplayName("Product ID")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0x{0:X8}")]
    public uint ProductID { get; set; }

    [DisplayFormat(NullDisplayText = "Unknown")]
    public string Manufacturer { get; set; }

    [DisplayFormat(NullDisplayText = "Unknown")]
    public string Product { get; set; }

    [DisplayName("Is media removable?")]
    public bool RemovableMedia { get; set; }
}

public class FireWireModelForView
{
    public List<FireWireModel> List { get; set; }
    public string              Json { get; set; }
}