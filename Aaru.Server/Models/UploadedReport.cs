// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : UploadedReport.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Model for storing uploaded device reports in database.
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
using System.ComponentModel;
using Aaru.CommonTypes.Metadata;

namespace Aaru.Server.Models
{
    public class UploadedReport : DeviceReportV2
    {
        public UploadedReport() => UploadedWhen = DateTime.UtcNow;

        public UploadedReport(DeviceReportV2 report)
        {
            ATA                       = report.ATA;
            ATAPI                     = report.ATAPI;
            CompactFlash              = report.CompactFlash;
            FireWire                  = report.FireWire;
            UploadedWhen              = DateTime.UtcNow;
            MultiMediaCard            = report.MultiMediaCard;
            PCMCIA                    = report.PCMCIA;
            SCSI                      = report.SCSI;
            SecureDigital             = report.SecureDigital;
            USB                       = report.USB;
            Manufacturer              = report.Manufacturer;
            Model                     = report.Model;
            Revision                  = report.Revision;
            Type                      = report.Type;
            GdRomSwapDiscCapabilities = report.GdRomSwapDiscCapabilities;
        }

        [DisplayName("Uploaded when")]
        public DateTime UploadedWhen { get; set; }

        public int? ATAId                       { get; set; }
        public int? ATAPIId                     { get; set; }
        public int? FireWireId                  { get; set; }
        public int? MultiMediaCardId            { get; set; }
        public int? PCMCIAId                    { get; set; }
        public int? SecureDigitalId             { get; set; }
        public int? SCSIId                      { get; set; }
        public int? USBId                       { get; set; }
        public int? GdRomSwapDiscCapabilitiesId { get; set; }
    }
}