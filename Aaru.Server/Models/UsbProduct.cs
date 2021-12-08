// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : UsbProduct.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Model for storing USB product identifiers in database.
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
// Copyright © 2011-2021 Natalia Portillo
// ****************************************************************************/

using System;
using Newtonsoft.Json;

namespace Aaru.Server.Models;

public class UsbProduct : BaseModel<int>
{
    public UsbProduct() {}

    public UsbProduct(UsbVendor vendor, ushort id, string product)
    {
        ProductId = id;
        Product   = product;
        AddedWhen = ModifiedWhen = DateTime.UtcNow;
        Vendor    = vendor;
    }

    public ushort   ProductId    { get; set; }
    public string   Product      { get; set; }
    public DateTime AddedWhen    { get; set; }
    public DateTime ModifiedWhen { get; set; }
    public int      VendorId     { get; set; }

    [JsonIgnore]
    public virtual UsbVendor Vendor { get; set; }
}