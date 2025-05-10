// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : SscTestedMedia.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Decodes SCSI Streaming media tests from reports.
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

namespace Aaru.Server.Core;

public static class SscTestedMedia
{
    /// <summary>Takes the tested media from SCSI Streaming devices of a device report and prints it as a list of values</summary>
    /// <param name="mediaOneValue">List to put values on</param>
    /// <param name="testedMedia">List of tested media</param>
    public static void Report(IEnumerable<TestedSequentialMedia> testedMedia,
                              out Dictionary<string, (Dictionary<string, string> Table, List<string> List)>
                                  mediaInformation)
    {
        mediaInformation = [];

        foreach(TestedSequentialMedia media in testedMedia)
        {
            Dictionary<string, string> table = [];
            List<string>               list  = [];
            string                     header;

            if(!string.IsNullOrWhiteSpace(media.MediumTypeName))
            {
                header = $"Information for medium named \"{media.MediumTypeName}\"";

                if(media.MediumType.HasValue) table.Add("Medium type code", $"{media.MediumType:X2}h");
            }
            else if(media.MediumType.HasValue)
                header = $"Information for medium type {media.MediumType:X2}h";
            else
                header = "Information for unknown medium type";

            if(!string.IsNullOrWhiteSpace(media.Manufacturer))
                table.Add("Medium manufacturer", media.Manufacturer);

            if(!string.IsNullOrWhiteSpace(media.Model)) table.Add("Medium model", media.Model);

            if(media.Density.HasValue) list.Add($"Medium has density code {media.Density:X2}h");

            if(media.CanReadMediaSerial == true) list.Add("Drive can read medium serial number.");

            if(media.MediaIsRecognized) list.Add("Drive recognizes this medium.");

            mediaInformation.Add(header, (table, list));
        }
    }
}