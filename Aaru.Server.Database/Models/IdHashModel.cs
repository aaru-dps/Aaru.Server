// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : IdHashModel.cs
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

using Aaru.CommonTypes.Structs.Devices.SCSI;
using Aaru.Helpers;

namespace Aaru.Server.Database.Models;

public class IdHashModel : BaseModel<int>
{
    string?       _description;
    public string Hash { get; set; }
    public string? Description
    {
        get => _description ?? string.Join(' ', VendorIdentification, ProductIdentification, ProductRevisionLevel);
        set => _description = value;
    }
    public int[]    Duplicates            { get; set; }
    public string   VendorIdentification  => StringHandlers.CToString(Inquiry?.VendorIdentification);
    public string   ProductIdentification => StringHandlers.CToString(Inquiry?.ProductIdentification);
    public string   ProductRevisionLevel  => StringHandlers.CToString(Inquiry?.ProductRevisionLevel);
    public Inquiry? Inquiry               { get; set; }
}