using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscImageChef.Server.Migrations
{
    public partial class EFTypesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>("Value", "Versions", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Vendor", "UsbVendors", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Product", "UsbProducts", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("RemovableMedia", "Usb", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<string>("Product", "Usb", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "Usb", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "TestedSequentialMedia", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("MediumTypeName", "TestedSequentialMedia", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("MediaIsRecognized", "TestedSequentialMedia", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("Manufacturer", "TestedSequentialMedia", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadMediaSerial", "TestedSequentialMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsSeekLba", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsSeek", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadSectors", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadRetryLba", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadRetry", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadLongRetryLba", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadLongRetry", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadLongLba", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadLong16", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadLong", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadLba48", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadLba", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadDmaRetryLba", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadDmaRetry", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadDmaLba48", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadDmaLba", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadDma", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadCdRaw", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadCdMsfRaw", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadCdMsf", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadCd", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadCapacity16", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsReadCapacity", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsRead6", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsRead16", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsRead12", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsRead10", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsPlextorReadRawDVD", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsPlextorReadCDDA", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsPioneerReadCDDAMSF", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsPioneerReadCDDA", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsNECReadCDDA", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsHLDTSTReadRawDVD", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SolidStateDevice", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "TestedMedia", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("MediumTypeName", "TestedMedia", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("MediaIsRecognized", "TestedMedia", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("Manufacturer", "TestedMedia", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadingIntersessionLeadOut", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadingIntersessionLeadIn", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadTOC", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadSpareAreaInformation", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadRecordablePFI", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadRWSubchannelWithC2", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadRWSubchannel", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadPRI", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadPQSubchannelWithC2", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadPQSubchannel", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadPMA", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadPFI", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadPAC", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadMediaSerial", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadMediaID", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadLeadOut", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadLeadIn", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadLayerCapacity", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadHDCMI", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadFullTOC", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadFirstTrackPreGap", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadDiscInformation", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadDMI", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadDDS", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadDCB", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadCorrectedSubchannelWithC2", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadCorrectedSubchannel", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadCMI", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadC2Pointers", "TestedMedia", nullable: true,
                                               oldClrType: typeof(ulong), oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadBCA", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadATIP", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadADIP", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CanReadAACS", "TestedMedia", nullable: true, oldClrType: typeof(ulong),
                                               oldType: "bit", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("Writable", "SupportedDensity", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("Organization", "SupportedDensity", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "SupportedDensity", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("Duplicate", "SupportedDensity", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("Description", "SupportedDensity", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("DefaultDensity", "SupportedDensity", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("Organization", "SscSupportedMedia", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "SscSupportedMedia", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Description", "SscSupportedMedia", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("WriteProtected", "ScsiMode", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("DPOandFUA", "ScsiMode", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("BlankCheckEnabled", "ScsiMode", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsModeSubpages", "Scsi", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsModeSense6", "Scsi", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsModeSense10", "Scsi", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("Revision", "Reports", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "Reports", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "Reports", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CompactFlash", "Reports", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<string>("ProductName", "Pcmcia", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "Pcmcia", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Compliance", "Pcmcia", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "Partitions", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Version", "OperatingSystems", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "OperatingSystems", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("SupportsWriteProtectPAC", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsWriteInhibitDCB", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsVCPS", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsSeparateVolume", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsSecurDisc", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsSWPP", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsPWP", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsOSSC", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsModePage1Ch", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsHybridDiscs", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsDeviceBusyEvent", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsDAP", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsCSS", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsCPRM", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsC2", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsBusEncryption", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("SupportsAACS", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("PreventJumper", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("MultiRead", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("Locked", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("ErrorRecoveryPage", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("EmbeddedChanger", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("DVDMultiRead", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("DBML", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("ChangerSupportsDiscPresent", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("ChangerIsSideChangeCapable", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteRawSubchannelInTAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteRawMultiSession", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteRaw", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteRWSubchannelInTAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteRWSubchannelInSAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWritePackedSubchannelInTAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteOldBDRE", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteOldBDR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteHDDVDRAM", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteHDDVDR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDVDRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDVDRDL", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDVDR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDVDPlusRWDL", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDVDPlusRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDVDPlusRDL", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDVDPlusR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDVDPlusMRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDDCDRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteDDCDR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteCSSManagedDVD", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteCDTAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteCDSAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteCDRWCAV", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteCDRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteCDMRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteBusEncryptedBlocks", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteBDRE2", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteBDRE1", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteBDR", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanWriteBD", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanUpgradeFirmware", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanTestWriteInTAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanTestWriteInSAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanTestWriteDVD", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanTestWriteDDCDR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReportMediaSerial", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReportDriveSerial", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadSpareAreaInformation", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadOldBDROM", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadOldBDRE", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadOldBDR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadLeadInCDText", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadHDDVDRAM", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadHDDVDR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadHDDVD", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadDriveAACSCertificate", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadDVDPlusRWDL", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadDVDPlusRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadDVDPlusRDL", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadDVDPlusR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadDVDPlusMRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadDVD", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadDDCD", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadCPRM_MKB", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadCDMRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadCD", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadBluBCA", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadBDROM", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadBDRE2", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadBDRE1", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadBDR", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadBD", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadAllDualRW", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanReadAllDualR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanPseudoOverwriteBDR", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanPlayCDAudio", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanOverwriteTAOTrack", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanOverwriteSAOTrack", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanMuteSeparateChannels", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanLoad", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanGenerateBindingNonce", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanFormatRRM", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanFormatQCert", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanFormatFRF", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanFormatCert", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanFormatBDREWithoutSpare", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanFormat", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanExpandBDRESpareArea", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanEraseSector", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanEject", "MmcFeatures", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<bool>("CanAudioScan", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("BufferUnderrunFreeInTAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("BufferUnderrunFreeInSAO", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("BufferUnderrunFreeInDVD", "MmcFeatures", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("Type", "Medias", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("Real", "Medias", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<string>("Name", "MediaFormats", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("RemovableMedia", "FireWire", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<string>("Product", "FireWire", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "FireWire", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "Filters", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "Filesystems", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Revision", "DeviceStats", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "DeviceStats", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "DeviceStats", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Bus", "DeviceStats", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Revision", "Devices", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "Devices", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "Devices", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("CompactFlash", "Devices", nullable: false, oldClrType: typeof(ulong),
                                               oldType: "bit");

            migrationBuilder.AlterColumn<string>("Name", "Commands", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "CdOffsets", nullable: true, oldClrType: typeof(string),
                                                 oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "CdOffsets", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Value", "AspNetUserTokens", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "AspNetUserTokens", maxLength: 128, nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(128)",
                                                 oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>("LoginProvider", "AspNetUserTokens", maxLength: 128, nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(128)",
                                                 oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>("UserId", "AspNetUserTokens", nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>("UserName", "AspNetUsers", maxLength: 256, nullable: true,
                                                 oldClrType: typeof(string), oldType: "varchar(256)", oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<bool>("TwoFactorEnabled", "AspNetUsers", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("SecurityStamp", "AspNetUsers", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<bool>("PhoneNumberConfirmed", "AspNetUsers", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("PhoneNumber", "AspNetUsers", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("PasswordHash", "AspNetUsers", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("NormalizedUserName", "AspNetUsers", maxLength: 256, nullable: true,
                                                 oldClrType: typeof(string), oldType: "varchar(256)", oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<string>("NormalizedEmail", "AspNetUsers", maxLength: 256, nullable: true,
                                                 oldClrType: typeof(string), oldType: "varchar(256)", oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<bool>("LockoutEnabled", "AspNetUsers", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<bool>("EmailConfirmed", "AspNetUsers", nullable: false,
                                               oldClrType: typeof(ulong), oldType: "bit");

            migrationBuilder.AlterColumn<string>("Email", "AspNetUsers", maxLength: 256, nullable: true,
                                                 oldClrType: typeof(string), oldType: "varchar(256)", oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<string>("ConcurrencyStamp", "AspNetUsers", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Id", "AspNetUsers", nullable: false, oldClrType: typeof(string),
                                                 oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>("RoleId", "AspNetUserRoles", nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>("UserId", "AspNetUserRoles", nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>("UserId", "AspNetUserLogins", nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>("ProviderDisplayName", "AspNetUserLogins", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("ProviderKey", "AspNetUserLogins", maxLength: 128, nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(128)",
                                                 oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>("LoginProvider", "AspNetUserLogins", maxLength: 128, nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(128)",
                                                 oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>("UserId", "AspNetUserClaims", nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>("ClaimValue", "AspNetUserClaims", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("ClaimType", "AspNetUserClaims", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("NormalizedName", "AspNetRoles", maxLength: 256, nullable: true,
                                                 oldClrType: typeof(string), oldType: "varchar(256)", oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "AspNetRoles", maxLength: 256, nullable: true,
                                                 oldClrType: typeof(string), oldType: "varchar(256)", oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<string>("ConcurrencyStamp", "AspNetRoles", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("Id", "AspNetRoles", nullable: false, oldClrType: typeof(string),
                                                 oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>("RoleId", "AspNetRoleClaims", nullable: false,
                                                 oldClrType: typeof(string), oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>("ClaimValue", "AspNetRoleClaims", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);

            migrationBuilder.AlterColumn<string>("ClaimType", "AspNetRoleClaims", nullable: true,
                                                 oldClrType: typeof(string), oldType: "longtext", oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>("Value", "Versions", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Vendor", "UsbVendors", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Product", "UsbProducts", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("RemovableMedia", "Usb", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Product", "Usb", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "Usb", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "TestedSequentialMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("MediumTypeName", "TestedSequentialMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("MediaIsRecognized", "TestedSequentialMedia", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Manufacturer", "TestedSequentialMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadMediaSerial", "TestedSequentialMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsSeekLba", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsSeek", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadSectors", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadRetryLba", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadRetry", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadLongRetryLba", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadLongRetry", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadLongLba", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadLong16", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadLong", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadLba48", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadLba", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadDmaRetryLba", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadDmaRetry", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadDmaLba48", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadDmaLba", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadDma", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadCdRaw", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadCdMsfRaw", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadCdMsf", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadCd", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadCapacity16", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsReadCapacity", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsRead6", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsRead16", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsRead12", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsRead10", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsPlextorReadRawDVD", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsPlextorReadCDDA", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsPioneerReadCDDAMSF", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsPioneerReadCDDA", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsNECReadCDDA", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsHLDTSTReadRawDVD", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SolidStateDevice", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "TestedMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("MediumTypeName", "TestedMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("MediaIsRecognized", "TestedMedia", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Manufacturer", "TestedMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadingIntersessionLeadOut", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadingIntersessionLeadIn", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadTOC", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadSpareAreaInformation", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadRecordablePFI", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadRWSubchannelWithC2", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadRWSubchannel", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadPRI", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadPQSubchannelWithC2", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadPQSubchannel", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadPMA", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadPFI", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadPAC", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadMediaSerial", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadMediaID", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadLeadOut", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadLeadIn", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadLayerCapacity", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadHDCMI", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadFullTOC", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadFirstTrackPreGap", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadDiscInformation", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadDMI", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadDDS", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadDCB", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadCorrectedSubchannelWithC2", "TestedMedia", "bit",
                                                nullable: true, oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadCorrectedSubchannel", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadCMI", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadC2Pointers", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadBCA", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadATIP", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadADIP", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CanReadAACS", "TestedMedia", "bit", nullable: true,
                                                oldClrType: typeof(bool), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("Writable", "SupportedDensity", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Organization", "SupportedDensity", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "SupportedDensity", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("Duplicate", "SupportedDensity", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Description", "SupportedDensity", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("DefaultDensity", "SupportedDensity", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Organization", "SscSupportedMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "SscSupportedMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Description", "SscSupportedMedia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("WriteProtected", "ScsiMode", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("DPOandFUA", "ScsiMode", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("BlankCheckEnabled", "ScsiMode", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsModeSubpages", "Scsi", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsModeSense6", "Scsi", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsModeSense10", "Scsi", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Revision", "Reports", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "Reports", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "Reports", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CompactFlash", "Reports", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("ProductName", "Pcmcia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "Pcmcia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Compliance", "Pcmcia", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "Partitions", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Version", "OperatingSystems", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "OperatingSystems", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("SupportsWriteProtectPAC", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsWriteInhibitDCB", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsVCPS", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsSeparateVolume", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsSecurDisc", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsSWPP", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsPWP", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsOSSC", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsModePage1Ch", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsHybridDiscs", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsDeviceBusyEvent", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsDAP", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsCSS", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsCPRM", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsC2", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsBusEncryption", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("SupportsAACS", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("PreventJumper", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("MultiRead", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("Locked", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("ErrorRecoveryPage", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("EmbeddedChanger", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("DVDMultiRead", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("DBML", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("ChangerSupportsDiscPresent", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("ChangerIsSideChangeCapable", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteRawSubchannelInTAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteRawMultiSession", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteRaw", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteRWSubchannelInTAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteRWSubchannelInSAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWritePackedSubchannelInTAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteOldBDRE", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteOldBDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteHDDVDRAM", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteHDDVDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDVDRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDVDRDL", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDVDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDVDPlusRWDL", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDVDPlusRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDVDPlusRDL", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDVDPlusR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDVDPlusMRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDDCDRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteDDCDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteCSSManagedDVD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteCDTAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteCDSAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteCDRWCAV", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteCDRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteCDMRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteBusEncryptedBlocks", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteBDRE2", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteBDRE1", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteBDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanWriteBD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanUpgradeFirmware", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanTestWriteInTAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanTestWriteInSAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanTestWriteDVD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanTestWriteDDCDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReportMediaSerial", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReportDriveSerial", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadSpareAreaInformation", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadOldBDROM", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadOldBDRE", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadOldBDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadLeadInCDText", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadHDDVDRAM", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadHDDVDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadHDDVD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadDriveAACSCertificate", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadDVDPlusRWDL", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadDVDPlusRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadDVDPlusRDL", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadDVDPlusR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadDVDPlusMRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadDVD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadDDCD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadCPRM_MKB", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadCDMRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadCD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadBluBCA", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadBDROM", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadBDRE2", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadBDRE1", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadBDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadBD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadAllDualRW", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanReadAllDualR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanPseudoOverwriteBDR", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanPlayCDAudio", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanOverwriteTAOTrack", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanOverwriteSAOTrack", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanMuteSeparateChannels", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanLoad", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanGenerateBindingNonce", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanFormatRRM", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanFormatQCert", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanFormatFRF", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanFormatCert", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanFormatBDREWithoutSpare", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanFormat", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanExpandBDRESpareArea", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanEraseSector", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanEject", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("CanAudioScan", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("BufferUnderrunFreeInTAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("BufferUnderrunFreeInSAO", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("BufferUnderrunFreeInDVD", "MmcFeatures", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Type", "Medias", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("Real", "Medias", "bit", nullable: false, oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Name", "MediaFormats", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("RemovableMedia", "FireWire", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Product", "FireWire", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "FireWire", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "Filters", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "Filesystems", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Revision", "DeviceStats", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "DeviceStats", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "DeviceStats", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Bus", "DeviceStats", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Revision", "Devices", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "Devices", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "Devices", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("CompactFlash", "Devices", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Name", "Commands", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Model", "CdOffsets", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Manufacturer", "CdOffsets", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Value", "AspNetUserTokens", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "AspNetUserTokens", "varchar(128)", maxLength: 128,
                                                 nullable: false, oldClrType: typeof(string), oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>("LoginProvider", "AspNetUserTokens", "varchar(128)", maxLength: 128,
                                                 nullable: false, oldClrType: typeof(string), oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>("UserId", "AspNetUserTokens", "varchar(255)", nullable: false,
                                                 oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>("UserName", "AspNetUsers", "varchar(256)", maxLength: 256,
                                                 nullable: true, oldClrType: typeof(string), oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("TwoFactorEnabled", "AspNetUsers", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("SecurityStamp", "AspNetUsers", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("PhoneNumberConfirmed", "AspNetUsers", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("PhoneNumber", "AspNetUsers", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("PasswordHash", "AspNetUsers", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("NormalizedUserName", "AspNetUsers", "varchar(256)", maxLength: 256,
                                                 nullable: true, oldClrType: typeof(string), oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<string>("NormalizedEmail", "AspNetUsers", "varchar(256)", maxLength: 256,
                                                 nullable: true, oldClrType: typeof(string), oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<ulong>("LockoutEnabled", "AspNetUsers", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<ulong>("EmailConfirmed", "AspNetUsers", "bit", nullable: false,
                                                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>("Email", "AspNetUsers", "varchar(256)", maxLength: 256, nullable: true,
                                                 oldClrType: typeof(string), oldMaxLength: 256, oldNullable: true);

            migrationBuilder.AlterColumn<string>("ConcurrencyStamp", "AspNetUsers", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Id", "AspNetUsers", "varchar(255)", nullable: false,
                                                 oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>("RoleId", "AspNetUserRoles", "varchar(255)", nullable: false,
                                                 oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>("UserId", "AspNetUserRoles", "varchar(255)", nullable: false,
                                                 oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>("UserId", "AspNetUserLogins", "varchar(255)", nullable: false,
                                                 oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>("ProviderDisplayName", "AspNetUserLogins", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("ProviderKey", "AspNetUserLogins", "varchar(128)", maxLength: 128,
                                                 nullable: false, oldClrType: typeof(string), oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>("LoginProvider", "AspNetUserLogins", "varchar(128)", maxLength: 128,
                                                 nullable: false, oldClrType: typeof(string), oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>("UserId", "AspNetUserClaims", "varchar(255)", nullable: false,
                                                 oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>("ClaimValue", "AspNetUserClaims", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("ClaimType", "AspNetUserClaims", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("NormalizedName", "AspNetRoles", "varchar(256)", maxLength: 256,
                                                 nullable: true, oldClrType: typeof(string), oldMaxLength: 256,
                                                 oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name", "AspNetRoles", "varchar(256)", maxLength: 256, nullable: true,
                                                 oldClrType: typeof(string), oldMaxLength: 256, oldNullable: true);

            migrationBuilder.AlterColumn<string>("ConcurrencyStamp", "AspNetRoles", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("Id", "AspNetRoles", "varchar(255)", nullable: false,
                                                 oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>("RoleId", "AspNetRoleClaims", "varchar(255)", nullable: false,
                                                 oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>("ClaimValue", "AspNetRoleClaims", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);

            migrationBuilder.AlterColumn<string>("ClaimType", "AspNetRoleClaims", "longtext", nullable: true,
                                                 oldClrType: typeof(string), oldNullable: true);
        }
    }
}