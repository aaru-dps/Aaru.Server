// /***************************************************************************
// The Disc Image Chef
// ----------------------------------------------------------------------------
//
// Filename       : Context.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : DiscImageChef Server.
//
// --[ Description ] ----------------------------------------------------------
//
//     Entity framework database context.
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

using System.Data.Common;
using DiscImageChef.CommonTypes.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DiscImageChef.Server.Models
{
    public sealed class DicServerContext : IdentityDbContext<IdentityUser>
    {
        public DicServerContext() { }

        public DicServerContext(DbContextOptions<DicServerContext> options) : base(options) { }

        public DbSet<Device>                   Devices               { get; set; }
        public DbSet<UploadedReport>           Reports               { get; set; }
        public DbSet<Command>                  Commands              { get; set; }
        public DbSet<DeviceStat>               DeviceStats           { get; set; }
        public DbSet<Filesystem>               Filesystems           { get; set; }
        public DbSet<Filter>                   Filters               { get; set; }
        public DbSet<Media>                    Medias                { get; set; }
        public DbSet<MediaFormat>              MediaFormats          { get; set; }
        public DbSet<OperatingSystem>          OperatingSystems      { get; set; }
        public DbSet<Partition>                Partitions            { get; set; }
        public DbSet<Version>                  Versions              { get; set; }
        public DbSet<UsbVendor>                UsbVendors            { get; set; }
        public DbSet<UsbProduct>               UsbProducts           { get; set; }
        public DbSet<CompactDiscOffset>        CdOffsets             { get; set; }
        public DbSet<CommonTypes.Metadata.Ata> Ata                   { get; set; }
        public DbSet<BlockDescriptor>          BlockDescriptor       { get; set; }
        public DbSet<Chs>                      Chs                   { get; set; }
        public DbSet<FireWire>                 FireWire              { get; set; }
        public DbSet<Mmc>                      Mmc                   { get; set; }
        public DbSet<MmcSd>                    MmcSd                 { get; set; }
        public DbSet<MmcFeatures>              MmcFeatures           { get; set; }
        public DbSet<Pcmcia>                   Pcmcia                { get; set; }
        public DbSet<Scsi>                     Scsi                  { get; set; }
        public DbSet<ScsiMode>                 ScsiMode              { get; set; }
        public DbSet<ScsiPage>                 ScsiPage              { get; set; }
        public DbSet<Ssc>                      Ssc                   { get; set; }
        public DbSet<SupportedDensity>         SupportedDensity      { get; set; }
        public DbSet<TestedMedia>              TestedMedia           { get; set; }
        public DbSet<TestedSequentialMedia>    TestedSequentialMedia { get; set; }
        public DbSet<Usb>                      Usb                   { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured)
                return;

            IConfigurationBuilder builder       = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot    configuration = builder.Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.Ata", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.TestedMedia", "ReadCapabilities").WithMany().
                  HasForeignKey("ReadCapabilitiesId").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.BlockDescriptor", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.ScsiMode", null).WithMany("BlockDescriptors").
                  HasForeignKey("ScsiModeId").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.DensityCode", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.SscSupportedMedia", null).WithMany("DensityCodes").
                  HasForeignKey("SscSupportedMediaId").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.Mmc", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.MmcFeatures", "Features").WithMany().
                  HasForeignKey("FeaturesId").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.Scsi", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.ScsiMode", "ModeSense").WithMany().
                  HasForeignKey("ModeSenseId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Mmc", "MultiMediaDevice").WithMany().
                  HasForeignKey("MultiMediaDeviceId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.TestedMedia", "ReadCapabilities").WithMany().
                  HasForeignKey("ReadCapabilitiesId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ssc", "SequentialDevice").WithMany().
                  HasForeignKey("SequentialDeviceId").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.ScsiPage", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.Scsi", null).WithMany("EVPDPages").HasForeignKey("ScsiId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.ScsiMode", null).WithMany("ModePages").
                  HasForeignKey("ScsiModeId").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.SscSupportedMedia", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ssc", null).WithMany("SupportedMediaTypes").
                  HasForeignKey("SscId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.TestedSequentialMedia", null).
                  WithMany("SupportedMediaTypes").HasForeignKey("TestedSequentialMediaId").
                  OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.SupportedDensity", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ssc", null).WithMany("SupportedDensities").
                  HasForeignKey("SscId").OnDelete(DeleteBehavior.Cascade);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.TestedSequentialMedia", null).
                  WithMany("SupportedDensities").HasForeignKey("TestedSequentialMediaId").
                  OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.TestedMedia", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ata", null).WithMany("RemovableMedias").
                  HasForeignKey("AtaId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Chs", "CHS").WithMany().HasForeignKey("CHSId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Chs", "CurrentCHS").WithMany().
                  HasForeignKey("CurrentCHSId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Mmc", null).WithMany("TestedMedia").HasForeignKey("MmcId").
                  OnDelete(DeleteBehavior.Cascade);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Scsi", null).WithMany("RemovableMedias").
                  HasForeignKey("ScsiId").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.CommonTypes.Metadata.TestedSequentialMedia", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ssc", null).WithMany("TestedMedia").HasForeignKey("SscId").
                  OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.Server.Models.Device", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ata", "ATA").WithMany().HasForeignKey("ATAId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ata", "ATAPI").WithMany().HasForeignKey("ATAPIId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.Server.Models.CompactDiscOffset", "CdOffset").WithMany("Devices").
                  HasForeignKey("CdOffsetId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.FireWire", "FireWire").WithMany().
                  HasForeignKey("FireWireId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.MmcSd", "MultiMediaCard").WithMany().
                  HasForeignKey("MultiMediaCardId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Pcmcia", "PCMCIA").WithMany().HasForeignKey("PCMCIAId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Scsi", "SCSI").WithMany().HasForeignKey("SCSIId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.MmcSd", "SecureDigital").WithMany().
                  HasForeignKey("SecureDigitalId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Usb", "USB").WithMany().HasForeignKey("USBId").
                  OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.Server.Models.DeviceStat", b =>
            {
                b.HasOne("DiscImageChef.Server.Models.Device", "Report").WithMany().HasForeignKey("ReportId").
                  OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("DiscImageChef.Server.Models.UploadedReport", b =>
            {
                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ata", "ATA").WithMany().HasForeignKey("ATAId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Ata", "ATAPI").WithMany().HasForeignKey("ATAPIId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.FireWire", "FireWire").WithMany().
                  HasForeignKey("FireWireId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.MmcSd", "MultiMediaCard").WithMany().
                  HasForeignKey("MultiMediaCardId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Pcmcia", "PCMCIA").WithMany().HasForeignKey("PCMCIAId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Scsi", "SCSI").WithMany().HasForeignKey("SCSIId").
                  OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.MmcSd", "SecureDigital").WithMany().
                  HasForeignKey("SecureDigitalId").OnDelete(DeleteBehavior.SetNull);

                b.HasOne("DiscImageChef.CommonTypes.Metadata.Usb", "USB").WithMany().HasForeignKey("USBId").
                  OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CompactDiscOffset>().HasIndex(b => b.ModifiedWhen);

            modelBuilder.Entity<Device>().HasIndex(b => b.ModifiedWhen);

            modelBuilder.Entity<UsbProduct>().HasIndex(b => b.ModifiedWhen);
            modelBuilder.Entity<UsbProduct>().HasIndex(b => b.ProductId);
            modelBuilder.Entity<UsbProduct>().HasIndex(b => b.VendorId);

            modelBuilder.Entity<UsbVendor>().HasIndex(b => b.ModifiedWhen);
            modelBuilder.Entity<UsbVendor>().HasIndex(b => b.VendorId).IsUnique();
        }

        internal static bool TableExists(string tableName)
        {
            using var db = new DicServerContext();

            DbConnection connection = db.Database.GetDbConnection();
            connection.Open();

            DbCommand command = connection.CreateCommand();

            command.CommandText =
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME=\"{tableName}\"";

            long result = (long)command.ExecuteScalar();

            return result != 0;
        }
    }
}