using System;
using Aaru.Server.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aaru.Server.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Check for old tables
            bool AtasExists                   = AaruServerContext.TableExists("Atas");
            bool BlockDescriptorsExists       = AaruServerContext.TableExists("BlockDescriptors");
            bool ChsExists                    = AaruServerContext.TableExists("Chs");
            bool CommandsExists               = AaruServerContext.TableExists("Commands");
            bool CompactDiscOffsetsExists     = AaruServerContext.TableExists("CompactDiscOffsets");
            bool DensityCodesExists           = AaruServerContext.TableExists("DensityCodes");
            bool DevicesExists                = AaruServerContext.TableExists("Devices");
            bool DeviceStatsExists            = AaruServerContext.TableExists("DeviceStats");
            bool FilesystemsExists            = AaruServerContext.TableExists("Filesystems");
            bool FiltersExists                = AaruServerContext.TableExists("Filters");
            bool FireWiresExists              = AaruServerContext.TableExists("FireWires");
            bool MediaExists                  = AaruServerContext.TableExists("Media");
            bool MediaFormatsExists           = AaruServerContext.TableExists("MediaFormats");
            bool MmcFeaturesExists            = AaruServerContext.TableExists("MmcFeatures");
            bool MmcsExists                   = AaruServerContext.TableExists("Mmcs");
            bool MmcSdsExists                 = AaruServerContext.TableExists("MmcSds");
            bool OperatingSystemsExists       = AaruServerContext.TableExists("OperatingSystems");
            bool PartitionsExists             = AaruServerContext.TableExists("Partitions");
            bool PcmciasExists                = AaruServerContext.TableExists("Pcmcias");
            bool ScsiModesExists              = AaruServerContext.TableExists("ScsiModes");
            bool ScsiPagesExists              = AaruServerContext.TableExists("ScsiPages");
            bool ScsisExists                  = AaruServerContext.TableExists("Scsis");
            bool SscsExists                   = AaruServerContext.TableExists("Sscs");
            bool SscSupportedMediasExists     = AaruServerContext.TableExists("SscSupportedMedias");
            bool SupportedDensitiesExists     = AaruServerContext.TableExists("SupportedDensities");
            bool TestedMediasExists           = AaruServerContext.TableExists("TestedMedias");
            bool TestedSequentialMediasExists = AaruServerContext.TableExists("TestedSequentialMedias");
            bool UploadedReportsExists        = AaruServerContext.TableExists("UploadedReports");
            bool UsbProductsExists            = AaruServerContext.TableExists("UsbProducts");
            bool UsbsExists                   = AaruServerContext.TableExists("Usbs");
            bool UsbVendorsExists             = AaruServerContext.TableExists("UsbVendors");
            bool VersionsExists               = AaruServerContext.TableExists("Versions");
            bool EFExists                     = AaruServerContext.TableExists("__MigrationHistory");
            #endregion

            #region Drop old restrictions
            if(AtasExists)
                migrationBuilder.DropForeignKey("FK_Atas_TestedMedias_ReadCapabilities_Id", "Atas");

            if(BlockDescriptorsExists)
                migrationBuilder.DropForeignKey("FK_BlockDescriptors_ScsiModes_ScsiMode_Id", "BlockDescriptors");

            if(DensityCodesExists)
                migrationBuilder.DropForeignKey("FK_DensityCodes_SscSupportedMedias_SscSupportedMedia_Id",
                                                "DensityCodes");

            if(DevicesExists)
            {
                migrationBuilder.DropForeignKey("FK_Devices_Atas_ATA_Id", "Devices");
                migrationBuilder.DropForeignKey("FK_Devices_Atas_ATAPI_Id", "Devices");
                migrationBuilder.DropForeignKey("FK_Devices_CompactDiscOffsets_CdOffset_Id", "Devices");
                migrationBuilder.DropForeignKey("FK_Devices_FireWires_FireWire_Id", "Devices");
                migrationBuilder.DropForeignKey("FK_Devices_MmcSds_MultiMediaCard_Id", "Devices");
                migrationBuilder.DropForeignKey("FK_Devices_MmcSds_SecureDigital_Id", "Devices");
                migrationBuilder.DropForeignKey("FK_Devices_Pcmcias_PCMCIA_Id", "Devices");
                migrationBuilder.DropForeignKey("FK_Devices_Scsis_SCSI_Id", "Devices");
                migrationBuilder.DropForeignKey("FK_Devices_Usbs_USB_Id", "Devices");
            }

            if(DeviceStatsExists)
                migrationBuilder.DropForeignKey("FK_DeviceStats_Devices_Report_Id", "DeviceStats");

            if(MmcsExists)
                migrationBuilder.DropForeignKey("FK_Mmcs_MmcFeatures_Features_Id", "Mmcs");

            if(ScsiPagesExists)
            {
                migrationBuilder.DropForeignKey("FK_ScsiPages_ScsiModes_ScsiMode_Id", "ScsiPages");
                migrationBuilder.DropForeignKey("FK_ScsiPages_Scsis_Scsi_Id", "ScsiPages");
            }

            if(ScsisExists)
            {
                migrationBuilder.DropForeignKey("FK_Scsis_Mmcs_MultiMediaDevice_Id", "Scsis");
                migrationBuilder.DropForeignKey("FK_Scsis_ScsiModes_ModeSense_Id", "Scsis");
                migrationBuilder.DropForeignKey("FK_Scsis_Sscs_SequentialDevice_Id", "Scsis");
                migrationBuilder.DropForeignKey("FK_Scsis_TestedMedias_ReadCapabilities_Id", "Scsis");
            }

            if(SscSupportedMediasExists)
            {
                migrationBuilder.DropForeignKey("FK_a812ec60296b45bcb3d245a5c6d01d73", "SscSupportedMedias");
                migrationBuilder.DropForeignKey("FK_SscSupportedMedias_Sscs_Ssc_Id", "SscSupportedMedias");
            }

            if(SupportedDensitiesExists)
            {
                migrationBuilder.DropForeignKey("FK_783f1b3552774280af1caf44fb27e285", "SupportedDensities");
                migrationBuilder.DropForeignKey("FK_SupportedDensities_Sscs_Ssc_Id", "SupportedDensities");
            }

            if(TestedMediasExists)
            {
                migrationBuilder.DropForeignKey("FK_TestedMedias_Atas_Ata_Id", "TestedMedias");
                migrationBuilder.DropForeignKey("FK_TestedMedias_Chs_CHS_Id", "TestedMedias");
                migrationBuilder.DropForeignKey("FK_TestedMedias_Chs_CurrentCHS_Id", "TestedMedias");
                migrationBuilder.DropForeignKey("FK_TestedMedias_Mmcs_Mmc_Id", "TestedMedias");
                migrationBuilder.DropForeignKey("FK_TestedMedias_Scsis_Scsi_Id", "TestedMedias");
            }

            if(TestedSequentialMediasExists)
                migrationBuilder.DropForeignKey("FK_TestedSequentialMedias_Sscs_Ssc_Id", "TestedSequentialMedias");

            if(UploadedReportsExists)
            {
                migrationBuilder.DropForeignKey("FK_UploadedReports_Atas_ATA_Id", "UploadedReports");
                migrationBuilder.DropForeignKey("FK_UploadedReports_Atas_ATAPI_Id", "UploadedReports");
                migrationBuilder.DropForeignKey("FK_UploadedReports_FireWires_FireWire_Id", "UploadedReports");
                migrationBuilder.DropForeignKey("FK_UploadedReports_MmcSds_MultiMediaCard_Id", "UploadedReports");
                migrationBuilder.DropForeignKey("FK_UploadedReports_MmcSds_SecureDigital_Id", "UploadedReports");
                migrationBuilder.DropForeignKey("FK_UploadedReports_Pcmcias_PCMCIA_Id", "UploadedReports");
                migrationBuilder.DropForeignKey("FK_UploadedReports_Scsis_SCSI_Id", "UploadedReports");
                migrationBuilder.DropForeignKey("FK_UploadedReports_Usbs_USB_Id", "UploadedReports");
            }

            if(UsbProductsExists)
                migrationBuilder.DropForeignKey("FK_UsbProducts_UsbVendors_VendorId", "UsbProducts");
            #endregion

            #region TABLE: CdOffsets
            migrationBuilder.CreateTable("CdOffsets", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Manufacturer = table.Column<string>(nullable: true), Model = table.Column<string>(nullable: true),
                Offset       = table.Column<short>(), Submissions          = table.Column<int>(),
                Agreement    = table.Column<float>(),
                AddedWhen    = table.Column<DateTime>(), ModifiedWhen = table.Column<DateTime>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_CdOffsets", x => x.Id);
            });

            if(CompactDiscOffsetsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO CdOffsets (Id, Manufacturer, Model, Offset, Submissions, Agreement, AddedWhen, ModifiedWhen) SELECT Id, Manufacturer, Model, Offset, Submissions, Agreement, AddedWhen, ModifiedWhen FROM CompactDiscOffsets");

                migrationBuilder.DropTable("CompactDiscOffsets");
            }
            #endregion

            #region TABLE: Chs
            if(ChsExists)
                migrationBuilder.RenameTable("Chs", newName: "Chs_old");

            migrationBuilder.CreateTable("Chs", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Cylinders    = table.Column<ushort>(), Heads = table.Column<ushort>(),
                Sectors      = table.Column<ushort>(),
                CylindersSql = table.Column<short>(), HeadsSql = table.Column<short>(),
                SectorsSql   = table.Column<short>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Chs", x => x.Id);
            });

            if(ChsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Chs (Id, Cylinders, Heads, Sectors, CylindersSql, HeadsSql, SectorsSql) SELECT Id, CylindersSql AS Cylinders, HeadsSql AS Heads, SectorsSql AS Sectors, CylindersSql, HeadsSql, SectorsSql FROM Chs_old");

                migrationBuilder.DropTable("Chs_old");
            }
            #endregion

            #region TABLE: Commands
            if(CommandsExists)
                migrationBuilder.RenameTable("Commands", newName: "Commands_old");

            migrationBuilder.CreateTable("Commands", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(nullable: true), Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Commands", x => x.Id);
            });

            if(CommandsExists)
            {
                migrationBuilder.Sql("INSERT INTO Commands (Id, Name, Count) SELECT Id, Name, Count FROM Commands_old");
                migrationBuilder.DropTable("Commands_old");
            }
            #endregion

            #region TABLE: Filesystems
            if(FilesystemsExists)
                migrationBuilder.RenameTable("Filesystems", newName: "Filesystems_old");

            migrationBuilder.CreateTable("Filesystems", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(nullable: true), Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Filesystems", x => x.Id);
            });

            if(FilesystemsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Filesystems (Id, Name, Count) SELECT Id, Name, Count FROM Filesystems_old");

                migrationBuilder.DropTable("Filesystems_old");
            }
            #endregion

            #region TABLE: Filters
            if(FiltersExists)
                migrationBuilder.RenameTable("Filters", newName: "Filters_old");

            migrationBuilder.CreateTable("Filters", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(nullable: true), Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Filters", x => x.Id);
            });

            if(FiltersExists)
            {
                migrationBuilder.Sql("INSERT INTO Filters (Id, Name, Count) SELECT Id, Name, Count FROM Filters_old");
                migrationBuilder.DropTable("Filters_old");
            }
            #endregion

            #region TABLE: FireWire
            migrationBuilder.CreateTable("FireWire", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                VendorID       = table.Column<uint>(), ProductID               = table.Column<uint>(),
                Manufacturer   = table.Column<string>(nullable: true), Product = table.Column<string>(nullable: true),
                RemovableMedia = table.Column<bool>(), VendorIDSql             = table.Column<int>(),
                ProductIDSql   = table.Column<int>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_FireWire", x => x.Id);
            });

            if(FireWiresExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO FireWire (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, VendorIDSql, ProductIDSql) SELECT Id, VendorIDSql AS VendorID, ProductIDSql AS ProductID, Manufacturer, Product, RemovableMedia, VendorIDSql, ProductIDSql FROM FireWires");

                migrationBuilder.DropTable("FireWires");
            }
            #endregion

            #region TABLE: MediaFormats
            if(MediaFormatsExists)
                migrationBuilder.RenameTable("MediaFormats", newName: "MediaFormats_old");

            migrationBuilder.CreateTable("MediaFormats", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(nullable: true), Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_MediaFormats", x => x.Id);
            });

            if(MediaFormatsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO MediaFormats (Id, Name, Count) SELECT Id, Name, Count FROM MediaFormats_old");

                migrationBuilder.DropTable("MediaFormats_old");
            }
            #endregion

            #region TABLE: Medias
            migrationBuilder.CreateTable("Medias", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Type = table.Column<string>(nullable: true), Real = table.Column<bool>(), Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Medias", x => x.Id);
            });

            if(MediaExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Medias (`Id`, `Type`, `Real`, `Count`) SELECT `Id`, `Type`, `Real`, `Count` FROM Media");

                migrationBuilder.DropTable("Media");
            }
            #endregion

            #region TABLE: MmcFeatures
            if(MmcFeaturesExists)
                migrationBuilder.RenameTable("MmcFeatures", newName: "MmcFeatures_old");

            migrationBuilder.CreateTable("MmcFeatures", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                AACSVersion                        = table.Column<byte>(nullable: true),
                AGIDs                              = table.Column<byte>(nullable: true),
                BindingNonceBlocks                 = table.Column<byte>(nullable: true),
                BlocksPerReadableUnit              = table.Column<ushort>(nullable: true),
                BufferUnderrunFreeInDVD            = table.Column<bool>(),
                BufferUnderrunFreeInSAO            = table.Column<bool>(),
                BufferUnderrunFreeInTAO            = table.Column<bool>(),
                CanAudioScan                       = table.Column<bool>(),
                CanEject                           = table.Column<bool>(),
                CanEraseSector                     = table.Column<bool>(),
                CanExpandBDRESpareArea             = table.Column<bool>(),
                CanFormat                          = table.Column<bool>(),
                CanFormatBDREWithoutSpare          = table.Column<bool>(),
                CanFormatCert                      = table.Column<bool>(),
                CanFormatFRF                       = table.Column<bool>(),
                CanFormatQCert                     = table.Column<bool>(),
                CanFormatRRM                       = table.Column<bool>(),
                CanGenerateBindingNonce            = table.Column<bool>(),
                CanLoad                            = table.Column<bool>(),
                CanMuteSeparateChannels            = table.Column<bool>(),
                CanOverwriteSAOTrack               = table.Column<bool>(),
                CanOverwriteTAOTrack               = table.Column<bool>(),
                CanPlayCDAudio                     = table.Column<bool>(),
                CanPseudoOverwriteBDR              = table.Column<bool>(),
                CanReadAllDualR                    = table.Column<bool>(),
                CanReadAllDualRW                   = table.Column<bool>(),
                CanReadBD                          = table.Column<bool>(),
                CanReadBDR                         = table.Column<bool>(),
                CanReadBDRE1                       = table.Column<bool>(),
                CanReadBDRE2                       = table.Column<bool>(),
                CanReadBDROM                       = table.Column<bool>(),
                CanReadBluBCA                      = table.Column<bool>(),
                CanReadCD                          = table.Column<bool>(),
                CanReadCDMRW                       = table.Column<bool>(),
                CanReadCPRM_MKB                    = table.Column<bool>(),
                CanReadDDCD                        = table.Column<bool>(),
                CanReadDVD                         = table.Column<bool>(),
                CanReadDVDPlusMRW                  = table.Column<bool>(),
                CanReadDVDPlusR                    = table.Column<bool>(),
                CanReadDVDPlusRDL                  = table.Column<bool>(),
                CanReadDVDPlusRW                   = table.Column<bool>(),
                CanReadDVDPlusRWDL                 = table.Column<bool>(),
                CanReadDriveAACSCertificate        = table.Column<bool>(),
                CanReadHDDVD                       = table.Column<bool>(),
                CanReadHDDVDR                      = table.Column<bool>(),
                CanReadHDDVDRAM                    = table.Column<bool>(),
                CanReadLeadInCDText                = table.Column<bool>(),
                CanReadOldBDR                      = table.Column<bool>(),
                CanReadOldBDRE                     = table.Column<bool>(),
                CanReadOldBDROM                    = table.Column<bool>(),
                CanReadSpareAreaInformation        = table.Column<bool>(),
                CanReportDriveSerial               = table.Column<bool>(),
                CanReportMediaSerial               = table.Column<bool>(),
                CanTestWriteDDCDR                  = table.Column<bool>(),
                CanTestWriteDVD                    = table.Column<bool>(),
                CanTestWriteInSAO                  = table.Column<bool>(),
                CanTestWriteInTAO                  = table.Column<bool>(),
                CanUpgradeFirmware                 = table.Column<bool>(),
                CanWriteBD                         = table.Column<bool>(),
                CanWriteBDR                        = table.Column<bool>(),
                CanWriteBDRE1                      = table.Column<bool>(),
                CanWriteBDRE2                      = table.Column<bool>(),
                CanWriteBusEncryptedBlocks         = table.Column<bool>(),
                CanWriteCDMRW                      = table.Column<bool>(),
                CanWriteCDRW                       = table.Column<bool>(),
                CanWriteCDRWCAV                    = table.Column<bool>(),
                CanWriteCDSAO                      = table.Column<bool>(),
                CanWriteCDTAO                      = table.Column<bool>(),
                CanWriteCSSManagedDVD              = table.Column<bool>(),
                CanWriteDDCDR                      = table.Column<bool>(),
                CanWriteDDCDRW                     = table.Column<bool>(),
                CanWriteDVDPlusMRW                 = table.Column<bool>(),
                CanWriteDVDPlusR                   = table.Column<bool>(),
                CanWriteDVDPlusRDL                 = table.Column<bool>(),
                CanWriteDVDPlusRW                  = table.Column<bool>(),
                CanWriteDVDPlusRWDL                = table.Column<bool>(),
                CanWriteDVDR                       = table.Column<bool>(),
                CanWriteDVDRDL                     = table.Column<bool>(),
                CanWriteDVDRW                      = table.Column<bool>(),
                CanWriteHDDVDR                     = table.Column<bool>(),
                CanWriteHDDVDRAM                   = table.Column<bool>(),
                CanWriteOldBDR                     = table.Column<bool>(),
                CanWriteOldBDRE                    = table.Column<bool>(),
                CanWritePackedSubchannelInTAO      = table.Column<bool>(),
                CanWriteRWSubchannelInSAO          = table.Column<bool>(),
                CanWriteRWSubchannelInTAO          = table.Column<bool>(),
                CanWriteRaw                        = table.Column<bool>(),
                CanWriteRawMultiSession            = table.Column<bool>(),
                CanWriteRawSubchannelInTAO         = table.Column<bool>(),
                ChangerIsSideChangeCapable         = table.Column<bool>(),
                ChangerSlots                       = table.Column<byte>(),
                ChangerSupportsDiscPresent         = table.Column<bool>(),
                CPRMVersion                        = table.Column<byte>(nullable: true),
                CSSVersion                         = table.Column<byte>(nullable: true),
                DBML                               = table.Column<bool>(),
                DVDMultiRead                       = table.Column<bool>(),
                EmbeddedChanger                    = table.Column<bool>(),
                ErrorRecoveryPage                  = table.Column<bool>(),
                FirmwareDate                       = table.Column<DateTime>(nullable: true),
                LoadingMechanismType               = table.Column<byte>(nullable: true),
                Locked                             = table.Column<bool>(),
                LogicalBlockSize                   = table.Column<uint>(nullable: true),
                MultiRead                          = table.Column<bool>(),
                PhysicalInterfaceStandardNumber    = table.Column<uint>(nullable: true),
                PreventJumper                      = table.Column<bool>(),
                SupportsAACS                       = table.Column<bool>(),
                SupportsBusEncryption              = table.Column<bool>(),
                SupportsC2                         = table.Column<bool>(),
                SupportsCPRM                       = table.Column<bool>(),
                SupportsCSS                        = table.Column<bool>(),
                SupportsDAP                        = table.Column<bool>(),
                SupportsDeviceBusyEvent            = table.Column<bool>(),
                SupportsHybridDiscs                = table.Column<bool>(),
                SupportsModePage1Ch                = table.Column<bool>(),
                SupportsOSSC                       = table.Column<bool>(),
                SupportsPWP                        = table.Column<bool>(),
                SupportsSWPP                       = table.Column<bool>(),
                SupportsSecurDisc                  = table.Column<bool>(),
                SupportsSeparateVolume             = table.Column<bool>(),
                SupportsVCPS                       = table.Column<bool>(),
                SupportsWriteInhibitDCB            = table.Column<bool>(),
                SupportsWriteProtectPAC            = table.Column<bool>(),
                VolumeLevels                       = table.Column<ushort>(nullable: true),
                BinaryData                         = table.Column<byte[]>(nullable: true),
                BlocksPerReadableUnitSql           = table.Column<short>(nullable: true),
                LogicalBlockSizeSql                = table.Column<int>(nullable: true),
                PhysicalInterfaceStandardNumberSql = table.Column<int>(nullable: true),
                VolumeLevelsSql                    = table.Column<short>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_MmcFeatures", x => x.Id);
            });

            if(MmcFeaturesExists)
            {
                migrationBuilder.Sql(@"INSERT INTO MmcFeatures (AACSVersion,
AGIDs,
BindingNonceBlocks,
BlocksPerReadableUnit,
BufferUnderrunFreeInDVD,
BufferUnderrunFreeInSAO,
BufferUnderrunFreeInTAO,
CanAudioScan,
CanEject,
CanEraseSector,
CanExpandBDRESpareArea,
CanFormat,
CanFormatBDREWithoutSpare,
CanFormatCert,
CanFormatFRF,
CanFormatQCert,
CanFormatRRM,
CanGenerateBindingNonce,
CanLoad,
CanMuteSeparateChannels,
CanOverwriteSAOTrack,
CanOverwriteTAOTrack,
CanPlayCDAudio,
CanPseudoOverwriteBDR,
CanReadAllDualR,
CanReadAllDualRW,
CanReadBD,
CanReadBDR,
CanReadBDRE1,
CanReadBDRE2,
CanReadBDROM,
CanReadBluBCA,
CanReadCD,
CanReadCDMRW,
CanReadCPRM_MKB,
CanReadDDCD,
CanReadDVD,
CanReadDVDPlusMRW,
CanReadDVDPlusR,
CanReadDVDPlusRDL,
CanReadDVDPlusRW,
CanReadDVDPlusRWDL,
CanReadDriveAACSCertificate,
CanReadHDDVD,
CanReadHDDVDR,
CanReadHDDVDRAM,
CanReadLeadInCDText,
CanReadOldBDR,
CanReadOldBDRE,
CanReadOldBDROM,
CanReadSpareAreaInformation,
CanReportDriveSerial,
CanReportMediaSerial,
CanTestWriteDDCDR,
CanTestWriteDVD,
CanTestWriteInSAO,
CanTestWriteInTAO,
CanUpgradeFirmware,
CanWriteBD,
CanWriteBDR,
CanWriteBDRE1,
CanWriteBDRE2,
CanWriteBusEncryptedBlocks,
CanWriteCDMRW,
CanWriteCDRW,
CanWriteCDRWCAV,
CanWriteCDSAO,
CanWriteCDTAO,
CanWriteCSSManagedDVD,
CanWriteDDCDR,
CanWriteDDCDRW,
CanWriteDVDPlusMRW,
CanWriteDVDPlusR,
CanWriteDVDPlusRDL,
CanWriteDVDPlusRW,
CanWriteDVDPlusRWDL,
CanWriteDVDR,
CanWriteDVDRDL,
CanWriteDVDRW,
CanWriteHDDVDR,
CanWriteHDDVDRAM,
CanWriteOldBDR,
CanWriteOldBDRE,
CanWritePackedSubchannelInTAO,
CanWriteRWSubchannelInSAO,
CanWriteRWSubchannelInTAO,
CanWriteRaw,
CanWriteRawMultiSession,
CanWriteRawSubchannelInTAO,
ChangerIsSideChangeCapable,
ChangerSlots,
ChangerSupportsDiscPresent,
CPRMVersion,
CSSVersion,
DBML,
DVDMultiRead,
EmbeddedChanger,
ErrorRecoveryPage,
FirmwareDate,
LoadingMechanismType,
Locked,
LogicalBlockSize,
MultiRead,
PhysicalInterfaceStandardNumber,
PreventJumper,
SupportsAACS,
SupportsBusEncryption,
SupportsC2,
SupportsCPRM,
SupportsCSS,
SupportsDAP,
SupportsDeviceBusyEvent,
SupportsHybridDiscs,
SupportsModePage1Ch,
SupportsOSSC,
SupportsPWP,
SupportsSWPP,
SupportsSecurDisc,
SupportsSeparateVolume,
SupportsVCPS,
SupportsWriteInhibitDCB,
SupportsWriteProtectPAC,
VolumeLevels,
BinaryData,
BlocksPerReadableUnitSql,
LogicalBlockSizeSql,
PhysicalInterfaceStandardNumberSql,
VolumeLevelsSql) SELECT AACSVersion,
AGIDs,
BindingNonceBlocks,
BlocksPerReadableUnitSql AS BlocksPerReadableUnit,
BufferUnderrunFreeInDVD,
BufferUnderrunFreeInSAO,
BufferUnderrunFreeInTAO,
CanAudioScan,
CanEject,
CanEraseSector,
CanExpandBDRESpareArea,
CanFormat,
CanFormatBDREWithoutSpare,
CanFormatCert,
CanFormatFRF,
CanFormatQCert,
CanFormatRRM,
CanGenerateBindingNonce,
CanLoad,
CanMuteSeparateChannels,
CanOverwriteSAOTrack,
CanOverwriteTAOTrack,
CanPlayCDAudio,
CanPseudoOverwriteBDR,
CanReadAllDualR,
CanReadAllDualRW,
CanReadBD,
CanReadBDR,
CanReadBDRE1,
CanReadBDRE2,
CanReadBDROM,
CanReadBluBCA,
CanReadCD,
CanReadCDMRW,
CanReadCPRM_MKB,
CanReadDDCD,
CanReadDVD,
CanReadDVDPlusMRW,
CanReadDVDPlusR,
CanReadDVDPlusRDL,
CanReadDVDPlusRW,
CanReadDVDPlusRWDL,
CanReadDriveAACSCertificate,
CanReadHDDVD,
CanReadHDDVDR,
CanReadHDDVDRAM,
CanReadLeadInCDText,
CanReadOldBDR,
CanReadOldBDRE,
CanReadOldBDROM,
CanReadSpareAreaInformation,
CanReportDriveSerial,
CanReportMediaSerial,
CanTestWriteDDCDR,
CanTestWriteDVD,
CanTestWriteInSAO,
CanTestWriteInTAO,
CanUpgradeFirmware,
CanWriteBD,
CanWriteBDR,
CanWriteBDRE1,
CanWriteBDRE2,
CanWriteBusEncryptedBlocks,
CanWriteCDMRW,
CanWriteCDRW,
CanWriteCDRWCAV,
CanWriteCDSAO,
CanWriteCDTAO,
CanWriteCSSManagedDVD,
CanWriteDDCDR,
CanWriteDDCDRW,
CanWriteDVDPlusMRW,
CanWriteDVDPlusR,
CanWriteDVDPlusRDL,
CanWriteDVDPlusRW,
CanWriteDVDPlusRWDL,
CanWriteDVDR,
CanWriteDVDRDL,
CanWriteDVDRW,
CanWriteHDDVDR,
CanWriteHDDVDRAM,
CanWriteOldBDR,
CanWriteOldBDRE,
CanWritePackedSubchannelInTAO,
CanWriteRWSubchannelInSAO,
CanWriteRWSubchannelInTAO,
CanWriteRaw,
CanWriteRawMultiSession,
CanWriteRawSubchannelInTAO,
ChangerIsSideChangeCapable,
ChangerSlots,
ChangerSupportsDiscPresent,
CPRMVersion,
CSSVersion,
DBML,
DVDMultiRead,
EmbeddedChanger,
ErrorRecoveryPage,
FirmwareDate,
LoadingMechanismType,
Locked,
LogicalBlockSizeSql AS LogicalBlockSize,
MultiRead,
PhysicalInterfaceStandardNumberSql AS PhysicalInterfaceStandardNumber,
PreventJumper,
SupportsAACS,
SupportsBusEncryption,
SupportsC2,
SupportsCPRM,
SupportsCSS,
SupportsDAP,
SupportsDeviceBusyEvent,
SupportsHybridDiscs,
SupportsModePage1Ch,
SupportsOSSC,
SupportsPWP,
SupportsSWPP,
SupportsSecurDisc,
SupportsSeparateVolume,
SupportsVCPS,
SupportsWriteInhibitDCB,
SupportsWriteProtectPAC,
VolumeLevelsSql AS VolumeLevels,
BinaryData,
BlocksPerReadableUnitSql,
LogicalBlockSizeSql,
PhysicalInterfaceStandardNumberSql,
VolumeLevelsSql FROM MmcFeatures_old WHERE VolumeLevelsSql >= 0 OR VolumeLevelsSql IS NULL");

                migrationBuilder.Sql(@"INSERT INTO MmcFeatures (AACSVersion,
AGIDs,
BindingNonceBlocks,
BlocksPerReadableUnit,
BufferUnderrunFreeInDVD,
BufferUnderrunFreeInSAO,
BufferUnderrunFreeInTAO,
CanAudioScan,
CanEject,
CanEraseSector,
CanExpandBDRESpareArea,
CanFormat,
CanFormatBDREWithoutSpare,
CanFormatCert,
CanFormatFRF,
CanFormatQCert,
CanFormatRRM,
CanGenerateBindingNonce,
CanLoad,
CanMuteSeparateChannels,
CanOverwriteSAOTrack,
CanOverwriteTAOTrack,
CanPlayCDAudio,
CanPseudoOverwriteBDR,
CanReadAllDualR,
CanReadAllDualRW,
CanReadBD,
CanReadBDR,
CanReadBDRE1,
CanReadBDRE2,
CanReadBDROM,
CanReadBluBCA,
CanReadCD,
CanReadCDMRW,
CanReadCPRM_MKB,
CanReadDDCD,
CanReadDVD,
CanReadDVDPlusMRW,
CanReadDVDPlusR,
CanReadDVDPlusRDL,
CanReadDVDPlusRW,
CanReadDVDPlusRWDL,
CanReadDriveAACSCertificate,
CanReadHDDVD,
CanReadHDDVDR,
CanReadHDDVDRAM,
CanReadLeadInCDText,
CanReadOldBDR,
CanReadOldBDRE,
CanReadOldBDROM,
CanReadSpareAreaInformation,
CanReportDriveSerial,
CanReportMediaSerial,
CanTestWriteDDCDR,
CanTestWriteDVD,
CanTestWriteInSAO,
CanTestWriteInTAO,
CanUpgradeFirmware,
CanWriteBD,
CanWriteBDR,
CanWriteBDRE1,
CanWriteBDRE2,
CanWriteBusEncryptedBlocks,
CanWriteCDMRW,
CanWriteCDRW,
CanWriteCDRWCAV,
CanWriteCDSAO,
CanWriteCDTAO,
CanWriteCSSManagedDVD,
CanWriteDDCDR,
CanWriteDDCDRW,
CanWriteDVDPlusMRW,
CanWriteDVDPlusR,
CanWriteDVDPlusRDL,
CanWriteDVDPlusRW,
CanWriteDVDPlusRWDL,
CanWriteDVDR,
CanWriteDVDRDL,
CanWriteDVDRW,
CanWriteHDDVDR,
CanWriteHDDVDRAM,
CanWriteOldBDR,
CanWriteOldBDRE,
CanWritePackedSubchannelInTAO,
CanWriteRWSubchannelInSAO,
CanWriteRWSubchannelInTAO,
CanWriteRaw,
CanWriteRawMultiSession,
CanWriteRawSubchannelInTAO,
ChangerIsSideChangeCapable,
ChangerSlots,
ChangerSupportsDiscPresent,
CPRMVersion,
CSSVersion,
DBML,
DVDMultiRead,
EmbeddedChanger,
ErrorRecoveryPage,
FirmwareDate,
LoadingMechanismType,
Locked,
LogicalBlockSize,
MultiRead,
PhysicalInterfaceStandardNumber,
PreventJumper,
SupportsAACS,
SupportsBusEncryption,
SupportsC2,
SupportsCPRM,
SupportsCSS,
SupportsDAP,
SupportsDeviceBusyEvent,
SupportsHybridDiscs,
SupportsModePage1Ch,
SupportsOSSC,
SupportsPWP,
SupportsSWPP,
SupportsSecurDisc,
SupportsSeparateVolume,
SupportsVCPS,
SupportsWriteInhibitDCB,
SupportsWriteProtectPAC,
VolumeLevels,
BinaryData,
BlocksPerReadableUnitSql,
LogicalBlockSizeSql,
PhysicalInterfaceStandardNumberSql,
VolumeLevelsSql) SELECT AACSVersion,
AGIDs,
BindingNonceBlocks,
BlocksPerReadableUnitSql AS BlocksPerReadableUnit,
BufferUnderrunFreeInDVD,
BufferUnderrunFreeInSAO,
BufferUnderrunFreeInTAO,
CanAudioScan,
CanEject,
CanEraseSector,
CanExpandBDRESpareArea,
CanFormat,
CanFormatBDREWithoutSpare,
CanFormatCert,
CanFormatFRF,
CanFormatQCert,
CanFormatRRM,
CanGenerateBindingNonce,
CanLoad,
CanMuteSeparateChannels,
CanOverwriteSAOTrack,
CanOverwriteTAOTrack,
CanPlayCDAudio,
CanPseudoOverwriteBDR,
CanReadAllDualR,
CanReadAllDualRW,
CanReadBD,
CanReadBDR,
CanReadBDRE1,
CanReadBDRE2,
CanReadBDROM,
CanReadBluBCA,
CanReadCD,
CanReadCDMRW,
CanReadCPRM_MKB,
CanReadDDCD,
CanReadDVD,
CanReadDVDPlusMRW,
CanReadDVDPlusR,
CanReadDVDPlusRDL,
CanReadDVDPlusRW,
CanReadDVDPlusRWDL,
CanReadDriveAACSCertificate,
CanReadHDDVD,
CanReadHDDVDR,
CanReadHDDVDRAM,
CanReadLeadInCDText,
CanReadOldBDR,
CanReadOldBDRE,
CanReadOldBDROM,
CanReadSpareAreaInformation,
CanReportDriveSerial,
CanReportMediaSerial,
CanTestWriteDDCDR,
CanTestWriteDVD,
CanTestWriteInSAO,
CanTestWriteInTAO,
CanUpgradeFirmware,
CanWriteBD,
CanWriteBDR,
CanWriteBDRE1,
CanWriteBDRE2,
CanWriteBusEncryptedBlocks,
CanWriteCDMRW,
CanWriteCDRW,
CanWriteCDRWCAV,
CanWriteCDSAO,
CanWriteCDTAO,
CanWriteCSSManagedDVD,
CanWriteDDCDR,
CanWriteDDCDRW,
CanWriteDVDPlusMRW,
CanWriteDVDPlusR,
CanWriteDVDPlusRDL,
CanWriteDVDPlusRW,
CanWriteDVDPlusRWDL,
CanWriteDVDR,
CanWriteDVDRDL,
CanWriteDVDRW,
CanWriteHDDVDR,
CanWriteHDDVDRAM,
CanWriteOldBDR,
CanWriteOldBDRE,
CanWritePackedSubchannelInTAO,
CanWriteRWSubchannelInSAO,
CanWriteRWSubchannelInTAO,
CanWriteRaw,
CanWriteRawMultiSession,
CanWriteRawSubchannelInTAO,
ChangerIsSideChangeCapable,
ChangerSlots,
ChangerSupportsDiscPresent,
CPRMVersion,
CSSVersion,
DBML,
DVDMultiRead,
EmbeddedChanger,
ErrorRecoveryPage,
FirmwareDate,
LoadingMechanismType,
Locked,
LogicalBlockSizeSql AS LogicalBlockSize,
MultiRead,
PhysicalInterfaceStandardNumberSql AS PhysicalInterfaceStandardNumber,
PreventJumper,
SupportsAACS,
SupportsBusEncryption,
SupportsC2,
SupportsCPRM,
SupportsCSS,
SupportsDAP,
SupportsDeviceBusyEvent,
SupportsHybridDiscs,
SupportsModePage1Ch,
SupportsOSSC,
SupportsPWP,
SupportsSWPP,
SupportsSecurDisc,
SupportsSeparateVolume,
SupportsVCPS,
SupportsWriteInhibitDCB,
SupportsWriteProtectPAC,
(65536+VolumeLevelsSql) AS VolumeLevels,
BinaryData,
BlocksPerReadableUnitSql,
LogicalBlockSizeSql,
PhysicalInterfaceStandardNumberSql,
VolumeLevelsSql FROM MmcFeatures_old WHERE VolumeLevelsSql < 0");

                migrationBuilder.DropTable("MmcFeatures_old");
            }
            #endregion

            #region TABLE: MmcSd
            migrationBuilder.CreateTable("MmcSd", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                CID         = table.Column<byte[]>(nullable: true), CSD = table.Column<byte[]>(nullable: true),
                OCR         = table.Column<byte[]>(nullable: true), SCR = table.Column<byte[]>(nullable: true),
                ExtendedCSD = table.Column<byte[]>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_MmcSd", x => x.Id);
            });

            if(MmcSdsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO MmcSd (Id, CID, CSD, OCR, SCR, ExtendedCSD) SELECT Id, CID, CSD, OCR, SCR, ExtendedCSD FROM MmcSds");

                migrationBuilder.DropTable("MmcSds");
            }
            #endregion

            #region TABLE: OperatingSystems
            if(OperatingSystemsExists)
                migrationBuilder.RenameTable("OperatingSystems", newName: "OperatingSystems_old");

            migrationBuilder.CreateTable("OperatingSystems", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name  = table.Column<string>(nullable: true), Version = table.Column<string>(nullable: true),
                Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_OperatingSystems", x => x.Id);
            });

            if(OperatingSystemsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO OperatingSystems (Id, Name, Version, Count) SELECT Id, Name, Version, Count FROM OperatingSystems_old");

                migrationBuilder.DropTable("OperatingSystems_old");
            }
            #endregion

            #region TABLE: Partitions
            if(PartitionsExists)
                migrationBuilder.RenameTable("Partitions", newName: "Partitions_old");

            migrationBuilder.CreateTable("Partitions", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(nullable: true), Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Partitions", x => x.Id);
            });

            if(PartitionsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Partitions (Id, Name, Count) SELECT Id, Name, Count FROM Partitions_old");

                migrationBuilder.DropTable("Partitions_old");
            }
            #endregion

            #region TABLE: Pcmcia
            migrationBuilder.CreateTable("Pcmcia", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                CIS                 = table.Column<byte[]>(nullable: true),
                Compliance          = table.Column<string>(nullable: true),
                ManufacturerCode    = table.Column<ushort>(nullable: true),
                CardCode            = table.Column<ushort>(nullable: true),
                Manufacturer        = table.Column<string>(nullable: true),
                ProductName         = table.Column<string>(nullable: true),
                ManufacturerCodeSql = table.Column<short>(nullable: true),
                CardCodeSql         = table.Column<short>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Pcmcia", x => x.Id);
            });

            if(PcmciasExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Pcmcia (Id, CIS, Compliance, ManufacturerCode, CardCode, Manufacturer, ProductName, ManufacturerCodeSql, CardCodeSql) SELECT Id, CIS, Compliance, ManufacturerCodeSql AS ManufacturerCode, CardCodeSql AS CardCode, Manufacturer, ProductName, ManufacturerCodeSql, CardCodeSql FROM Pcmcias");

                migrationBuilder.DropTable("Pcmcias");
            }
            #endregion

            #region TABLE: ScsiMode
            migrationBuilder.CreateTable("ScsiMode", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                MediumType        = table.Column<byte>(nullable: true), WriteProtected = table.Column<bool>(),
                Speed             = table.Column<byte>(nullable: true),
                BufferedMode      = table.Column<byte>(nullable: true),
                BlankCheckEnabled = table.Column<bool>(), DPOandFUA = table.Column<bool>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_ScsiMode", x => x.Id);
            });

            if(ScsiModesExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO ScsiMode (Id, MediumType, WriteProtected, Speed, BufferedMode, BlankCheckEnabled, DPOandFUA) SELECT Id, MediumType, WriteProtected, Speed, BufferedMode, BlankCheckEnabled, DPOandFUA FROM ScsiModes");

                migrationBuilder.DropTable("ScsiModes");
            }
            #endregion

            #region TABLE: Ssc
            migrationBuilder.CreateTable("Ssc", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                BlockSizeGranularity = table.Column<byte>(nullable: true),
                MaxBlockLength       = table.Column<uint>(nullable: true),
                MinBlockLength       = table.Column<uint>(nullable: true),
                MaxBlockLengthSql    = table.Column<int>(nullable: true),
                MinBlockLengthSql    = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Ssc", x => x.Id);
            });

            if(SscsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Ssc (Id, BlockSizeGranularity, MaxBlockLength, MinBlockLength, MaxBlockLengthSql, MinBlockLengthSql) SELECT Id, BlockSizeGranularity, MaxBlockLengthSql AS MaxBlockLength, MinBlockLengthSql AS MinBlockLength, MaxBlockLengthSql, MinBlockLengthSql FROM Sscs");

                migrationBuilder.DropTable("Sscs");
            }
            #endregion

            #region TABLE: Usb
            migrationBuilder.CreateTable("Usb", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                VendorID       = table.Column<ushort>(), ProductID             = table.Column<ushort>(),
                Manufacturer   = table.Column<string>(nullable: true), Product = table.Column<string>(nullable: true),
                RemovableMedia = table.Column<bool>(), Descriptors             = table.Column<byte[]>(nullable: true),
                VendorIDSql    = table.Column<short>(), ProductIDSql           = table.Column<short>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Usb", x => x.Id);
            });

            if(UsbsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Usb (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql) SELECT Id, VendorIDSql AS VendorID, ProductIDSql AS ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql FROM Usbs WHERE VendorIDSql >= 0 AND ProductIDSql >= 0");

                migrationBuilder.
                    Sql("INSERT INTO Usb (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql) SELECT Id, (65536+VendorIDSql) AS VendorID, ProductIDSql AS ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql FROM Usbs WHERE VendorIDSql < 0 AND ProductIDSql >= 0");

                migrationBuilder.
                    Sql("INSERT INTO Usb (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql) SELECT Id, VendorIDSql AS VendorID, (65536+ProductIDSql) AS ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql FROM Usbs WHERE VendorIDSql >= 0 AND ProductIDSql < 0");

                migrationBuilder.
                    Sql("INSERT INTO Usb (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql) SELECT Id, (65536+VendorIDSql) AS VendorID, (65536+ProductIDSql) AS ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql FROM Usbs WHERE VendorIDSql < 0 AND ProductIDSql < 0");

                migrationBuilder.DropTable("Usbs");
            }
            #endregion

            #region TABLE: UsbVendors
            if(UsbVendorsExists)
                migrationBuilder.RenameTable("UsbVendors", newName: "UsbVendors_old");

            migrationBuilder.CreateTable("UsbVendors", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                VendorId  = table.Column<int>(), Vendor            = table.Column<string>(nullable: true),
                AddedWhen = table.Column<DateTime>(), ModifiedWhen = table.Column<DateTime>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UsbVendors", x => x.Id);
            });

            if(UsbVendorsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO UsbVendors (Id, VendorId, Vendor, AddedWhen, ModifiedWhen) SELECT Id, VendorId, Vendor, AddedWhen, ModifiedWhen FROM UsbVendors_old");

                migrationBuilder.DropTable("UsbVendors_old");
            }
            #endregion

            #region TABLE: Versions
            if(VersionsExists)
                migrationBuilder.RenameTable("Versions", newName: "Versions_old");

            migrationBuilder.CreateTable("Versions", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Value = table.Column<string>(nullable: true), Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Versions", x => x.Id);
            });

            if(VersionsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Versions (`Id`, `Value`, `Count`) SELECT `Id`, `Value`, `Count` FROM Versions_old");

                migrationBuilder.DropTable("Versions_old");
            }
            #endregion

            #region TABLE: Mmc
            migrationBuilder.CreateTable("Mmc", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                FeaturesId = table.Column<int>(nullable: true), ModeSense2AData = table.Column<byte[]>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Mmc", x => x.Id);

                table.ForeignKey("FK_Mmc_MmcFeatures_FeaturesId", x => x.FeaturesId, "MmcFeatures", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(MmcsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Mmc (Id, FeaturesId, ModeSense2AData) SELECT Id, Features_Id, ModeSense2AData FROM Mmcs WHERE EXISTS(SELECT 1 FROM MmcFeatures WHERE MmcFeatures.Id = Features_Id) OR Features_Id IS NULL");

                migrationBuilder.DropTable("Mmcs");
            }
            #endregion

            #region TABLE: BlockDescriptor
            migrationBuilder.CreateTable("BlockDescriptor", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Density        = table.Column<byte>(), Blocks                  = table.Column<ulong>(nullable: true),
                BlockLength    = table.Column<uint>(nullable: true), BlocksSql = table.Column<long>(nullable: true),
                BlockLengthSql = table.Column<int>(nullable: true), ScsiModeId = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_BlockDescriptor", x => x.Id);

                table.ForeignKey("FK_BlockDescriptor_ScsiMode_ScsiModeId", x => x.ScsiModeId, "ScsiMode", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(BlockDescriptorsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO BlockDescriptor (Id, Density, Blocks, BlocksSql, BlockLength, BlockLengthSql, ScsiModeId) SELECT Id, Density, BlocksSql AS Blocks, BlocksSql, BlockLengthSql AS BlockLength, BlockLengthSql, ScsiMode_Id FROM BlockDescriptors");

                migrationBuilder.DropTable("BlockDescriptors");
            }
            #endregion

            #region TABLE: TestedSequentialMedia
            migrationBuilder.CreateTable("TestedSequentialMedia", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                CanReadMediaSerial = table.Column<bool>(nullable: true),
                Density            = table.Column<byte>(nullable: true),
                Manufacturer       = table.Column<string>(nullable: true), MediaIsRecognized = table.Column<bool>(),
                MediumType         = table.Column<byte>(nullable: true),
                MediumTypeName     = table.Column<string>(nullable: true),
                Model              = table.Column<string>(nullable: true),
                ModeSense6Data     = table.Column<byte[]>(nullable: true),
                ModeSense10Data    = table.Column<byte[]>(nullable: true),
                SscId              = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_TestedSequentialMedia", x => x.Id);

                table.ForeignKey("FK_TestedSequentialMedia_Ssc_SscId", x => x.SscId, "Ssc", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(TestedSequentialMediasExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO TestedSequentialMedia (Id, CanReadMediaSerial, Density, Manufacturer, MediaIsRecognized, MediumType, MediumTypeName, Model, ModeSense6Data, ModeSense10Data, SscId) SELECT Id, CanReadMediaSerial, Density, Manufacturer, MediaIsRecognized, MediumType, MediumTypeName, Model, ModeSense6Data, ModeSense10Data, Ssc_Id FROM TestedSequentialMedias");

                migrationBuilder.DropTable("TestedSequentialMedias");
            }
            #endregion

            #region TABLE: UsbProducts
            if(UsbProductsExists)
                migrationBuilder.RenameTable("UsbProducts", newName: "UsbProducts_old");

            migrationBuilder.CreateTable("UsbProducts", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                ProductId = table.Column<int>(), Product           = table.Column<string>(nullable: true),
                AddedWhen = table.Column<DateTime>(), ModifiedWhen = table.Column<DateTime>(),
                VendorId  = table.Column<int>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UsbProducts", x => x.Id);

                table.ForeignKey("FK_UsbProducts_UsbVendors_VendorId", x => x.VendorId, "UsbVendors", "Id",
                                 onDelete: ReferentialAction.Cascade);
            });

            if(UsbProductsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO UsbProducts (Id, ProductId, Product, AddedWhen, ModifiedWhen, VendorId) SELECT Id, ProductId, Product, AddedWhen, ModifiedWhen, VendorId FROM UsbProducts_old");

                migrationBuilder.DropTable("UsbProducts_old");
            }
            #endregion

            #region TABLE: SscSupportedMedia
            migrationBuilder.CreateTable("SscSupportedMedia", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                MediumType              = table.Column<byte>(), Width = table.Column<ushort>(),
                Length                  = table.Column<ushort>(),
                Organization            = table.Column<string>(nullable: true),
                Name                    = table.Column<string>(nullable: true),
                Description             = table.Column<string>(nullable: true), WidthSql = table.Column<short>(),
                LengthSql               = table.Column<short>(),
                SscId                   = table.Column<int>(nullable: true),
                TestedSequentialMediaId = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_SscSupportedMedia", x => x.Id);

                table.ForeignKey("FK_SscSupportedMedia_Ssc_SscId", x => x.SscId, "Ssc", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_SscSupportedMedia_TestedSequentialMedia_TestedSequentialMedi~",
                                 x => x.TestedSequentialMediaId, "TestedSequentialMedia", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(SscSupportedMediasExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO SscSupportedMedia (Id, MediumType, Width, Length, Organization, Name, Description, WidthSql, LengthSql, SscId, TestedSequentialMediaId) SELECT Id, MediumType, WidthSql AS Width, LengthSql AS Length, Organization, Name, Description, WidthSql, LengthSql, Ssc_Id, TestedSequentialMedia_Id FROM SscSupportedMedias");

                migrationBuilder.DropTable("SscSupportedMedias");
            }
            #endregion

            #region TABLE: SupportedDensity
            migrationBuilder.CreateTable("SupportedDensity", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                PrimaryCode             = table.Column<byte>(), SecondaryCode = table.Column<byte>(),
                Writable                = table.Column<bool>(), Duplicate     = table.Column<bool>(),
                DefaultDensity          = table.Column<bool>(), BitsPerMm     = table.Column<uint>(),
                Width                   = table.Column<ushort>(),
                Tracks                  = table.Column<ushort>(), Capacity = table.Column<uint>(),
                Organization            = table.Column<string>(nullable: true),
                Name                    = table.Column<string>(nullable: true),
                Description             = table.Column<string>(nullable: true), BitsPerMmSql = table.Column<int>(),
                WidthSql                = table.Column<short>(), TracksSql                   = table.Column<short>(),
                CapacitySql             = table.Column<int>(),
                SscId                   = table.Column<int>(nullable: true),
                TestedSequentialMediaId = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_SupportedDensity", x => x.Id);

                table.ForeignKey("FK_SupportedDensity_Ssc_SscId", x => x.SscId, "Ssc", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                                 x => x.TestedSequentialMediaId, "TestedSequentialMedia", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(SupportedDensitiesExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO SupportedDensity (PrimaryCode, SecondaryCode, Writable, Duplicate, DefaultDensity, BitsPerMm, Width, Tracks, Capacity, Organization, Name, Description, BitsPerMmSql, WidthSql, TracksSql, CapacitySql, SscId, TestedSequentialMediaId) SELECT PrimaryCode, SecondaryCode, Writable, Duplicate, DefaultDensity, BitsPerMmSql AS BitsPerMm, WidthSql AS Width, TracksSql AS Tracks, CapacitySql AS Capacity, Organization, Name, Description, BitsPerMmSql, WidthSql, TracksSql, CapacitySql, Ssc_Id, TestedSequentialMedia_Id FROM SupportedDensities");

                migrationBuilder.DropTable("SupportedDensities");
            }
            #endregion

            #region TABLE: DensityCode
            migrationBuilder.CreateTable("DensityCode", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Code = table.Column<int>(), SscSupportedMediaId = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_DensityCode", x => x.Id);

                table.ForeignKey("FK_DensityCode_SscSupportedMedia_SscSupportedMediaId", x => x.SscSupportedMediaId,
                                 "SscSupportedMedia", "Id", onDelete: ReferentialAction.Restrict);
            });

            if(DensityCodesExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO DensityCode (Id, Code, SscSupportedMediaId) SELECT Id, Code, SscSupportedMedia_Id FROM DensityCodes");

                migrationBuilder.DropTable("DensityCodes");
            }
            #endregion

            #region TABLE: Devices
            if(DevicesExists)
                migrationBuilder.RenameTable("Devices", newName: "Devices_old");

            migrationBuilder.CreateTable("Devices", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                USBId            = table.Column<int>(nullable: true),
                FireWireId       = table.Column<int>(nullable: true),
                PCMCIAId         = table.Column<int>(nullable: true), CompactFlash = table.Column<bool>(),
                ATAId            = table.Column<int>(nullable: true),
                ATAPIId          = table.Column<int>(nullable: true),
                SCSIId           = table.Column<int>(nullable: true),
                MultiMediaCardId = table.Column<int>(nullable: true),
                SecureDigitalId  = table.Column<int>(nullable: true),
                Manufacturer     = table.Column<string>(nullable: true),
                Model            = table.Column<string>(nullable: true),
                Revision         = table.Column<string>(nullable: true), Type = table.Column<int>(),
                AddedWhen        = table.Column<DateTime>(),
                ModifiedWhen     = table.Column<DateTime>(nullable: true),
                CdOffsetId       = table.Column<int>(nullable: true), OptimalMultipleSectorsRead = table.Column<int>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Devices", x => x.Id);

                table.ForeignKey("FK_Devices_CdOffsets_CdOffsetId", x => x.CdOffsetId, "CdOffsets", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Devices_FireWire_FireWireId", x => x.FireWireId, "FireWire", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Devices_MmcSd_MultiMediaCardId", x => x.MultiMediaCardId, "MmcSd", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Devices_Pcmcia_PCMCIAId", x => x.PCMCIAId, "Pcmcia", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Devices_MmcSd_SecureDigitalId", x => x.SecureDigitalId, "MmcSd", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Devices_Usb_USBId", x => x.USBId, "Usb", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(DevicesExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Devices (Id, USBId, FireWireId, PCMCIAId, CompactFlash, ATAId, ATAPIId, SCSIId, MultiMediaCardId, SecureDigitalId, Manufacturer, Model, Revision, Type, AddedWhen, ModifiedWhen, CdOffsetId, OptimalMultipleSectorsRead) SELECT Id, USB_Id, FireWire_Id, PCMCIA_Id, CompactFlash, ATA_Id, ATAPI_Id, SCSI_Id, MultiMediaCard_Id, SecureDigital_Id, Manufacturer, Model, Revision, Type, AddedWhen, ModifiedWhen, CdOffset_Id, OptimalMultipleSectorsRead FROM Devices_old");

                migrationBuilder.DropTable("Devices_old");
            }
            #endregion

            #region TABLE: DeviceStats
            if(DeviceStatsExists)
                migrationBuilder.RenameTable("DeviceStats", newName: "DeviceStats_old");

            migrationBuilder.CreateTable("DeviceStats", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Manufacturer = table.Column<string>(nullable: true), Model = table.Column<string>(nullable: true),
                Revision     = table.Column<string>(nullable: true), Bus   = table.Column<string>(nullable: true),
                ReportId     = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_DeviceStats", x => x.Id);

                table.ForeignKey("FK_DeviceStats_Devices_ReportId", x => x.ReportId, "Devices", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(DeviceStatsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO DeviceStats (Id, Manufacturer, Model, Revision, Bus, ReportId) SELECT Id, Manufacturer, Model, Revision, Bus, Report_Id FROM DeviceStats_old");

                migrationBuilder.DropTable("DeviceStats_old");
            }
            #endregion

            #region TABLE: Reports
            migrationBuilder.CreateTable("Reports", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                USBId            = table.Column<int>(nullable: true),
                FireWireId       = table.Column<int>(nullable: true),
                PCMCIAId         = table.Column<int>(nullable: true), CompactFlash = table.Column<bool>(),
                ATAId            = table.Column<int>(nullable: true),
                ATAPIId          = table.Column<int>(nullable: true),
                SCSIId           = table.Column<int>(nullable: true),
                MultiMediaCardId = table.Column<int>(nullable: true),
                SecureDigitalId  = table.Column<int>(nullable: true),
                Manufacturer     = table.Column<string>(nullable: true), Model = table.Column<string>(nullable: true),
                Revision         = table.Column<string>(nullable: true), Type  = table.Column<int>(),
                UploadedWhen     = table.Column<DateTime>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Reports", x => x.Id);

                table.ForeignKey("FK_Reports_FireWire_FireWireId", x => x.FireWireId, "FireWire", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Reports_MmcSd_MultiMediaCardId", x => x.MultiMediaCardId, "MmcSd", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Reports_Pcmcia_PCMCIAId", x => x.PCMCIAId, "Pcmcia", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Reports_MmcSd_SecureDigitalId", x => x.SecureDigitalId, "MmcSd", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Reports_Usb_USBId", x => x.USBId, "Usb", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(UploadedReportsExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Reports (Id, USBId, FireWireId, PCMCIAId, CompactFlash, ATAId, ATAPIId, SCSIId, MultiMediaCardId, SecureDigitalId, Manufacturer, Model, Revision, Type, UploadedWhen) SELECT Id, USB_Id, FireWire_Id, PCMCIA_Id, CompactFlash, ATA_Id, ATAPI_Id, SCSI_Id, MultiMediaCard_Id, SecureDigital_Id, Manufacturer, Model, Revision, Type, UploadedWhen FROM UploadedReports");

                migrationBuilder.DropTable("UploadedReports");
            }
            #endregion

            #region TABLE: TestedMedia
            migrationBuilder.CreateTable("TestedMedia", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                IdentifyData                     = table.Column<byte[]>(nullable: true),
                Blocks                           = table.Column<ulong>(nullable: true),
                BlockSize                        = table.Column<uint>(nullable: true),
                CanReadAACS                      = table.Column<bool>(nullable: true),
                CanReadADIP                      = table.Column<bool>(nullable: true),
                CanReadATIP                      = table.Column<bool>(nullable: true),
                CanReadBCA                       = table.Column<bool>(nullable: true),
                CanReadC2Pointers                = table.Column<bool>(nullable: true),
                CanReadCMI                       = table.Column<bool>(nullable: true),
                CanReadCorrectedSubchannel       = table.Column<bool>(nullable: true),
                CanReadCorrectedSubchannelWithC2 = table.Column<bool>(nullable: true),
                CanReadDCB                       = table.Column<bool>(nullable: true),
                CanReadDDS                       = table.Column<bool>(nullable: true),
                CanReadDMI                       = table.Column<bool>(nullable: true),
                CanReadDiscInformation           = table.Column<bool>(nullable: true),
                CanReadFullTOC                   = table.Column<bool>(nullable: true),
                CanReadHDCMI                     = table.Column<bool>(nullable: true),
                CanReadLayerCapacity             = table.Column<bool>(nullable: true),
                CanReadFirstTrackPreGap          = table.Column<bool>(nullable: true),
                CanReadLeadIn                    = table.Column<bool>(nullable: true),
                CanReadLeadOut                   = table.Column<bool>(nullable: true),
                CanReadMediaID                   = table.Column<bool>(nullable: true),
                CanReadMediaSerial               = table.Column<bool>(nullable: true),
                CanReadPAC                       = table.Column<bool>(nullable: true),
                CanReadPFI                       = table.Column<bool>(nullable: true),
                CanReadPMA                       = table.Column<bool>(nullable: true),
                CanReadPQSubchannel              = table.Column<bool>(nullable: true),
                CanReadPQSubchannelWithC2        = table.Column<bool>(nullable: true),
                CanReadPRI                       = table.Column<bool>(nullable: true),
                CanReadRWSubchannel              = table.Column<bool>(nullable: true),
                CanReadRWSubchannelWithC2        = table.Column<bool>(nullable: true),
                CanReadRecordablePFI             = table.Column<bool>(nullable: true),
                CanReadSpareAreaInformation      = table.Column<bool>(nullable: true),
                CanReadTOC                       = table.Column<bool>(nullable: true),
                Density                          = table.Column<byte>(nullable: true),
                LongBlockSize                    = table.Column<uint>(nullable: true),
                Manufacturer                     = table.Column<string>(nullable: true),
                MediaIsRecognized                = table.Column<bool>(),
                MediumType                       = table.Column<byte>(nullable: true),
                MediumTypeName                   = table.Column<string>(nullable: true),
                Model                            = table.Column<string>(nullable: true),
                SupportsHLDTSTReadRawDVD         = table.Column<bool>(nullable: true),
                SupportsNECReadCDDA              = table.Column<bool>(nullable: true),
                SupportsPioneerReadCDDA          = table.Column<bool>(nullable: true),
                SupportsPioneerReadCDDAMSF       = table.Column<bool>(nullable: true),
                SupportsPlextorReadCDDA          = table.Column<bool>(nullable: true),
                SupportsPlextorReadRawDVD        = table.Column<bool>(nullable: true),
                SupportsRead10                   = table.Column<bool>(nullable: true),
                SupportsRead12                   = table.Column<bool>(nullable: true),
                SupportsRead16                   = table.Column<bool>(nullable: true),
                SupportsRead6                    = table.Column<bool>(nullable: true),
                SupportsReadCapacity16           = table.Column<bool>(nullable: true),
                SupportsReadCapacity             = table.Column<bool>(nullable: true),
                SupportsReadCd                   = table.Column<bool>(nullable: true),
                SupportsReadCdMsf                = table.Column<bool>(nullable: true),
                SupportsReadCdRaw                = table.Column<bool>(nullable: true),
                SupportsReadCdMsfRaw             = table.Column<bool>(nullable: true),
                SupportsReadLong16               = table.Column<bool>(nullable: true),
                SupportsReadLong                 = table.Column<bool>(nullable: true),
                ModeSense6Data                   = table.Column<byte[]>(nullable: true),
                ModeSense10Data                  = table.Column<byte[]>(nullable: true),
                CHSId                            = table.Column<int>(nullable: true),
                CurrentCHSId                     = table.Column<int>(nullable: true),
                LBASectors                       = table.Column<uint>(nullable: true),
                LBA48Sectors                     = table.Column<ulong>(nullable: true),
                LogicalAlignment                 = table.Column<ushort>(nullable: true),
                NominalRotationRate              = table.Column<ushort>(nullable: true),
                PhysicalBlockSize                = table.Column<uint>(nullable: true),
                SolidStateDevice                 = table.Column<bool>(nullable: true),
                UnformattedBPT                   = table.Column<ushort>(nullable: true),
                UnformattedBPS                   = table.Column<ushort>(nullable: true),
                SupportsReadDmaLba               = table.Column<bool>(nullable: true),
                SupportsReadDmaRetryLba          = table.Column<bool>(nullable: true),
                SupportsReadLba                  = table.Column<bool>(nullable: true),
                SupportsReadRetryLba             = table.Column<bool>(nullable: true),
                SupportsReadLongLba              = table.Column<bool>(nullable: true),
                SupportsReadLongRetryLba         = table.Column<bool>(nullable: true),
                SupportsSeekLba                  = table.Column<bool>(nullable: true),
                SupportsReadDmaLba48             = table.Column<bool>(nullable: true),
                SupportsReadLba48                = table.Column<bool>(nullable: true),
                SupportsReadDma                  = table.Column<bool>(nullable: true),
                SupportsReadDmaRetry             = table.Column<bool>(nullable: true),
                SupportsReadRetry                = table.Column<bool>(nullable: true),
                SupportsReadSectors              = table.Column<bool>(nullable: true),
                SupportsReadLongRetry            = table.Column<bool>(nullable: true),
                SupportsSeek                     = table.Column<bool>(nullable: true),
                CanReadingIntersessionLeadIn     = table.Column<bool>(nullable: true),
                CanReadingIntersessionLeadOut    = table.Column<bool>(nullable: true),
                IntersessionLeadInData           = table.Column<byte[]>(nullable: true),
                IntersessionLeadOutData          = table.Column<byte[]>(nullable: true),
                BlocksSql                        = table.Column<long>(nullable: true),
                BlockSizeSql                     = table.Column<int>(nullable: true),
                LongBlockSizeSql                 = table.Column<int>(nullable: true),
                LBASectorsSql                    = table.Column<int>(nullable: true),
                LBA48SectorsSql                  = table.Column<long>(nullable: true),
                LogicalAlignmentSql              = table.Column<short>(nullable: true),
                NominalRotationRateSql           = table.Column<short>(nullable: true),
                PhysicalBlockSizeSql             = table.Column<int>(nullable: true),
                UnformattedBPTSql                = table.Column<short>(nullable: true),
                UnformattedBPSSql                = table.Column<short>(nullable: true),
                Read6Data                        = table.Column<byte[]>(nullable: true),
                Read10Data                       = table.Column<byte[]>(nullable: true),
                Read12Data                       = table.Column<byte[]>(nullable: true),
                Read16Data                       = table.Column<byte[]>(nullable: true),
                ReadLong10Data                   = table.Column<byte[]>(nullable: true),
                ReadLong16Data                   = table.Column<byte[]>(nullable: true),
                ReadSectorsData                  = table.Column<byte[]>(nullable: true),
                ReadSectorsRetryData             = table.Column<byte[]>(nullable: true),
                ReadDmaData                      = table.Column<byte[]>(nullable: true),
                ReadDmaRetryData                 = table.Column<byte[]>(nullable: true),
                ReadLbaData                      = table.Column<byte[]>(nullable: true),
                ReadRetryLbaData                 = table.Column<byte[]>(nullable: true),
                ReadDmaLbaData                   = table.Column<byte[]>(nullable: true),
                ReadDmaRetryLbaData              = table.Column<byte[]>(nullable: true),
                ReadLba48Data                    = table.Column<byte[]>(nullable: true),
                ReadDmaLba48Data                 = table.Column<byte[]>(nullable: true),
                ReadLongData                     = table.Column<byte[]>(nullable: true),
                ReadLongRetryData                = table.Column<byte[]>(nullable: true),
                ReadLongLbaData                  = table.Column<byte[]>(nullable: true),
                ReadLongRetryLbaData             = table.Column<byte[]>(nullable: true),
                TocData                          = table.Column<byte[]>(nullable: true),
                FullTocData                      = table.Column<byte[]>(nullable: true),
                AtipData                         = table.Column<byte[]>(nullable: true),
                PmaData                          = table.Column<byte[]>(nullable: true),
                ReadCdData                       = table.Column<byte[]>(nullable: true),
                ReadCdMsfData                    = table.Column<byte[]>(nullable: true),
                ReadCdFullData                   = table.Column<byte[]>(nullable: true),
                ReadCdMsfFullData                = table.Column<byte[]>(nullable: true),
                Track1PregapData                 = table.Column<byte[]>(nullable: true),
                LeadInData                       = table.Column<byte[]>(nullable: true),
                LeadOutData                      = table.Column<byte[]>(nullable: true),
                C2PointersData                   = table.Column<byte[]>(nullable: true),
                PQSubchannelData                 = table.Column<byte[]>(nullable: true),
                RWSubchannelData                 = table.Column<byte[]>(nullable: true),
                CorrectedSubchannelData          = table.Column<byte[]>(nullable: true),
                PQSubchannelWithC2Data           = table.Column<byte[]>(nullable: true),
                RWSubchannelWithC2Data           = table.Column<byte[]>(nullable: true),
                CorrectedSubchannelWithC2Data    = table.Column<byte[]>(nullable: true),
                PfiData                          = table.Column<byte[]>(nullable: true),
                DmiData                          = table.Column<byte[]>(nullable: true),
                CmiData                          = table.Column<byte[]>(nullable: true),
                DvdBcaData                       = table.Column<byte[]>(nullable: true),
                DvdAacsData                      = table.Column<byte[]>(nullable: true),
                DvdDdsData                       = table.Column<byte[]>(nullable: true),
                DvdSaiData                       = table.Column<byte[]>(nullable: true),
                PriData                          = table.Column<byte[]>(nullable: true),
                EmbossedPfiData                  = table.Column<byte[]>(nullable: true),
                AdipData                         = table.Column<byte[]>(nullable: true),
                DcbData                          = table.Column<byte[]>(nullable: true),
                HdCmiData                        = table.Column<byte[]>(nullable: true),
                DvdLayerData                     = table.Column<byte[]>(nullable: true),
                BluBcaData                       = table.Column<byte[]>(nullable: true),
                BluDdsData                       = table.Column<byte[]>(nullable: true),
                BluSaiData                       = table.Column<byte[]>(nullable: true),
                BluDiData                        = table.Column<byte[]>(nullable: true),
                BluPacData                       = table.Column<byte[]>(nullable: true),
                PlextorReadCddaData              = table.Column<byte[]>(nullable: true),
                PioneerReadCddaData              = table.Column<byte[]>(nullable: true),
                PioneerReadCddaMsfData           = table.Column<byte[]>(nullable: true),
                NecReadCddaData                  = table.Column<byte[]>(nullable: true),
                PlextorReadRawDVDData            = table.Column<byte[]>(nullable: true),
                HLDTSTReadRawDVDData             = table.Column<byte[]>(nullable: true),
                AtaId                            = table.Column<int>(nullable: true),
                MmcId                            = table.Column<int>(nullable: true),
                ScsiId                           = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_TestedMedia", x => x.Id);

                table.ForeignKey("FK_TestedMedia_Chs_CHSId", x => x.CHSId, "Chs", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_TestedMedia_Chs_CurrentCHSId", x => x.CurrentCHSId, "Chs", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_TestedMedia_Mmc_MmcId", x => x.MmcId, "Mmc", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(TestedMediasExists)
            {
                string preFormat =
                    @"INSERT INTO TestedMedia (Id, IdentifyData, Blocks, BlockSize, CanReadAACS, CanReadADIP,
                         CanReadATIP, CanReadBCA, CanReadC2Pointers, CanReadCMI, CanReadCorrectedSubchannel,
                         CanReadCorrectedSubchannelWithC2, CanReadDCB, CanReadDDS, CanReadDMI, CanReadDiscInformation,
                         CanReadFullTOC, CanReadHDCMI, CanReadLayerCapacity, CanReadFirstTrackPreGap, CanReadLeadIn,
                         CanReadLeadOut, CanReadMediaID, CanReadMediaSerial, CanReadPAC, CanReadPFI, CanReadPMA, 
                         CanReadPQSubchannel, CanReadPQSubchannelWithC2, CanReadPRI, CanReadRWSubchannel, 
                         CanReadRWSubchannelWithC2, CanReadRecordablePFI, CanReadSpareAreaInformation, CanReadTOC, Density,
                         LongBlockSize, Manufacturer, MediaIsRecognized, MediumType, MediumTypeName, Model, 
                         SupportsHLDTSTReadRawDVD, SupportsNECReadCDDA, SupportsPioneerReadCDDA, SupportsPioneerReadCDDAMSF, 
                         SupportsPlextorReadCDDA, SupportsPlextorReadRawDVD, SupportsRead10, SupportsRead12, SupportsRead16, 
                         SupportsRead6, SupportsReadCapacity16, SupportsReadCapacity, SupportsReadCd, SupportsReadCdMsf, 
                         SupportsReadCdRaw, SupportsReadCdMsfRaw, SupportsReadLong16, SupportsReadLong, ModeSense6Data,
                         ModeSense10Data, CHSId, CurrentCHSId, LBASectors, LBA48Sectors, LogicalAlignment, NominalRotationRate,
                         PhysicalBlockSize, SolidStateDevice, UnformattedBPT, UnformattedBPS, SupportsReadDmaLba, SupportsReadDmaRetryLba,
                         SupportsReadLba, SupportsReadRetryLba, SupportsReadLongLba, SupportsReadLongRetryLba, SupportsSeekLba,
                         SupportsReadDmaLba48, SupportsReadLba48, SupportsReadDma, SupportsReadDmaRetry, SupportsReadRetry,
                         SupportsReadSectors, SupportsReadLongRetry, SupportsSeek, CanReadingIntersessionLeadIn,
                         CanReadingIntersessionLeadOut, IntersessionLeadInData, IntersessionLeadOutData, BlocksSql,
                         BlockSizeSql, LongBlockSizeSql, LBASectorsSql, LBA48SectorsSql, LogicalAlignmentSql, 
                         NominalRotationRateSql, PhysicalBlockSizeSql, UnformattedBPTSql, UnformattedBPSSql, Read6Data,
                         Read10Data, Read12Data, Read16Data, ReadLong10Data, ReadLong16Data, ReadSectorsData, 
                         ReadSectorsRetryData, ReadDmaData, ReadDmaRetryData, ReadLbaData, ReadRetryLbaData, ReadDmaLbaData,
                         ReadDmaRetryLbaData, ReadLba48Data, ReadDmaLba48Data, ReadLongData, ReadLongRetryData,
                         ReadLongLbaData, ReadLongRetryLbaData, TocData, FullTocData, AtipData, PmaData, ReadCdData,
                         ReadCdMsfData, ReadCdFullData, ReadCdMsfFullData, Track1PregapData, LeadInData, LeadOutData,
                         C2PointersData, PQSubchannelData, RWSubchannelData, CorrectedSubchannelData, PQSubchannelWithC2Data,
                         RWSubchannelWithC2Data, CorrectedSubchannelWithC2Data, PfiData, DmiData, CmiData, DvdBcaData,
                         DvdAacsData, DvdDdsData, DvdSaiData, PriData, EmbossedPfiData, AdipData, DcbData, HdCmiData,
                         DvdLayerData, BluBcaData, BluDdsData, BluSaiData, BluDiData, BluPacData, PlextorReadCddaData,
                         PioneerReadCddaData, PioneerReadCddaMsfData, NecReadCddaData, PlextorReadRawDVDData, HLDTSTReadRawDVDData,
                         AtaId, MmcId, ScsiId)
                         SELECT Id, IdentifyData, BlocksSql AS Blocks, BlockSizeSql AS BlockSize, CanReadAACS, CanReadADIP,
                                CanReadATIP, CanReadBCA, CanReadC2Pointers, CanReadCMI, CanReadCorrectedSubchannel, 
                                CanReadCorrectedSubchannelWithC2, CanReadDCB, CanReadDDS, CanReadDMI, CanReadDiscInformation,
                                CanReadFullTOC, CanReadHDCMI, CanReadLayerCapacity, CanReadFirstTrackPreGap, CanReadLeadIn,
                                CanReadLeadOut, CanReadMediaID, CanReadMediaSerial, CanReadPAC, CanReadPFI, CanReadPMA,
                                CanReadPQSubchannel, CanReadPQSubchannelWithC2, CanReadPRI, CanReadRWSubchannel,
                                CanReadRWSubchannelWithC2, CanReadRecordablePFI, CanReadSpareAreaInformation, CanReadTOC,
                                Density, LongBlockSizeSql AS LongBlockSize, Manufacturer, MediaIsRecognized, MediumType,
                                MediumTypeName, Model, SupportsHLDTSTReadRawDVD, SupportsNECReadCDDA, SupportsPioneerReadCDDA,
                                SupportsPioneerReadCDDAMSF, SupportsPlextorReadCDDA, SupportsPlextorReadRawDVD, SupportsRead10,
                                SupportsRead12, SupportsRead16, SupportsRead6, SupportsReadCapacity16, SupportsReadCapacity,
                                SupportsReadCd, SupportsReadCdMsf, SupportsReadCdRaw, SupportsReadCdMsfRaw,
                                SupportsReadLong16, SupportsReadLong, ModeSense6Data, ModeSense10Data, CHS_Id, CurrentCHS_Id,
                                LBASectorsSql AS LBASectors, LBA48SectorsSql AS LBA48Sectors,
                                LogicalAlignmentSql AS LogicalAlignment,NominalRotationRateSql AS NominalRotationRate,
                                PhysicalBlockSizeSql AS PhysicalBlockSize, SolidStateDevice, {0} AS UnformattedBPT,
                                UnformattedBPSSql AS UnformattedBPS, SupportsReadDmaLba, SupportsReadDmaRetryLba, SupportsReadLba,
                                SupportsReadRetryLba, SupportsReadLongLba, SupportsReadLongRetryLba, SupportsSeekLba, 
                                SupportsReadDmaLba48, SupportsReadLba48, SupportsReadDma, SupportsReadDmaRetry, SupportsReadRetry,
                                SupportsReadSectors, SupportsReadLongRetry, SupportsSeek, CanReadingIntersessionLeadIn, 
                                CanReadingIntersessionLeadOut, IntersessionLeadInData, IntersessionLeadOutData, BlocksSql,
                                BlockSizeSql, LongBlockSizeSql, LBASectorsSql, LBA48SectorsSql, LogicalAlignmentSql, 
                                NominalRotationRateSql, PhysicalBlockSizeSql, UnformattedBPTSql, UnformattedBPSSql, 
                                Read6Data, Read10Data, Read12Data, Read16Data, ReadLong10Data, ReadLong16Data, 
                                ReadSectorsData, ReadSectorsRetryData, ReadDmaData, ReadDmaRetryData, ReadLbaData, 
                                ReadRetryLbaData, ReadDmaLbaData, ReadDmaRetryLbaData, ReadLba48Data, ReadDmaLba48Data,
                                ReadLongData, ReadLongRetryData, ReadLongLbaData, ReadLongRetryLbaData, TocData, FullTocData,
                                AtipData, PmaData, ReadCdData, ReadCdMsfData, ReadCdFullData, ReadCdMsfFullData, Track1PregapData,
                                LeadInData, LeadOutData, C2PointersData, PQSubchannelData, RWSubchannelData, CorrectedSubchannelData,
                                PQSubchannelWithC2Data, RWSubchannelWithC2Data, CorrectedSubchannelWithC2Data, PfiData, DmiData,
                                CmiData, DvdBcaData, DvdAacsData, DvdDdsData, DvdSaiData, PriData, EmbossedPfiData, 
                                AdipData, DcbData, HdCmiData, DvdLayerData, BluBcaData, BluDdsData, BluSaiData, BluDiData,
                                BluPacData, PlextorReadCddaData, PioneerReadCddaData, PioneerReadCddaMsfData, NecReadCddaData,
                                PlextorReadRawDVDData, HLDTSTReadRawDVDData, Ata_Id, Mmc_Id, Scsi_Id
                         FROM TestedMedias WHERE
                                (EXISTS(SELECT 1 FROM Atas WHERE Atas.Id = Ata_Id) OR Ata_Id IS NULL) AND
                                (EXISTS(SELECT 1 FROM Scsis WHERE Scsis.Id = SCSI_Id) OR SCSI_Id IS NULL) AND
                                (EXISTS(SELECT 1 FROM Chs WHERE Chs.Id = CHS_Id) OR CHS_Id IS NULL) AND
                                (EXISTS(SELECT 1 FROM Chs WHERE Chs.Id = CurrentCHS_Id)  OR CurrentCHS_Id IS NULL) AND
                                (EXISTS(SELECT 1 FROM Mmc WHERE Mmc.Id = Mmc_Id) OR Mmc_Id IS NULL) AND
                                UnformattedBPTSql {1}";

                migrationBuilder.Sql(string.Format(preFormat, "UnformattedBPTSql", "IS NULL"));
                migrationBuilder.Sql(string.Format(preFormat, "UnformattedBPTSql", ">= 0"));
                migrationBuilder.Sql(string.Format(preFormat, "(65536+UnformattedBPTSql)", "< 0"));

                migrationBuilder.DropTable("TestedMedias");
            }
            #endregion

            #region TABLE: Ata
            migrationBuilder.CreateTable("Ata", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Identify = table.Column<byte[]>(nullable: true), ReadCapabilitiesId = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Ata", x => x.Id);

                table.ForeignKey("FK_Ata_TestedMedia_ReadCapabilitiesId", x => x.ReadCapabilitiesId, "TestedMedia",
                                 "Id", onDelete: ReferentialAction.Restrict);
            });

            if(AtasExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO Ata (Id, Identify, ReadCapabilitiesId) SELECT Id, Identify, ReadCapabilities_Id FROM Atas");

                migrationBuilder.DropTable("Atas");
            }
            #endregion

            #region TABLE: Scsi
            migrationBuilder.CreateTable("Scsi", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                InquiryData               = table.Column<byte[]>(nullable: true),
                SupportsModeSense6        = table.Column<bool>(),
                SupportsModeSense10       = table.Column<bool>(),
                SupportsModeSubpages      = table.Column<bool>(),
                ModeSenseId               = table.Column<int>(nullable: true),
                MultiMediaDeviceId        = table.Column<int>(nullable: true),
                ReadCapabilitiesId        = table.Column<int>(nullable: true),
                SequentialDeviceId        = table.Column<int>(nullable: true),
                ModeSense6Data            = table.Column<byte[]>(nullable: true),
                ModeSense10Data           = table.Column<byte[]>(nullable: true),
                ModeSense6CurrentData     = table.Column<byte[]>(nullable: true),
                ModeSense10CurrentData    = table.Column<byte[]>(nullable: true),
                ModeSense6ChangeableData  = table.Column<byte[]>(nullable: true),
                ModeSense10ChangeableData = table.Column<byte[]>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Scsi", x => x.Id);

                table.ForeignKey("FK_Scsi_ScsiMode_ModeSenseId", x => x.ModeSenseId, "ScsiMode", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Scsi_Mmc_MultiMediaDeviceId", x => x.MultiMediaDeviceId, "Mmc", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Scsi_TestedMedia_ReadCapabilitiesId", x => x.ReadCapabilitiesId, "TestedMedia",
                                 "Id", onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_Scsi_Ssc_SequentialDeviceId", x => x.SequentialDeviceId, "Ssc", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(ScsisExists)
            {
                migrationBuilder.Sql(
                                     "INSERT INTO Scsi (Id, InquiryData, SupportsModeSense6, SupportsModeSense10, SupportsModeSubpages, ModeSenseId, MultiMediaDeviceId, ReadCapabilitiesId, SequentialDeviceId, ModeSense6Data, ModeSense10Data, ModeSense6CurrentData, ModeSense10CurrentData, ModeSense6ChangeableData, ModeSense10ChangeableData) SELECT Id, InquiryData, SupportsModeSense6, SupportsModeSense10, SupportsModeSubpages, ModeSense_Id, MultiMediaDevice_Id, ReadCapabilities_Id, SequentialDevice_Id, ModeSense6Data, ModeSense10Data, ModeSense6CurrentData, ModeSense10CurrentData, ModeSense6ChangeableData, ModeSense10ChangeableData FROM Scsis WHERE EXISTS(SELECT 1 from Mmc WHERE Mmc.Id = Scsis.MultiMediaDevice_Id) OR MultiMediaDevice_Id IS NULL");

                migrationBuilder.DropTable("Scsis");
            }
            #endregion

            #region TABLE: ScsiPage
            migrationBuilder.CreateTable("ScsiPage", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                page       = table.Column<byte>(), subpage                = table.Column<byte>(nullable: true),
                value      = table.Column<byte[]>(nullable: true), ScsiId = table.Column<int>(nullable: true),
                ScsiModeId = table.Column<int>(nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_ScsiPage", x => x.Id);

                table.ForeignKey("FK_ScsiPage_Scsi_ScsiId", x => x.ScsiId, "Scsi", "Id",
                                 onDelete: ReferentialAction.Restrict);

                table.ForeignKey("FK_ScsiPage_ScsiMode_ScsiModeId", x => x.ScsiModeId, "ScsiMode", "Id",
                                 onDelete: ReferentialAction.Restrict);
            });

            if(ScsiPagesExists)
            {
                migrationBuilder.
                    Sql("INSERT INTO ScsiPage (Id, page, subpage, value, ScsiId, ScsiModeId) SELECT Id, page, subpage, value, Scsi_Id, ScsiMode_Id FROM ScsiPages");

                migrationBuilder.DropTable("ScsiPages");
            }
            #endregion

            migrationBuilder.CreateIndex("IX_Ata_ReadCapabilitiesId", "Ata", "ReadCapabilitiesId");

            migrationBuilder.CreateIndex("IX_BlockDescriptor_ScsiModeId", "BlockDescriptor", "ScsiModeId");

            migrationBuilder.CreateIndex("IX_CdOffsets_ModifiedWhen", "CdOffsets", "ModifiedWhen");

            migrationBuilder.CreateIndex("IX_DensityCode_SscSupportedMediaId", "DensityCode", "SscSupportedMediaId");

            migrationBuilder.CreateIndex("IX_Devices_ATAId", "Devices", "ATAId");

            migrationBuilder.CreateIndex("IX_Devices_ATAPIId", "Devices", "ATAPIId");

            migrationBuilder.CreateIndex("IX_Devices_CdOffsetId", "Devices", "CdOffsetId");

            migrationBuilder.CreateIndex("IX_Devices_FireWireId", "Devices", "FireWireId");

            migrationBuilder.CreateIndex("IX_Devices_ModifiedWhen", "Devices", "ModifiedWhen");

            migrationBuilder.CreateIndex("IX_Devices_MultiMediaCardId", "Devices", "MultiMediaCardId");

            migrationBuilder.CreateIndex("IX_Devices_PCMCIAId", "Devices", "PCMCIAId");

            migrationBuilder.CreateIndex("IX_Devices_SCSIId", "Devices", "SCSIId");

            migrationBuilder.CreateIndex("IX_Devices_SecureDigitalId", "Devices", "SecureDigitalId");

            migrationBuilder.CreateIndex("IX_Devices_USBId", "Devices", "USBId");

            migrationBuilder.CreateIndex("IX_DeviceStats_ReportId", "DeviceStats", "ReportId");

            migrationBuilder.CreateIndex("IX_Mmc_FeaturesId", "Mmc", "FeaturesId");

            migrationBuilder.CreateIndex("IX_Reports_ATAId", "Reports", "ATAId");

            migrationBuilder.CreateIndex("IX_Reports_ATAPIId", "Reports", "ATAPIId");

            migrationBuilder.CreateIndex("IX_Reports_FireWireId", "Reports", "FireWireId");

            migrationBuilder.CreateIndex("IX_Reports_MultiMediaCardId", "Reports", "MultiMediaCardId");

            migrationBuilder.CreateIndex("IX_Reports_PCMCIAId", "Reports", "PCMCIAId");

            migrationBuilder.CreateIndex("IX_Reports_SCSIId", "Reports", "SCSIId");

            migrationBuilder.CreateIndex("IX_Reports_SecureDigitalId", "Reports", "SecureDigitalId");

            migrationBuilder.CreateIndex("IX_Reports_USBId", "Reports", "USBId");

            migrationBuilder.CreateIndex("IX_Scsi_ModeSenseId", "Scsi", "ModeSenseId");

            migrationBuilder.CreateIndex("IX_Scsi_MultiMediaDeviceId", "Scsi", "MultiMediaDeviceId");

            migrationBuilder.CreateIndex("IX_Scsi_ReadCapabilitiesId", "Scsi", "ReadCapabilitiesId");

            migrationBuilder.CreateIndex("IX_Scsi_SequentialDeviceId", "Scsi", "SequentialDeviceId");

            migrationBuilder.CreateIndex("IX_ScsiPage_ScsiId", "ScsiPage", "ScsiId");

            migrationBuilder.CreateIndex("IX_ScsiPage_ScsiModeId", "ScsiPage", "ScsiModeId");

            migrationBuilder.CreateIndex("IX_SscSupportedMedia_SscId", "SscSupportedMedia", "SscId");

            migrationBuilder.CreateIndex("IX_SscSupportedMedia_TestedSequentialMediaId", "SscSupportedMedia",
                                         "TestedSequentialMediaId");

            migrationBuilder.CreateIndex("IX_SupportedDensity_SscId", "SupportedDensity", "SscId");

            migrationBuilder.CreateIndex("IX_SupportedDensity_TestedSequentialMediaId", "SupportedDensity",
                                         "TestedSequentialMediaId");

            migrationBuilder.CreateIndex("IX_TestedMedia_AtaId", "TestedMedia", "AtaId");

            migrationBuilder.CreateIndex("IX_TestedMedia_CHSId", "TestedMedia", "CHSId");

            migrationBuilder.CreateIndex("IX_TestedMedia_CurrentCHSId", "TestedMedia", "CurrentCHSId");

            migrationBuilder.CreateIndex("IX_TestedMedia_MmcId", "TestedMedia", "MmcId");

            migrationBuilder.CreateIndex("IX_TestedMedia_ScsiId", "TestedMedia", "ScsiId");

            migrationBuilder.CreateIndex("IX_TestedSequentialMedia_SscId", "TestedSequentialMedia", "SscId");

            migrationBuilder.CreateIndex("IX_UsbProducts_ModifiedWhen", "UsbProducts", "ModifiedWhen");

            migrationBuilder.CreateIndex("IX_UsbProducts_ProductId", "UsbProducts", "ProductId");

            migrationBuilder.CreateIndex("IX_UsbProducts_VendorId", "UsbProducts", "VendorId");

            migrationBuilder.CreateIndex("IX_UsbVendors_ModifiedWhen", "UsbVendors", "ModifiedWhen");

            migrationBuilder.CreateIndex("IX_UsbVendors_VendorId", "UsbVendors", "VendorId", unique: true);

            migrationBuilder.AddForeignKey("FK_Devices_Ata_ATAId", "Devices", "ATAId", "Ata", principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey("FK_Devices_Ata_ATAPIId", "Devices", "ATAPIId", "Ata", principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey("FK_Devices_Scsi_SCSIId", "Devices", "SCSIId", "Scsi", principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey("FK_Reports_Ata_ATAId", "Reports", "ATAId", "Ata", principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey("FK_Reports_Ata_ATAPIId", "Reports", "ATAPIId", "Ata", principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);

            migrationBuilder.
                Sql("DELETE FROM Reports WHERE NOT EXISTS(SELECT 1 from Scsi WHERE Scsi.Id = SCSIId) AND SCSIId IS NOT NULL");

            migrationBuilder.AddForeignKey("FK_Reports_Scsi_SCSIId", "Reports", "SCSIId", "Scsi", principalColumn: "Id",
                                           onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey("FK_TestedMedia_Ata_AtaId", "TestedMedia", "AtaId", "Ata",
                                           principalColumn: "Id", onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey("FK_TestedMedia_Scsi_ScsiId", "TestedMedia", "ScsiId", "Scsi",
                                           principalColumn: "Id", onDelete: ReferentialAction.Restrict);

            if(EFExists)
                migrationBuilder.DropTable("__MigrationHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_Ata_TestedMedia_ReadCapabilitiesId", "Ata");

            migrationBuilder.DropForeignKey("FK_Scsi_TestedMedia_ReadCapabilitiesId", "Scsi");

            migrationBuilder.DropTable("BlockDescriptor");

            migrationBuilder.DropTable("Commands");

            migrationBuilder.DropTable("DensityCode");

            migrationBuilder.DropTable("DeviceStats");

            migrationBuilder.DropTable("Filesystems");

            migrationBuilder.DropTable("Filters");

            migrationBuilder.DropTable("MediaFormats");

            migrationBuilder.DropTable("Medias");

            migrationBuilder.DropTable("OperatingSystems");

            migrationBuilder.DropTable("Partitions");

            migrationBuilder.DropTable("Reports");

            migrationBuilder.DropTable("ScsiPage");

            migrationBuilder.DropTable("SupportedDensity");

            migrationBuilder.DropTable("UsbProducts");

            migrationBuilder.DropTable("Versions");

            migrationBuilder.DropTable("SscSupportedMedia");

            migrationBuilder.DropTable("Devices");

            migrationBuilder.DropTable("UsbVendors");

            migrationBuilder.DropTable("TestedSequentialMedia");

            migrationBuilder.DropTable("CdOffsets");

            migrationBuilder.DropTable("FireWire");

            migrationBuilder.DropTable("MmcSd");

            migrationBuilder.DropTable("Pcmcia");

            migrationBuilder.DropTable("Usb");

            migrationBuilder.DropTable("TestedMedia");

            migrationBuilder.DropTable("Ata");

            migrationBuilder.DropTable("Chs");

            migrationBuilder.DropTable("Scsi");

            migrationBuilder.DropTable("ScsiMode");

            migrationBuilder.DropTable("Mmc");

            migrationBuilder.DropTable("Ssc");

            migrationBuilder.DropTable("MmcFeatures");
        }
    }
}