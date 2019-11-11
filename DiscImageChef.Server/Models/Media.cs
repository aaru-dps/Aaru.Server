// /***************************************************************************
// The Disc Image Chef
// ----------------------------------------------------------------------------
//
// Filename       : Media.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : DiscImageChef Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Model for storing media type statistics in database.
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
// Copyright © 2011-2019 Natalia Portillo
// ****************************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DiscImageChef.CommonTypes;

namespace DiscImageChef.Server.Models
{
    public class Media
    {
        [NotMapped]
        (string type, string subType) _mediaType;

        [Key]
        public int Id { get;       set; }
        public string Type  { get; set; }
        public bool   Real  { get; set; }
        public long   Count { get; set; }

        [NotMapped]
        (string type, string subType) MediaType
        {
            get
            {
                if(_mediaType != default)
                    return _mediaType;

                try
                {
                    if(Enum.TryParse(Type, out MediaType enumMediaType))
                        _mediaType = CommonTypes.Metadata.MediaType.MediaTypeToString(enumMediaType);
                    else if(int.TryParse(Type, out int asInt))
                        _mediaType = CommonTypes.Metadata.MediaType.MediaTypeToString((MediaType)asInt);
                }
                catch
                {
                    // Could not get media type/subtype pair from type, so just leave it as is
                }

                return _mediaType;
            }
        }

        [NotMapped]
        public string PhysicalType => MediaType.type;
        [NotMapped]
        public string LogicalType => MediaType.subType;
    }
}