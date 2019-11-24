// /***************************************************************************
// The Disc Image Chef
// ----------------------------------------------------------------------------
//
// Filename       : Device.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : DiscImageChef Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Model for storing processed device reports in database.
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
using System.ComponentModel;
using DiscImageChef.CommonTypes.Enums;
using DiscImageChef.CommonTypes.Metadata;

namespace DiscImageChef.Server.Models
{
    public class Device : DeviceReportV2
    {
        public Device() => AddedWhen = DateTime.UtcNow;

        public Device(DeviceReportV2 report)
        {
            ATA            = report.ATA;
            ATAPI          = report.ATAPI;
            CompactFlash   = report.CompactFlash;
            FireWire       = report.FireWire;
            AddedWhen      = DateTime.UtcNow;
            ModifiedWhen   = DateTime.UtcNow;
            MultiMediaCard = report.MultiMediaCard;
            PCMCIA         = report.PCMCIA;
            SCSI           = report.SCSI;
            SecureDigital  = report.SecureDigital;
            USB            = report.USB;
            Manufacturer   = report.Manufacturer;
            Model          = report.Model;
            Revision       = report.Revision;
            Type           = report.Type;
        }

        public Device(int? ataId, int? atapiId, int? firewireId, int? multimediacardId, int? pcmciaId,
                      int? securedigitalId, int? scsiId, int? usbId, DateTime uploadedWhen, string manufacturer,
                      string model, string revision, bool compactFlash, DeviceType type)
        {
            ATAId            = ataId;
            ATAPIId          = atapiId;
            FireWireId       = firewireId;
            MultiMediaCardId = multimediacardId;
            PCMCIAId         = pcmciaId;
            SecureDigitalId  = securedigitalId;
            SCSIId           = scsiId;
            USBId            = usbId;
            AddedWhen        = uploadedWhen;
            ModifiedWhen     = DateTime.UtcNow;
            Manufacturer     = manufacturer;
            Model            = model;
            Revision         = revision;
            CompactFlash     = compactFlash;
            Type             = type;
        }

        [DisplayName("Added when")]
        public DateTime AddedWhen { get; set; }
        [DisplayName("Modified when")]
        public DateTime? ModifiedWhen { get;             set; }
        public virtual CompactDiscOffset CdOffset { get; set; }

        [DefaultValue(0), DisplayName("Optimal no. of sectors to be read at once")]
        public int OptimalMultipleSectorsRead { get; set; }

        public int? ATAId            { get; set; }
        public int? ATAPIId          { get; set; }
        public int? FireWireId       { get; set; }
        public int? MultiMediaCardId { get; set; }
        public int? PCMCIAId         { get; set; }
        public int? SecureDigitalId  { get; set; }
        public int? SCSIId           { get; set; }
        public int? USBId            { get; set; }
    }
}