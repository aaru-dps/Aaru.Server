using System;
using DiscImageChef.Server.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscImageChef.Server.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Check for old tables
            var AtasExists = DicServerContext.TableExists("Atas");
            var BlockDescriptorsExists = DicServerContext.TableExists("BlockDescriptors");
            var ChsExists = DicServerContext.TableExists("Chs");
            var CommandsExists = DicServerContext.TableExists("Commands");
            var CompactDiscOffsetsExists = DicServerContext.TableExists("CompactDiscOffsets");
            var DensityCodesExists = DicServerContext.TableExists("DensityCodes");
            var DevicesExists = DicServerContext.TableExists("Devices");
            var DeviceStatsExists = DicServerContext.TableExists("DeviceStats");
            var FilesystemsExists = DicServerContext.TableExists("Filesystems");
            var FiltersExists = DicServerContext.TableExists("Filters");
            var FireWiresExists = DicServerContext.TableExists("FireWires");
            var MediaExists = DicServerContext.TableExists("Media");
            var MediaFormatsExists = DicServerContext.TableExists("MediaFormats");
            var MmcFeaturesExists = DicServerContext.TableExists("MmcFeatures");
            var MmcsExists = DicServerContext.TableExists("Mmcs");
            var MmcSdsExists = DicServerContext.TableExists("MmcSds");
            var OperatingSystemsExists = DicServerContext.TableExists("OperatingSystems");
            var PartitionsExists = DicServerContext.TableExists("Partitions");
            var PcmciasExists = DicServerContext.TableExists("Pcmcias");
            var ScsiModesExists = DicServerContext.TableExists("ScsiModes");
            var ScsiPagesExists = DicServerContext.TableExists("ScsiPages");
            var ScsisExists = DicServerContext.TableExists("Scsis");
            var SscsExists = DicServerContext.TableExists("Sscs");
            var SscSupportedMediasExists = DicServerContext.TableExists("SscSupportedMedias");
            var SupportedDensitiesExists = DicServerContext.TableExists("SupportedDensities");
            var TestedMediasExists = DicServerContext.TableExists("TestedMedias");
            var TestedSequentialMediasExists = DicServerContext.TableExists("TestedSequentialMedias");
            var UploadedReportsExists = DicServerContext.TableExists("UploadedReports");
            var UsbProductsExists = DicServerContext.TableExists("UsbProducts");
            var UsbsExists = DicServerContext.TableExists("Usbs");
            var UsbVendorsExists = DicServerContext.TableExists("UsbVendors");
            var VersionsExists = DicServerContext.TableExists("Versions");
            var EFExists = DicServerContext.TableExists("__MigrationHistory");
            #endregion

            #region Drop old restrictions
            if(AtasExists)
                migrationBuilder.DropForeignKey("FK_Atas_TestedMedias_ReadCapabilities_Id", "Atas");
            if (BlockDescriptorsExists)
                migrationBuilder.DropForeignKey("FK_BlockDescriptors_ScsiModes_ScsiMode_Id", "BlockDescriptors");
            if (DensityCodesExists)
                migrationBuilder.DropForeignKey(
                    "FK_DensityCodes_SscSupportedMedias_SscSupportedMedia_Id", "DensityCodes");
            if (DevicesExists)
            {
                migrationBuilder.DropForeignKey(
                    "FK_Devices_Atas_ATA_Id", "Devices");
                migrationBuilder.DropForeignKey(
                    "FK_Devices_Atas_ATAPI_Id", "Devices");
                migrationBuilder.DropForeignKey(
                    "FK_Devices_CompactDiscOffsets_CdOffset_Id", "Devices");
                migrationBuilder.DropForeignKey(
                    "FK_Devices_FireWires_FireWire_Id", "Devices");
                migrationBuilder.DropForeignKey(
                    "FK_Devices_MmcSds_MultiMediaCard_Id", "Devices");
                migrationBuilder.DropForeignKey(
                    "FK_Devices_MmcSds_SecureDigital_Id", "Devices");
                migrationBuilder.DropForeignKey(
                    "FK_Devices_Pcmcias_PCMCIA_Id", "Devices");
                migrationBuilder.DropForeignKey(
                    "FK_Devices_Scsis_SCSI_Id", "Devices");
                migrationBuilder.DropForeignKey(
                    "FK_Devices_Usbs_USB_Id", "Devices");
            }
            if (DeviceStatsExists)
                migrationBuilder.DropForeignKey(
                    "FK_DeviceStats_Devices_Report_Id", "DeviceStats");
            if (MmcsExists)
                migrationBuilder.DropForeignKey(
                    "FK_Mmcs_MmcFeatures_Features_Id", "Mmcs");
            if (ScsiPagesExists)
            {
                migrationBuilder.DropForeignKey(
                    "FK_ScsiPages_ScsiModes_ScsiMode_Id","ScsiPages");
                migrationBuilder.DropForeignKey(
                    "FK_ScsiPages_Scsis_Scsi_Id","ScsiPages");
            }
            if (ScsisExists)
            {
                migrationBuilder.DropForeignKey(
                    "FK_Scsis_Mmcs_MultiMediaDevice_Id","Scsis");
                migrationBuilder.DropForeignKey(
                    "FK_Scsis_ScsiModes_ModeSense_Id","Scsis");
                migrationBuilder.DropForeignKey(
                    "FK_Scsis_Sscs_SequentialDevice_Id","Scsis");
                migrationBuilder.DropForeignKey(
                    "FK_Scsis_TestedMedias_ReadCapabilities_Id","Scsis");
            }
            if (SscSupportedMediasExists)
            {
                migrationBuilder.DropForeignKey(
                    "FK_a812ec60296b45bcb3d245a5c6d01d73","SscSupportedMedias");
                migrationBuilder.DropForeignKey(
                    "FK_SscSupportedMedias_Sscs_Ssc_Id","SscSupportedMedias");
            }
            if (SupportedDensitiesExists)
            {
                migrationBuilder.DropForeignKey(
                    "FK_783f1b3552774280af1caf44fb27e285","SupportedDensities");
                migrationBuilder.DropForeignKey(
                    "FK_SupportedDensities_Sscs_Ssc_Id","SupportedDensities");
            }
            if (TestedMediasExists)
            {
                migrationBuilder.DropForeignKey(
                    "FK_TestedMedias_Atas_Ata_Id","TestedMedias");
                migrationBuilder.DropForeignKey(
                    "FK_TestedMedias_Chs_CHS_Id","TestedMedias");
                migrationBuilder.DropForeignKey(
                    "FK_TestedMedias_Chs_CurrentCHS_Id","TestedMedias");
                migrationBuilder.DropForeignKey(
                    "FK_TestedMedias_Mmcs_Mmc_Id","TestedMedias");
                migrationBuilder.DropForeignKey(
                    "FK_TestedMedias_Scsis_Scsi_Id","TestedMedias");
            }
            if (TestedSequentialMediasExists)
                migrationBuilder.DropForeignKey(
                    "FK_TestedSequentialMedias_Sscs_Ssc_Id","TestedSequentialMedias");
            if (UploadedReportsExists)
            {
                migrationBuilder.DropForeignKey(
                    "FK_UploadedReports_Atas_ATA_Id","UploadedReports");
                migrationBuilder.DropForeignKey(
                    "FK_UploadedReports_Atas_ATAPI_Id","UploadedReports");
                migrationBuilder.DropForeignKey(
                    "FK_UploadedReports_FireWires_FireWire_Id","UploadedReports");
                migrationBuilder.DropForeignKey(
                    "FK_UploadedReports_MmcSds_MultiMediaCard_Id","UploadedReports");
                migrationBuilder.DropForeignKey(
                    "FK_UploadedReports_MmcSds_SecureDigital_Id","UploadedReports");
                migrationBuilder.DropForeignKey(
                    "FK_UploadedReports_Pcmcias_PCMCIA_Id","UploadedReports");
                migrationBuilder.DropForeignKey(
                    "FK_UploadedReports_Scsis_SCSI_Id","UploadedReports");
                migrationBuilder.DropForeignKey(
                    "FK_UploadedReports_Usbs_USB_Id","UploadedReports");
            }
            if (UsbProductsExists)
                migrationBuilder.DropForeignKey(
                    "FK_UsbProducts_UsbVendors_VendorId","UsbProducts");
            #endregion

            #region TABLE: CdOffsets
            migrationBuilder.CreateTable(
                name: "CdOffsets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Offset = table.Column<short>(nullable: false),
                    Submissions = table.Column<int>(nullable: false),
                    Agreement = table.Column<float>(nullable: false),
                    AddedWhen = table.Column<DateTime>(nullable: false),
                    ModifiedWhen = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CdOffsets", x => x.Id);
                });

            if (CompactDiscOffsetsExists)
            {
                migrationBuilder.Sql("INSERT INTO CdOffsets (Id, Manufacturer, Model, Offset, Submissions, Agreement, AddedWhen, ModifiedWhen) SELECT Id, Manufacturer, Model, Offset, Submissions, Agreement, AddedWhen, ModifiedWhen FROM CompactDiscOffsets");
                migrationBuilder.DropTable("CompactDiscOffsets");
            }
            #endregion

            #region TABLE: Chs
            if (ChsExists)
                migrationBuilder.RenameTable(name: "Chs", newName: "Chs_old");

            migrationBuilder.CreateTable(
                name: "Chs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cylinders = table.Column<ushort>(nullable: false),
                    Heads = table.Column<ushort>(nullable: false),
                    Sectors = table.Column<ushort>(nullable: false),
                    CylindersSql = table.Column<short>(nullable: false),
                    HeadsSql = table.Column<short>(nullable: false),
                    SectorsSql = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chs", x => x.Id);
                });

            if (ChsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Chs (Id, Cylinders, Heads, Sectors, CylindersSql, HeadsSql, SectorsSql) SELECT Id, CylindersSql AS Cylinders, HeadsSql AS Heads, SectorsSql AS Sectors, CylindersSql, HeadsSql, SectorsSql FROM Chs_old");
                migrationBuilder.DropTable("Chs_old");
            }
            #endregion

            #region TABLE: Commands
            if (CommandsExists)
                migrationBuilder.RenameTable(name: "Commands", newName: "Commands_old");

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                });

            if (CommandsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Commands (Id, Name, Count) SELECT Id, Name, Count FROM Commands_old");
                migrationBuilder.DropTable("Commands_old");
            }
            #endregion

            #region TABLE: Filesystems
            if (FilesystemsExists)
                migrationBuilder.RenameTable(name: "Filesystems", newName: "Filesystems_old");

            migrationBuilder.CreateTable(
                name: "Filesystems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filesystems", x => x.Id);
                });

            if (FilesystemsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Filesystems (Id, Name, Count) SELECT Id, Name, Count FROM Filesystems_old");
                migrationBuilder.DropTable("Filesystems_old");
            }
            #endregion

            #region TABLE: Filters
            if (FiltersExists)
                migrationBuilder.RenameTable(name: "Filters", newName: "Filters_old");

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.Id);
                });

            if (FiltersExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Filters (Id, Name, Count) SELECT Id, Name, Count FROM Filters_old");
                migrationBuilder.DropTable("Filters_old");
            }
            #endregion

            #region TABLE: FireWire
            migrationBuilder.CreateTable(
                name: "FireWire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VendorID = table.Column<uint>(nullable: false),
                    ProductID = table.Column<uint>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: true),
                    Product = table.Column<string>(nullable: true),
                    RemovableMedia = table.Column<bool>(nullable: false),
                    VendorIDSql = table.Column<int>(nullable: false),
                    ProductIDSql = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireWire", x => x.Id);
                });

            if (FireWiresExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO FireWire (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, VendorIDSql, ProductIDSql) SELECT Id, VendorIDSql AS VendorID, ProductIDSql AS ProductID, Manufacturer, Product, RemovableMedia, VendorIDSql, ProductIDSql FROM FireWires");
                migrationBuilder.DropTable("FireWires");
            }
            #endregion

            #region TABLE: MediaFormats
            if (MediaFormatsExists)
                migrationBuilder.RenameTable(name: "MediaFormats", newName: "MediaFormats_old");

            migrationBuilder.CreateTable(
                name: "MediaFormats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFormats", x => x.Id);
                });

            if (MediaFormatsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO MediaFormats (Id, Name, Count) SELECT Id, Name, Count FROM MediaFormats_old");
                migrationBuilder.DropTable("MediaFormats_old");
            }
            #endregion

            #region TABLE: Medias
            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true),
                    Real = table.Column<bool>(nullable: false),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                });

            if (MediaExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Medias (`Id`, `Type`, `Real`, `Count`) SELECT `Id`, `Type`, `Real`, `Count` FROM Media");
                migrationBuilder.DropTable("Media");
            }
            #endregion

            #region TABLE: MmcFeatures
            if (MmcFeaturesExists)
                migrationBuilder.RenameTable(name: "MmcFeatures", newName: "MmcFeatures_old");

            migrationBuilder.CreateTable(
                name: "MmcFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AACSVersion = table.Column<byte>(nullable: true),
                    AGIDs = table.Column<byte>(nullable: true),
                    BindingNonceBlocks = table.Column<byte>(nullable: true),
                    BlocksPerReadableUnit = table.Column<ushort>(nullable: true),
                    BufferUnderrunFreeInDVD = table.Column<bool>(nullable: false),
                    BufferUnderrunFreeInSAO = table.Column<bool>(nullable: false),
                    BufferUnderrunFreeInTAO = table.Column<bool>(nullable: false),
                    CanAudioScan = table.Column<bool>(nullable: false),
                    CanEject = table.Column<bool>(nullable: false),
                    CanEraseSector = table.Column<bool>(nullable: false),
                    CanExpandBDRESpareArea = table.Column<bool>(nullable: false),
                    CanFormat = table.Column<bool>(nullable: false),
                    CanFormatBDREWithoutSpare = table.Column<bool>(nullable: false),
                    CanFormatCert = table.Column<bool>(nullable: false),
                    CanFormatFRF = table.Column<bool>(nullable: false),
                    CanFormatQCert = table.Column<bool>(nullable: false),
                    CanFormatRRM = table.Column<bool>(nullable: false),
                    CanGenerateBindingNonce = table.Column<bool>(nullable: false),
                    CanLoad = table.Column<bool>(nullable: false),
                    CanMuteSeparateChannels = table.Column<bool>(nullable: false),
                    CanOverwriteSAOTrack = table.Column<bool>(nullable: false),
                    CanOverwriteTAOTrack = table.Column<bool>(nullable: false),
                    CanPlayCDAudio = table.Column<bool>(nullable: false),
                    CanPseudoOverwriteBDR = table.Column<bool>(nullable: false),
                    CanReadAllDualR = table.Column<bool>(nullable: false),
                    CanReadAllDualRW = table.Column<bool>(nullable: false),
                    CanReadBD = table.Column<bool>(nullable: false),
                    CanReadBDR = table.Column<bool>(nullable: false),
                    CanReadBDRE1 = table.Column<bool>(nullable: false),
                    CanReadBDRE2 = table.Column<bool>(nullable: false),
                    CanReadBDROM = table.Column<bool>(nullable: false),
                    CanReadBluBCA = table.Column<bool>(nullable: false),
                    CanReadCD = table.Column<bool>(nullable: false),
                    CanReadCDMRW = table.Column<bool>(nullable: false),
                    CanReadCPRM_MKB = table.Column<bool>(nullable: false),
                    CanReadDDCD = table.Column<bool>(nullable: false),
                    CanReadDVD = table.Column<bool>(nullable: false),
                    CanReadDVDPlusMRW = table.Column<bool>(nullable: false),
                    CanReadDVDPlusR = table.Column<bool>(nullable: false),
                    CanReadDVDPlusRDL = table.Column<bool>(nullable: false),
                    CanReadDVDPlusRW = table.Column<bool>(nullable: false),
                    CanReadDVDPlusRWDL = table.Column<bool>(nullable: false),
                    CanReadDriveAACSCertificate = table.Column<bool>(nullable: false),
                    CanReadHDDVD = table.Column<bool>(nullable: false),
                    CanReadHDDVDR = table.Column<bool>(nullable: false),
                    CanReadHDDVDRAM = table.Column<bool>(nullable: false),
                    CanReadLeadInCDText = table.Column<bool>(nullable: false),
                    CanReadOldBDR = table.Column<bool>(nullable: false),
                    CanReadOldBDRE = table.Column<bool>(nullable: false),
                    CanReadOldBDROM = table.Column<bool>(nullable: false),
                    CanReadSpareAreaInformation = table.Column<bool>(nullable: false),
                    CanReportDriveSerial = table.Column<bool>(nullable: false),
                    CanReportMediaSerial = table.Column<bool>(nullable: false),
                    CanTestWriteDDCDR = table.Column<bool>(nullable: false),
                    CanTestWriteDVD = table.Column<bool>(nullable: false),
                    CanTestWriteInSAO = table.Column<bool>(nullable: false),
                    CanTestWriteInTAO = table.Column<bool>(nullable: false),
                    CanUpgradeFirmware = table.Column<bool>(nullable: false),
                    CanWriteBD = table.Column<bool>(nullable: false),
                    CanWriteBDR = table.Column<bool>(nullable: false),
                    CanWriteBDRE1 = table.Column<bool>(nullable: false),
                    CanWriteBDRE2 = table.Column<bool>(nullable: false),
                    CanWriteBusEncryptedBlocks = table.Column<bool>(nullable: false),
                    CanWriteCDMRW = table.Column<bool>(nullable: false),
                    CanWriteCDRW = table.Column<bool>(nullable: false),
                    CanWriteCDRWCAV = table.Column<bool>(nullable: false),
                    CanWriteCDSAO = table.Column<bool>(nullable: false),
                    CanWriteCDTAO = table.Column<bool>(nullable: false),
                    CanWriteCSSManagedDVD = table.Column<bool>(nullable: false),
                    CanWriteDDCDR = table.Column<bool>(nullable: false),
                    CanWriteDDCDRW = table.Column<bool>(nullable: false),
                    CanWriteDVDPlusMRW = table.Column<bool>(nullable: false),
                    CanWriteDVDPlusR = table.Column<bool>(nullable: false),
                    CanWriteDVDPlusRDL = table.Column<bool>(nullable: false),
                    CanWriteDVDPlusRW = table.Column<bool>(nullable: false),
                    CanWriteDVDPlusRWDL = table.Column<bool>(nullable: false),
                    CanWriteDVDR = table.Column<bool>(nullable: false),
                    CanWriteDVDRDL = table.Column<bool>(nullable: false),
                    CanWriteDVDRW = table.Column<bool>(nullable: false),
                    CanWriteHDDVDR = table.Column<bool>(nullable: false),
                    CanWriteHDDVDRAM = table.Column<bool>(nullable: false),
                    CanWriteOldBDR = table.Column<bool>(nullable: false),
                    CanWriteOldBDRE = table.Column<bool>(nullable: false),
                    CanWritePackedSubchannelInTAO = table.Column<bool>(nullable: false),
                    CanWriteRWSubchannelInSAO = table.Column<bool>(nullable: false),
                    CanWriteRWSubchannelInTAO = table.Column<bool>(nullable: false),
                    CanWriteRaw = table.Column<bool>(nullable: false),
                    CanWriteRawMultiSession = table.Column<bool>(nullable: false),
                    CanWriteRawSubchannelInTAO = table.Column<bool>(nullable: false),
                    ChangerIsSideChangeCapable = table.Column<bool>(nullable: false),
                    ChangerSlots = table.Column<byte>(nullable: false),
                    ChangerSupportsDiscPresent = table.Column<bool>(nullable: false),
                    CPRMVersion = table.Column<byte>(nullable: true),
                    CSSVersion = table.Column<byte>(nullable: true),
                    DBML = table.Column<bool>(nullable: false),
                    DVDMultiRead = table.Column<bool>(nullable: false),
                    EmbeddedChanger = table.Column<bool>(nullable: false),
                    ErrorRecoveryPage = table.Column<bool>(nullable: false),
                    FirmwareDate = table.Column<DateTime>(nullable: true),
                    LoadingMechanismType = table.Column<byte>(nullable: true),
                    Locked = table.Column<bool>(nullable: false),
                    LogicalBlockSize = table.Column<uint>(nullable: true),
                    MultiRead = table.Column<bool>(nullable: false),
                    PhysicalInterfaceStandardNumber = table.Column<uint>(nullable: true),
                    PreventJumper = table.Column<bool>(nullable: false),
                    SupportsAACS = table.Column<bool>(nullable: false),
                    SupportsBusEncryption = table.Column<bool>(nullable: false),
                    SupportsC2 = table.Column<bool>(nullable: false),
                    SupportsCPRM = table.Column<bool>(nullable: false),
                    SupportsCSS = table.Column<bool>(nullable: false),
                    SupportsDAP = table.Column<bool>(nullable: false),
                    SupportsDeviceBusyEvent = table.Column<bool>(nullable: false),
                    SupportsHybridDiscs = table.Column<bool>(nullable: false),
                    SupportsModePage1Ch = table.Column<bool>(nullable: false),
                    SupportsOSSC = table.Column<bool>(nullable: false),
                    SupportsPWP = table.Column<bool>(nullable: false),
                    SupportsSWPP = table.Column<bool>(nullable: false),
                    SupportsSecurDisc = table.Column<bool>(nullable: false),
                    SupportsSeparateVolume = table.Column<bool>(nullable: false),
                    SupportsVCPS = table.Column<bool>(nullable: false),
                    SupportsWriteInhibitDCB = table.Column<bool>(nullable: false),
                    SupportsWriteProtectPAC = table.Column<bool>(nullable: false),
                    VolumeLevels = table.Column<ushort>(nullable: true),
                    BinaryData = table.Column<byte[]>(nullable: true),
                    BlocksPerReadableUnitSql = table.Column<short>(nullable: true),
                    LogicalBlockSizeSql = table.Column<int>(nullable: true),
                    PhysicalInterfaceStandardNumberSql = table.Column<int>(nullable: true),
                    VolumeLevelsSql = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MmcFeatures", x => x.Id);
                });

            if (MmcFeaturesExists)
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
            migrationBuilder.CreateTable(
                name: "MmcSd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CID = table.Column<byte[]>(nullable: true),
                    CSD = table.Column<byte[]>(nullable: true),
                    OCR = table.Column<byte[]>(nullable: true),
                    SCR = table.Column<byte[]>(nullable: true),
                    ExtendedCSD = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MmcSd", x => x.Id);
                });

            if (MmcSdsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO MmcSd (Id, CID, CSD, OCR, SCR, ExtendedCSD) SELECT Id, CID, CSD, OCR, SCR, ExtendedCSD FROM MmcSds");
                migrationBuilder.DropTable("MmcSds");
            }
            #endregion

            #region TABLE: OperatingSystems
            if (OperatingSystemsExists)
                migrationBuilder.RenameTable(name: "OperatingSystems", newName: "OperatingSystems_old");

            migrationBuilder.CreateTable(
                name: "OperatingSystems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingSystems", x => x.Id);
                });

            if (OperatingSystemsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO OperatingSystems (Id, Name, Version, Count) SELECT Id, Name, Version, Count FROM OperatingSystems_old");
                migrationBuilder.DropTable("OperatingSystems_old");
            }
            #endregion

            #region TABLE: Partitions
            if (PartitionsExists)
                migrationBuilder.RenameTable(name: "Partitions", newName: "Partitions_old");

            migrationBuilder.CreateTable(
                name: "Partitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partitions", x => x.Id);
                });

            if (PartitionsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Partitions (Id, Name, Count) SELECT Id, Name, Count FROM Partitions_old");
                migrationBuilder.DropTable("Partitions_old");
            }
            #endregion

            #region TABLE: Pcmcia
            migrationBuilder.CreateTable(
                name: "Pcmcia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CIS = table.Column<byte[]>(nullable: true),
                    Compliance = table.Column<string>(nullable: true),
                    ManufacturerCode = table.Column<ushort>(nullable: true),
                    CardCode = table.Column<ushort>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ManufacturerCodeSql = table.Column<short>(nullable: true),
                    CardCodeSql = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pcmcia", x => x.Id);
                });

            if (PcmciasExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Pcmcia (Id, CIS, Compliance, ManufacturerCode, CardCode, Manufacturer, ProductName, ManufacturerCodeSql, CardCodeSql) SELECT Id, CIS, Compliance, ManufacturerCodeSql AS ManufacturerCode, CardCodeSql AS CardCode, Manufacturer, ProductName, ManufacturerCodeSql, CardCodeSql FROM Pcmcias");
                migrationBuilder.DropTable("Pcmcias");
            }
            #endregion

            #region TABLE: ScsiMode
            migrationBuilder.CreateTable(
                name: "ScsiMode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MediumType = table.Column<byte>(nullable: true),
                    WriteProtected = table.Column<bool>(nullable: false),
                    Speed = table.Column<byte>(nullable: true),
                    BufferedMode = table.Column<byte>(nullable: true),
                    BlankCheckEnabled = table.Column<bool>(nullable: false),
                    DPOandFUA = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScsiMode", x => x.Id);
                });

            if (ScsiModesExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO ScsiMode (Id, MediumType, WriteProtected, Speed, BufferedMode, BlankCheckEnabled, DPOandFUA) SELECT Id, MediumType, WriteProtected, Speed, BufferedMode, BlankCheckEnabled, DPOandFUA FROM ScsiModes");
                migrationBuilder.DropTable("ScsiModes");
            }
            #endregion

            #region TABLE: Ssc
            migrationBuilder.CreateTable(
                name: "Ssc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BlockSizeGranularity = table.Column<byte>(nullable: true),
                    MaxBlockLength = table.Column<uint>(nullable: true),
                    MinBlockLength = table.Column<uint>(nullable: true),
                    MaxBlockLengthSql = table.Column<int>(nullable: true),
                    MinBlockLengthSql = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ssc", x => x.Id);
                });

            if (SscsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Ssc (Id, BlockSizeGranularity, MaxBlockLength, MinBlockLength, MaxBlockLengthSql, MinBlockLengthSql) SELECT Id, BlockSizeGranularity, MaxBlockLengthSql AS MaxBlockLength, MinBlockLengthSql AS MinBlockLength, MaxBlockLengthSql, MinBlockLengthSql FROM Sscs");
                migrationBuilder.DropTable("Sscs");
            }
            #endregion

            #region TABLE: Usb
            migrationBuilder.CreateTable(
                name: "Usb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VendorID = table.Column<ushort>(nullable: false),
                    ProductID = table.Column<ushort>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: true),
                    Product = table.Column<string>(nullable: true),
                    RemovableMedia = table.Column<bool>(nullable: false),
                    Descriptors = table.Column<byte[]>(nullable: true),
                    VendorIDSql = table.Column<short>(nullable: false),
                    ProductIDSql = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usb", x => x.Id);
                });

            if (UsbsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Usb (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql) SELECT Id, VendorIDSql AS VendorID, ProductIDSql AS ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql FROM Usbs WHERE VendorIDSql >= 0 AND ProductIDSql >= 0");
                migrationBuilder.Sql(
                    "INSERT INTO Usb (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql) SELECT Id, (65536+VendorIDSql) AS VendorID, ProductIDSql AS ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql FROM Usbs WHERE VendorIDSql < 0 AND ProductIDSql >= 0");
                migrationBuilder.Sql(
                    "INSERT INTO Usb (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql) SELECT Id, VendorIDSql AS VendorID, (65536+ProductIDSql) AS ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql FROM Usbs WHERE VendorIDSql >= 0 AND ProductIDSql < 0");
                migrationBuilder.Sql(
                    "INSERT INTO Usb (Id, VendorID, ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql) SELECT Id, (65536+VendorIDSql) AS VendorID, (65536+ProductIDSql) AS ProductID, Manufacturer, Product, RemovableMedia, Descriptors, VendorIDSql, ProductIDSql FROM Usbs WHERE VendorIDSql < 0 AND ProductIDSql < 0");
                migrationBuilder.DropTable("Usbs");
            }
            #endregion

            #region TABLE: UsbVendors

            if (UsbVendorsExists)
                migrationBuilder.RenameTable(name: "UsbVendors", newName: "UsbVendors_old");

            migrationBuilder.CreateTable(
                name: "UsbVendors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VendorId = table.Column<int>(nullable: false),
                    Vendor = table.Column<string>(nullable: true),
                    AddedWhen = table.Column<DateTime>(nullable: false),
                    ModifiedWhen = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsbVendors", x => x.Id);
                });

            if (UsbVendorsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO UsbVendors (Id, VendorId, Vendor, AddedWhen, ModifiedWhen) SELECT Id, VendorId, Vendor, AddedWhen, ModifiedWhen FROM UsbVendors_old");
                migrationBuilder.DropTable("UsbVendors_old");
            }
            #endregion

            #region TABLE: Versions

            if (VersionsExists)
                migrationBuilder.RenameTable(name: "Versions", newName: "Versions_old");

            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => x.Id);
                });

            if (VersionsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Versions (`Id`, `Value`, `Count`) SELECT `Id`, `Value`, `Count` FROM Versions_old");
                migrationBuilder.DropTable("Versions_old");
            }
            #endregion

            #region TABLE: Mmc
            migrationBuilder.CreateTable(
                name: "Mmc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FeaturesId = table.Column<int>(nullable: true),
                    ModeSense2AData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mmc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mmc_MmcFeatures_FeaturesId",
                        column: x => x.FeaturesId,
                        principalTable: "MmcFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (MmcsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Mmc (Id, FeaturesId, ModeSense2AData) SELECT Id, Features_Id, ModeSense2AData FROM Mmcs WHERE EXISTS(SELECT 1 FROM MmcFeatures WHERE MmcFeatures.Id = Features_Id) OR Features_Id IS NULL");
                migrationBuilder.DropTable("Mmcs");
            }
            #endregion

            #region TABLE: BlockDescriptor
            migrationBuilder.CreateTable(
                name: "BlockDescriptor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Density = table.Column<byte>(nullable: false),
                    Blocks = table.Column<ulong>(nullable: true),
                    BlockLength = table.Column<uint>(nullable: true),
                    BlocksSql = table.Column<long>(nullable: true),
                    BlockLengthSql = table.Column<int>(nullable: true),
                    ScsiModeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockDescriptor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockDescriptor_ScsiMode_ScsiModeId",
                        column: x => x.ScsiModeId,
                        principalTable: "ScsiMode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (BlockDescriptorsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO BlockDescriptor (Id, Density, Blocks, BlocksSql, BlockLength, BlockLengthSql, ScsiModeId) SELECT Id, Density, BlocksSql AS Blocks, BlocksSql, BlockLengthSql AS BlockLength, BlockLengthSql, ScsiMode_Id FROM BlockDescriptors");
                migrationBuilder.DropTable("BlockDescriptors");
            }
            #endregion

            #region TABLE: TestedSequentialMedia
            migrationBuilder.CreateTable(
                name: "TestedSequentialMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CanReadMediaSerial = table.Column<bool>(nullable: true),
                    Density = table.Column<byte>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    MediaIsRecognized = table.Column<bool>(nullable: false),
                    MediumType = table.Column<byte>(nullable: true),
                    MediumTypeName = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    ModeSense6Data = table.Column<byte[]>(nullable: true),
                    ModeSense10Data = table.Column<byte[]>(nullable: true),
                    SscId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestedSequentialMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestedSequentialMedia_Ssc_SscId",
                        column: x => x.SscId,
                        principalTable: "Ssc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (TestedSequentialMediasExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO TestedSequentialMedia (Id, CanReadMediaSerial, Density, Manufacturer, MediaIsRecognized, MediumType, MediumTypeName, Model, ModeSense6Data, ModeSense10Data, SscId) SELECT Id, CanReadMediaSerial, Density, Manufacturer, MediaIsRecognized, MediumType, MediumTypeName, Model, ModeSense6Data, ModeSense10Data, Ssc_Id FROM TestedSequentialMedias");
                migrationBuilder.DropTable("TestedSequentialMedias");
            }
            #endregion

            #region TABLE: UsbProducts

            if (UsbProductsExists)
            {
                migrationBuilder.RenameTable(name: "UsbProducts", newName: "UsbProducts_old");
            }

            migrationBuilder.CreateTable(
                name: "UsbProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    Product = table.Column<string>(nullable: true),
                    AddedWhen = table.Column<DateTime>(nullable: false),
                    ModifiedWhen = table.Column<DateTime>(nullable: false),
                    VendorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsbProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsbProducts_UsbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "UsbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            if (UsbProductsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO UsbProducts (Id, ProductId, Product, AddedWhen, ModifiedWhen, VendorId) SELECT Id, ProductId, Product, AddedWhen, ModifiedWhen, VendorId FROM UsbProducts_old");
                migrationBuilder.DropTable("UsbProducts_old");
            }
            #endregion

#region TABLE: SscSupportedMedia
            migrationBuilder.CreateTable(
                name: "SscSupportedMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MediumType = table.Column<byte>(nullable: false),
                    Width = table.Column<ushort>(nullable: false),
                    Length = table.Column<ushort>(nullable: false),
                    Organization = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    WidthSql = table.Column<short>(nullable: false),
                    LengthSql = table.Column<short>(nullable: false),
                    SscId = table.Column<int>(nullable: true),
                    TestedSequentialMediaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SscSupportedMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SscSupportedMedia_Ssc_SscId",
                        column: x => x.SscId,
                        principalTable: "Ssc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SscSupportedMedia_TestedSequentialMedia_TestedSequentialMedi~",
                        column: x => x.TestedSequentialMediaId,
                        principalTable: "TestedSequentialMedia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (SscSupportedMediasExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO SscSupportedMedia (Id, MediumType, Width, Length, Organization, Name, Description, WidthSql, LengthSql, SscId, TestedSequentialMediaId) SELECT Id, MediumType, WidthSql AS Width, LengthSql AS Length, Organization, Name, Description, WidthSql, LengthSql, Ssc_Id, TestedSequentialMedia_Id FROM SscSupportedMedias");
                migrationBuilder.DropTable("SscSupportedMedias");
            }
            #endregion

            #region TABLE: SupportedDensity
            migrationBuilder.CreateTable(
                name: "SupportedDensity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PrimaryCode = table.Column<byte>(nullable: false),
                    SecondaryCode = table.Column<byte>(nullable: false),
                    Writable = table.Column<bool>(nullable: false),
                    Duplicate = table.Column<bool>(nullable: false),
                    DefaultDensity = table.Column<bool>(nullable: false),
                    BitsPerMm = table.Column<uint>(nullable: false),
                    Width = table.Column<ushort>(nullable: false),
                    Tracks = table.Column<ushort>(nullable: false),
                    Capacity = table.Column<uint>(nullable: false),
                    Organization = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BitsPerMmSql = table.Column<int>(nullable: false),
                    WidthSql = table.Column<short>(nullable: false),
                    TracksSql = table.Column<short>(nullable: false),
                    CapacitySql = table.Column<int>(nullable: false),
                    SscId = table.Column<int>(nullable: true),
                    TestedSequentialMediaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportedDensity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportedDensity_Ssc_SscId",
                        column: x => x.SscId,
                        principalTable: "Ssc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                        column: x => x.TestedSequentialMediaId,
                        principalTable: "TestedSequentialMedia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (SupportedDensitiesExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO SupportedDensity (PrimaryCode, SecondaryCode, Writable, Duplicate, DefaultDensity, BitsPerMm, Width, Tracks, Capacity, Organization, Name, Description, BitsPerMmSql, WidthSql, TracksSql, CapacitySql, SscId, TestedSequentialMediaId) SELECT PrimaryCode, SecondaryCode, Writable, Duplicate, DefaultDensity, BitsPerMmSql AS BitsPerMm, WidthSql AS Width, TracksSql AS Tracks, CapacitySql AS Capacity, Organization, Name, Description, BitsPerMmSql, WidthSql, TracksSql, CapacitySql, Ssc_Id, TestedSequentialMedia_Id FROM SupportedDensities");
                migrationBuilder.DropTable("SupportedDensities");
            }
#endregion

#region TABLE: DensityCode
            migrationBuilder.CreateTable(
                name: "DensityCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    SscSupportedMediaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DensityCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DensityCode_SscSupportedMedia_SscSupportedMediaId",
                        column: x => x.SscSupportedMediaId,
                        principalTable: "SscSupportedMedia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (DensityCodesExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO DensityCode (Id, Code, SscSupportedMediaId) SELECT Id, Code, SscSupportedMedia_Id FROM DensityCodes");
                migrationBuilder.DropTable("DensityCodes");
            }
            #endregion

            #region TABLE: Devices

            if (DevicesExists)
                migrationBuilder.RenameTable(name: "Devices", newName: "Devices_old");

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USBId = table.Column<int>(nullable: true),
                    FireWireId = table.Column<int>(nullable: true),
                    PCMCIAId = table.Column<int>(nullable: true),
                    CompactFlash = table.Column<bool>(nullable: false),
                    ATAId = table.Column<int>(nullable: true),
                    ATAPIId = table.Column<int>(nullable: true),
                    SCSIId = table.Column<int>(nullable: true),
                    MultiMediaCardId = table.Column<int>(nullable: true),
                    SecureDigitalId = table.Column<int>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Revision = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    AddedWhen = table.Column<DateTime>(nullable: false),
                    ModifiedWhen = table.Column<DateTime>(nullable: true),
                    CdOffsetId = table.Column<int>(nullable: true),
                    OptimalMultipleSectorsRead = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_CdOffsets_CdOffsetId",
                        column: x => x.CdOffsetId,
                        principalTable: "CdOffsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_FireWire_FireWireId",
                        column: x => x.FireWireId,
                        principalTable: "FireWire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_MmcSd_MultiMediaCardId",
                        column: x => x.MultiMediaCardId,
                        principalTable: "MmcSd",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_Pcmcia_PCMCIAId",
                        column: x => x.PCMCIAId,
                        principalTable: "Pcmcia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_MmcSd_SecureDigitalId",
                        column: x => x.SecureDigitalId,
                        principalTable: "MmcSd",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_Usb_USBId",
                        column: x => x.USBId,
                        principalTable: "Usb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (DevicesExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Devices (Id, USBId, FireWireId, PCMCIAId, CompactFlash, ATAId, ATAPIId, SCSIId, MultiMediaCardId, SecureDigitalId, Manufacturer, Model, Revision, Type, AddedWhen, ModifiedWhen, CdOffsetId, OptimalMultipleSectorsRead) SELECT Id, USB_Id, FireWire_Id, PCMCIA_Id, CompactFlash, ATA_Id, ATAPI_Id, SCSI_Id, MultiMediaCard_Id, SecureDigital_Id, Manufacturer, Model, Revision, Type, AddedWhen, ModifiedWhen, CdOffset_Id, OptimalMultipleSectorsRead FROM Devices_old");
                migrationBuilder.DropTable("Devices_old");
            }
            #endregion

            #region TABLE: DeviceStats

            if (DeviceStatsExists)
                migrationBuilder.RenameTable(name: "DeviceStats", newName: "DeviceStats_old");

            migrationBuilder.CreateTable(
                name: "DeviceStats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Revision = table.Column<string>(nullable: true),
                    Bus = table.Column<string>(nullable: true),
                    ReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceStats_Devices_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (DeviceStatsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO DeviceStats (Id, Manufacturer, Model, Revision, Bus, ReportId) SELECT Id, Manufacturer, Model, Revision, Bus, Report_Id FROM DeviceStats_old");
                migrationBuilder.DropTable("DeviceStats_old");
            }
            #endregion

            #region TABLE: Reports
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USBId = table.Column<int>(nullable: true),
                    FireWireId = table.Column<int>(nullable: true),
                    PCMCIAId = table.Column<int>(nullable: true),
                    CompactFlash = table.Column<bool>(nullable: false),
                    ATAId = table.Column<int>(nullable: true),
                    ATAPIId = table.Column<int>(nullable: true),
                    SCSIId = table.Column<int>(nullable: true),
                    MultiMediaCardId = table.Column<int>(nullable: true),
                    SecureDigitalId = table.Column<int>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Revision = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UploadedWhen = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_FireWire_FireWireId",
                        column: x => x.FireWireId,
                        principalTable: "FireWire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_MmcSd_MultiMediaCardId",
                        column: x => x.MultiMediaCardId,
                        principalTable: "MmcSd",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Pcmcia_PCMCIAId",
                        column: x => x.PCMCIAId,
                        principalTable: "Pcmcia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_MmcSd_SecureDigitalId",
                        column: x => x.SecureDigitalId,
                        principalTable: "MmcSd",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Usb_USBId",
                        column: x => x.USBId,
                        principalTable: "Usb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (UploadedReportsExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Reports (Id, USBId, FireWireId, PCMCIAId, CompactFlash, ATAId, ATAPIId, SCSIId, MultiMediaCardId, SecureDigitalId, Manufacturer, Model, Revision, Type, UploadedWhen) SELECT Id, USB_Id, FireWire_Id, PCMCIA_Id, CompactFlash, ATA_Id, ATAPI_Id, SCSI_Id, MultiMediaCard_Id, SecureDigital_Id, Manufacturer, Model, Revision, Type, UploadedWhen FROM UploadedReports");
                migrationBuilder.DropTable("UploadedReports");
            }
            #endregion

            #region TABLE: TestedMedia
            migrationBuilder.CreateTable(
                name: "TestedMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentifyData = table.Column<byte[]>(nullable: true),
                    Blocks = table.Column<ulong>(nullable: true),
                    BlockSize = table.Column<uint>(nullable: true),
                    CanReadAACS = table.Column<bool>(nullable: true),
                    CanReadADIP = table.Column<bool>(nullable: true),
                    CanReadATIP = table.Column<bool>(nullable: true),
                    CanReadBCA = table.Column<bool>(nullable: true),
                    CanReadC2Pointers = table.Column<bool>(nullable: true),
                    CanReadCMI = table.Column<bool>(nullable: true),
                    CanReadCorrectedSubchannel = table.Column<bool>(nullable: true),
                    CanReadCorrectedSubchannelWithC2 = table.Column<bool>(nullable: true),
                    CanReadDCB = table.Column<bool>(nullable: true),
                    CanReadDDS = table.Column<bool>(nullable: true),
                    CanReadDMI = table.Column<bool>(nullable: true),
                    CanReadDiscInformation = table.Column<bool>(nullable: true),
                    CanReadFullTOC = table.Column<bool>(nullable: true),
                    CanReadHDCMI = table.Column<bool>(nullable: true),
                    CanReadLayerCapacity = table.Column<bool>(nullable: true),
                    CanReadFirstTrackPreGap = table.Column<bool>(nullable: true),
                    CanReadLeadIn = table.Column<bool>(nullable: true),
                    CanReadLeadOut = table.Column<bool>(nullable: true),
                    CanReadMediaID = table.Column<bool>(nullable: true),
                    CanReadMediaSerial = table.Column<bool>(nullable: true),
                    CanReadPAC = table.Column<bool>(nullable: true),
                    CanReadPFI = table.Column<bool>(nullable: true),
                    CanReadPMA = table.Column<bool>(nullable: true),
                    CanReadPQSubchannel = table.Column<bool>(nullable: true),
                    CanReadPQSubchannelWithC2 = table.Column<bool>(nullable: true),
                    CanReadPRI = table.Column<bool>(nullable: true),
                    CanReadRWSubchannel = table.Column<bool>(nullable: true),
                    CanReadRWSubchannelWithC2 = table.Column<bool>(nullable: true),
                    CanReadRecordablePFI = table.Column<bool>(nullable: true),
                    CanReadSpareAreaInformation = table.Column<bool>(nullable: true),
                    CanReadTOC = table.Column<bool>(nullable: true),
                    Density = table.Column<byte>(nullable: true),
                    LongBlockSize = table.Column<uint>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    MediaIsRecognized = table.Column<bool>(nullable: false),
                    MediumType = table.Column<byte>(nullable: true),
                    MediumTypeName = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    SupportsHLDTSTReadRawDVD = table.Column<bool>(nullable: true),
                    SupportsNECReadCDDA = table.Column<bool>(nullable: true),
                    SupportsPioneerReadCDDA = table.Column<bool>(nullable: true),
                    SupportsPioneerReadCDDAMSF = table.Column<bool>(nullable: true),
                    SupportsPlextorReadCDDA = table.Column<bool>(nullable: true),
                    SupportsPlextorReadRawDVD = table.Column<bool>(nullable: true),
                    SupportsRead10 = table.Column<bool>(nullable: true),
                    SupportsRead12 = table.Column<bool>(nullable: true),
                    SupportsRead16 = table.Column<bool>(nullable: true),
                    SupportsRead6 = table.Column<bool>(nullable: true),
                    SupportsReadCapacity16 = table.Column<bool>(nullable: true),
                    SupportsReadCapacity = table.Column<bool>(nullable: true),
                    SupportsReadCd = table.Column<bool>(nullable: true),
                    SupportsReadCdMsf = table.Column<bool>(nullable: true),
                    SupportsReadCdRaw = table.Column<bool>(nullable: true),
                    SupportsReadCdMsfRaw = table.Column<bool>(nullable: true),
                    SupportsReadLong16 = table.Column<bool>(nullable: true),
                    SupportsReadLong = table.Column<bool>(nullable: true),
                    ModeSense6Data = table.Column<byte[]>(nullable: true),
                    ModeSense10Data = table.Column<byte[]>(nullable: true),
                    CHSId = table.Column<int>(nullable: true),
                    CurrentCHSId = table.Column<int>(nullable: true),
                    LBASectors = table.Column<uint>(nullable: true),
                    LBA48Sectors = table.Column<ulong>(nullable: true),
                    LogicalAlignment = table.Column<ushort>(nullable: true),
                    NominalRotationRate = table.Column<ushort>(nullable: true),
                    PhysicalBlockSize = table.Column<uint>(nullable: true),
                    SolidStateDevice = table.Column<bool>(nullable: true),
                    UnformattedBPT = table.Column<ushort>(nullable: true),
                    UnformattedBPS = table.Column<ushort>(nullable: true),
                    SupportsReadDmaLba = table.Column<bool>(nullable: true),
                    SupportsReadDmaRetryLba = table.Column<bool>(nullable: true),
                    SupportsReadLba = table.Column<bool>(nullable: true),
                    SupportsReadRetryLba = table.Column<bool>(nullable: true),
                    SupportsReadLongLba = table.Column<bool>(nullable: true),
                    SupportsReadLongRetryLba = table.Column<bool>(nullable: true),
                    SupportsSeekLba = table.Column<bool>(nullable: true),
                    SupportsReadDmaLba48 = table.Column<bool>(nullable: true),
                    SupportsReadLba48 = table.Column<bool>(nullable: true),
                    SupportsReadDma = table.Column<bool>(nullable: true),
                    SupportsReadDmaRetry = table.Column<bool>(nullable: true),
                    SupportsReadRetry = table.Column<bool>(nullable: true),
                    SupportsReadSectors = table.Column<bool>(nullable: true),
                    SupportsReadLongRetry = table.Column<bool>(nullable: true),
                    SupportsSeek = table.Column<bool>(nullable: true),
                    CanReadingIntersessionLeadIn = table.Column<bool>(nullable: true),
                    CanReadingIntersessionLeadOut = table.Column<bool>(nullable: true),
                    IntersessionLeadInData = table.Column<byte[]>(nullable: true),
                    IntersessionLeadOutData = table.Column<byte[]>(nullable: true),
                    BlocksSql = table.Column<long>(nullable: true),
                    BlockSizeSql = table.Column<int>(nullable: true),
                    LongBlockSizeSql = table.Column<int>(nullable: true),
                    LBASectorsSql = table.Column<int>(nullable: true),
                    LBA48SectorsSql = table.Column<long>(nullable: true),
                    LogicalAlignmentSql = table.Column<short>(nullable: true),
                    NominalRotationRateSql = table.Column<short>(nullable: true),
                    PhysicalBlockSizeSql = table.Column<int>(nullable: true),
                    UnformattedBPTSql = table.Column<short>(nullable: true),
                    UnformattedBPSSql = table.Column<short>(nullable: true),
                    Read6Data = table.Column<byte[]>(nullable: true),
                    Read10Data = table.Column<byte[]>(nullable: true),
                    Read12Data = table.Column<byte[]>(nullable: true),
                    Read16Data = table.Column<byte[]>(nullable: true),
                    ReadLong10Data = table.Column<byte[]>(nullable: true),
                    ReadLong16Data = table.Column<byte[]>(nullable: true),
                    ReadSectorsData = table.Column<byte[]>(nullable: true),
                    ReadSectorsRetryData = table.Column<byte[]>(nullable: true),
                    ReadDmaData = table.Column<byte[]>(nullable: true),
                    ReadDmaRetryData = table.Column<byte[]>(nullable: true),
                    ReadLbaData = table.Column<byte[]>(nullable: true),
                    ReadRetryLbaData = table.Column<byte[]>(nullable: true),
                    ReadDmaLbaData = table.Column<byte[]>(nullable: true),
                    ReadDmaRetryLbaData = table.Column<byte[]>(nullable: true),
                    ReadLba48Data = table.Column<byte[]>(nullable: true),
                    ReadDmaLba48Data = table.Column<byte[]>(nullable: true),
                    ReadLongData = table.Column<byte[]>(nullable: true),
                    ReadLongRetryData = table.Column<byte[]>(nullable: true),
                    ReadLongLbaData = table.Column<byte[]>(nullable: true),
                    ReadLongRetryLbaData = table.Column<byte[]>(nullable: true),
                    TocData = table.Column<byte[]>(nullable: true),
                    FullTocData = table.Column<byte[]>(nullable: true),
                    AtipData = table.Column<byte[]>(nullable: true),
                    PmaData = table.Column<byte[]>(nullable: true),
                    ReadCdData = table.Column<byte[]>(nullable: true),
                    ReadCdMsfData = table.Column<byte[]>(nullable: true),
                    ReadCdFullData = table.Column<byte[]>(nullable: true),
                    ReadCdMsfFullData = table.Column<byte[]>(nullable: true),
                    Track1PregapData = table.Column<byte[]>(nullable: true),
                    LeadInData = table.Column<byte[]>(nullable: true),
                    LeadOutData = table.Column<byte[]>(nullable: true),
                    C2PointersData = table.Column<byte[]>(nullable: true),
                    PQSubchannelData = table.Column<byte[]>(nullable: true),
                    RWSubchannelData = table.Column<byte[]>(nullable: true),
                    CorrectedSubchannelData = table.Column<byte[]>(nullable: true),
                    PQSubchannelWithC2Data = table.Column<byte[]>(nullable: true),
                    RWSubchannelWithC2Data = table.Column<byte[]>(nullable: true),
                    CorrectedSubchannelWithC2Data = table.Column<byte[]>(nullable: true),
                    PfiData = table.Column<byte[]>(nullable: true),
                    DmiData = table.Column<byte[]>(nullable: true),
                    CmiData = table.Column<byte[]>(nullable: true),
                    DvdBcaData = table.Column<byte[]>(nullable: true),
                    DvdAacsData = table.Column<byte[]>(nullable: true),
                    DvdDdsData = table.Column<byte[]>(nullable: true),
                    DvdSaiData = table.Column<byte[]>(nullable: true),
                    PriData = table.Column<byte[]>(nullable: true),
                    EmbossedPfiData = table.Column<byte[]>(nullable: true),
                    AdipData = table.Column<byte[]>(nullable: true),
                    DcbData = table.Column<byte[]>(nullable: true),
                    HdCmiData = table.Column<byte[]>(nullable: true),
                    DvdLayerData = table.Column<byte[]>(nullable: true),
                    BluBcaData = table.Column<byte[]>(nullable: true),
                    BluDdsData = table.Column<byte[]>(nullable: true),
                    BluSaiData = table.Column<byte[]>(nullable: true),
                    BluDiData = table.Column<byte[]>(nullable: true),
                    BluPacData = table.Column<byte[]>(nullable: true),
                    PlextorReadCddaData = table.Column<byte[]>(nullable: true),
                    PioneerReadCddaData = table.Column<byte[]>(nullable: true),
                    PioneerReadCddaMsfData = table.Column<byte[]>(nullable: true),
                    NecReadCddaData = table.Column<byte[]>(nullable: true),
                    PlextorReadRawDVDData = table.Column<byte[]>(nullable: true),
                    HLDTSTReadRawDVDData = table.Column<byte[]>(nullable: true),
                    AtaId = table.Column<int>(nullable: true),
                    MmcId = table.Column<int>(nullable: true),
                    ScsiId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestedMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestedMedia_Chs_CHSId",
                        column: x => x.CHSId,
                        principalTable: "Chs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestedMedia_Chs_CurrentCHSId",
                        column: x => x.CurrentCHSId,
                        principalTable: "Chs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestedMedia_Mmc_MmcId",
                        column: x => x.MmcId,
                        principalTable: "Mmc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (TestedMediasExists)
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
            migrationBuilder.CreateTable(
                name: "Ata",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Identify = table.Column<byte[]>(nullable: true),
                    ReadCapabilitiesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ata_TestedMedia_ReadCapabilitiesId",
                        column: x => x.ReadCapabilitiesId,
                        principalTable: "TestedMedia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (AtasExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Ata (Id, Identify, ReadCapabilitiesId) SELECT Id, Identify, ReadCapabilities_Id FROM Atas");
                migrationBuilder.DropTable("Atas");
            }
            #endregion

            #region TABLE: Scsi
            migrationBuilder.CreateTable(
                name: "Scsi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InquiryData = table.Column<byte[]>(nullable: true),
                    SupportsModeSense6 = table.Column<bool>(nullable: false),
                    SupportsModeSense10 = table.Column<bool>(nullable: false),
                    SupportsModeSubpages = table.Column<bool>(nullable: false),
                    ModeSenseId = table.Column<int>(nullable: true),
                    MultiMediaDeviceId = table.Column<int>(nullable: true),
                    ReadCapabilitiesId = table.Column<int>(nullable: true),
                    SequentialDeviceId = table.Column<int>(nullable: true),
                    ModeSense6Data = table.Column<byte[]>(nullable: true),
                    ModeSense10Data = table.Column<byte[]>(nullable: true),
                    ModeSense6CurrentData = table.Column<byte[]>(nullable: true),
                    ModeSense10CurrentData = table.Column<byte[]>(nullable: true),
                    ModeSense6ChangeableData = table.Column<byte[]>(nullable: true),
                    ModeSense10ChangeableData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scsi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scsi_ScsiMode_ModeSenseId",
                        column: x => x.ModeSenseId,
                        principalTable: "ScsiMode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scsi_Mmc_MultiMediaDeviceId",
                        column: x => x.MultiMediaDeviceId,
                        principalTable: "Mmc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scsi_TestedMedia_ReadCapabilitiesId",
                        column: x => x.ReadCapabilitiesId,
                        principalTable: "TestedMedia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scsi_Ssc_SequentialDeviceId",
                        column: x => x.SequentialDeviceId,
                        principalTable: "Ssc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (ScsisExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO Scsi (Id, InquiryData, SupportsModeSense6, SupportsModeSense10, SupportsModeSubpages, ModeSenseId, MultiMediaDeviceId, ReadCapabilitiesId, SequentialDeviceId, ModeSense6Data, ModeSense10Data, ModeSense6CurrentData, ModeSense10CurrentData, ModeSense6ChangeableData, ModeSense10ChangeableData) SELECT Id, InquiryData, SupportsModeSense6, SupportsModeSense10, SupportsModeSubpages, ModeSense_Id, MultiMediaDevice_Id, ReadCapabilities_Id, SequentialDevice_Id, ModeSense6Data, ModeSense10Data, ModeSense6CurrentData, ModeSense10CurrentData, ModeSense6ChangeableData, ModeSense10ChangeableData FROM Scsis WHERE EXISTS(SELECT 1 from Mmc WHERE Mmc.Id = Scsis.MultiMediaDevice_Id) OR MultiMediaDevice_Id IS NULL");
                migrationBuilder.DropTable("Scsis");
            }
            #endregion

            #region TABLE: ScsiPage
            migrationBuilder.CreateTable(
                name: "ScsiPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    page = table.Column<byte>(nullable: false),
                    subpage = table.Column<byte>(nullable: true),
                    value = table.Column<byte[]>(nullable: true),
                    ScsiId = table.Column<int>(nullable: true),
                    ScsiModeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScsiPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScsiPage_Scsi_ScsiId",
                        column: x => x.ScsiId,
                        principalTable: "Scsi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScsiPage_ScsiMode_ScsiModeId",
                        column: x => x.ScsiModeId,
                        principalTable: "ScsiMode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            if (ScsiPagesExists)
            {
                migrationBuilder.Sql(
                    "INSERT INTO ScsiPage (Id, page, subpage, value, ScsiId, ScsiModeId) SELECT Id, page, subpage, value, Scsi_Id, ScsiMode_Id FROM ScsiPages");
                migrationBuilder.DropTable("ScsiPages");
            }
            #endregion

            migrationBuilder.CreateIndex(
                name: "IX_Ata_ReadCapabilitiesId",
                table: "Ata",
                column: "ReadCapabilitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockDescriptor_ScsiModeId",
                table: "BlockDescriptor",
                column: "ScsiModeId");

            migrationBuilder.CreateIndex(
                name: "IX_CdOffsets_ModifiedWhen",
                table: "CdOffsets",
                column: "ModifiedWhen");

            migrationBuilder.CreateIndex(
                name: "IX_DensityCode_SscSupportedMediaId",
                table: "DensityCode",
                column: "SscSupportedMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ATAId",
                table: "Devices",
                column: "ATAId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ATAPIId",
                table: "Devices",
                column: "ATAPIId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CdOffsetId",
                table: "Devices",
                column: "CdOffsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FireWireId",
                table: "Devices",
                column: "FireWireId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ModifiedWhen",
                table: "Devices",
                column: "ModifiedWhen");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_MultiMediaCardId",
                table: "Devices",
                column: "MultiMediaCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_PCMCIAId",
                table: "Devices",
                column: "PCMCIAId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_SCSIId",
                table: "Devices",
                column: "SCSIId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_SecureDigitalId",
                table: "Devices",
                column: "SecureDigitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_USBId",
                table: "Devices",
                column: "USBId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStats_ReportId",
                table: "DeviceStats",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Mmc_FeaturesId",
                table: "Mmc",
                column: "FeaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ATAId",
                table: "Reports",
                column: "ATAId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ATAPIId",
                table: "Reports",
                column: "ATAPIId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_FireWireId",
                table: "Reports",
                column: "FireWireId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MultiMediaCardId",
                table: "Reports",
                column: "MultiMediaCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PCMCIAId",
                table: "Reports",
                column: "PCMCIAId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SCSIId",
                table: "Reports",
                column: "SCSIId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SecureDigitalId",
                table: "Reports",
                column: "SecureDigitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_USBId",
                table: "Reports",
                column: "USBId");

            migrationBuilder.CreateIndex(
                name: "IX_Scsi_ModeSenseId",
                table: "Scsi",
                column: "ModeSenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Scsi_MultiMediaDeviceId",
                table: "Scsi",
                column: "MultiMediaDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Scsi_ReadCapabilitiesId",
                table: "Scsi",
                column: "ReadCapabilitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Scsi_SequentialDeviceId",
                table: "Scsi",
                column: "SequentialDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScsiPage_ScsiId",
                table: "ScsiPage",
                column: "ScsiId");

            migrationBuilder.CreateIndex(
                name: "IX_ScsiPage_ScsiModeId",
                table: "ScsiPage",
                column: "ScsiModeId");

            migrationBuilder.CreateIndex(
                name: "IX_SscSupportedMedia_SscId",
                table: "SscSupportedMedia",
                column: "SscId");

            migrationBuilder.CreateIndex(
                name: "IX_SscSupportedMedia_TestedSequentialMediaId",
                table: "SscSupportedMedia",
                column: "TestedSequentialMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportedDensity_SscId",
                table: "SupportedDensity",
                column: "SscId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportedDensity_TestedSequentialMediaId",
                table: "SupportedDensity",
                column: "TestedSequentialMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_TestedMedia_AtaId",
                table: "TestedMedia",
                column: "AtaId");

            migrationBuilder.CreateIndex(
                name: "IX_TestedMedia_CHSId",
                table: "TestedMedia",
                column: "CHSId");

            migrationBuilder.CreateIndex(
                name: "IX_TestedMedia_CurrentCHSId",
                table: "TestedMedia",
                column: "CurrentCHSId");

            migrationBuilder.CreateIndex(
                name: "IX_TestedMedia_MmcId",
                table: "TestedMedia",
                column: "MmcId");

            migrationBuilder.CreateIndex(
                name: "IX_TestedMedia_ScsiId",
                table: "TestedMedia",
                column: "ScsiId");

            migrationBuilder.CreateIndex(
                name: "IX_TestedSequentialMedia_SscId",
                table: "TestedSequentialMedia",
                column: "SscId");

            migrationBuilder.CreateIndex(
                name: "IX_UsbProducts_ModifiedWhen",
                table: "UsbProducts",
                column: "ModifiedWhen");

            migrationBuilder.CreateIndex(
                name: "IX_UsbProducts_ProductId",
                table: "UsbProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UsbProducts_VendorId",
                table: "UsbProducts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_UsbVendors_ModifiedWhen",
                table: "UsbVendors",
                column: "ModifiedWhen");

            migrationBuilder.CreateIndex(
                name: "IX_UsbVendors_VendorId",
                table: "UsbVendors",
                column: "VendorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Ata_ATAId",
                table: "Devices",
                column: "ATAId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Ata_ATAPIId",
                table: "Devices",
                column: "ATAPIId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Scsi_SCSIId",
                table: "Devices",
                column: "SCSIId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Ata_ATAId",
                table: "Reports",
                column: "ATAId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Ata_ATAPIId",
                table: "Reports",
                column: "ATAPIId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(
                "DELETE FROM Reports WHERE NOT EXISTS(SELECT 1 from Scsi WHERE Scsi.Id = SCSIId) AND SCSIId IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Scsi_SCSIId",
                table: "Reports",
                column: "SCSIId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Ata_AtaId",
                table: "TestedMedia",
                column: "AtaId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Scsi_ScsiId",
                table: "TestedMedia",
                column: "ScsiId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            if(EFExists)
                migrationBuilder.DropTable("__MigrationHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ata_TestedMedia_ReadCapabilitiesId",
                table: "Ata");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_TestedMedia_ReadCapabilitiesId",
                table: "Scsi");

            migrationBuilder.DropTable(
                name: "BlockDescriptor");

            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "DensityCode");

            migrationBuilder.DropTable(
                name: "DeviceStats");

            migrationBuilder.DropTable(
                name: "Filesystems");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "MediaFormats");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "OperatingSystems");

            migrationBuilder.DropTable(
                name: "Partitions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "ScsiPage");

            migrationBuilder.DropTable(
                name: "SupportedDensity");

            migrationBuilder.DropTable(
                name: "UsbProducts");

            migrationBuilder.DropTable(
                name: "Versions");

            migrationBuilder.DropTable(
                name: "SscSupportedMedia");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "UsbVendors");

            migrationBuilder.DropTable(
                name: "TestedSequentialMedia");

            migrationBuilder.DropTable(
                name: "CdOffsets");

            migrationBuilder.DropTable(
                name: "FireWire");

            migrationBuilder.DropTable(
                name: "MmcSd");

            migrationBuilder.DropTable(
                name: "Pcmcia");

            migrationBuilder.DropTable(
                name: "Usb");

            migrationBuilder.DropTable(
                name: "TestedMedia");

            migrationBuilder.DropTable(
                name: "Ata");

            migrationBuilder.DropTable(
                name: "Chs");

            migrationBuilder.DropTable(
                name: "Scsi");

            migrationBuilder.DropTable(
                name: "ScsiMode");

            migrationBuilder.DropTable(
                name: "Mmc");

            migrationBuilder.DropTable(
                name: "Ssc");

            migrationBuilder.DropTable(
                name: "MmcFeatures");
        }
    }
}
