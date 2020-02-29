using Microsoft.EntityFrameworkCore.Migrations;

namespace Aaru.Server.Migrations
{
    public partial class MakeFieldsUnsigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("ProductIDSql", "Usb");

            migrationBuilder.DropColumn("VendorIDSql", "Usb");

            migrationBuilder.DropColumn("BlockSizeSql", "TestedMedia");

            migrationBuilder.DropColumn("BlocksSql", "TestedMedia");

            migrationBuilder.DropColumn("LBA48SectorsSql", "TestedMedia");

            migrationBuilder.DropColumn("LBASectorsSql", "TestedMedia");

            migrationBuilder.DropColumn("LogicalAlignmentSql", "TestedMedia");

            migrationBuilder.DropColumn("LongBlockSizeSql", "TestedMedia");

            migrationBuilder.DropColumn("NominalRotationRateSql", "TestedMedia");

            migrationBuilder.DropColumn("PhysicalBlockSizeSql", "TestedMedia");

            migrationBuilder.DropColumn("UnformattedBPSSql", "TestedMedia");

            migrationBuilder.DropColumn("UnformattedBPTSql", "TestedMedia");

            migrationBuilder.DropColumn("BitsPerMmSql", "SupportedDensity");

            migrationBuilder.DropColumn("CapacitySql", "SupportedDensity");

            migrationBuilder.DropColumn("TracksSql", "SupportedDensity");

            migrationBuilder.DropColumn("WidthSql", "SupportedDensity");

            migrationBuilder.DropColumn("LengthSql", "SscSupportedMedia");

            migrationBuilder.DropColumn("WidthSql", "SscSupportedMedia");

            migrationBuilder.DropColumn("MaxBlockLengthSql", "Ssc");

            migrationBuilder.DropColumn("MinBlockLengthSql", "Ssc");

            migrationBuilder.DropColumn("CardCodeSql", "Pcmcia");

            migrationBuilder.DropColumn("ManufacturerCodeSql", "Pcmcia");

            migrationBuilder.DropColumn("BlocksPerReadableUnitSql", "MmcFeatures");

            migrationBuilder.DropColumn("LogicalBlockSizeSql", "MmcFeatures");

            migrationBuilder.DropColumn("PhysicalInterfaceStandardNumberSql", "MmcFeatures");

            migrationBuilder.DropColumn("VolumeLevelsSql", "MmcFeatures");

            migrationBuilder.DropColumn("ProductIDSql", "FireWire");

            migrationBuilder.DropColumn("VendorIDSql", "FireWire");

            migrationBuilder.DropColumn("CylindersSql", "Chs");

            migrationBuilder.DropColumn("HeadsSql", "Chs");

            migrationBuilder.DropColumn("SectorsSql", "Chs");

            migrationBuilder.DropColumn("BlockLengthSql", "BlockDescriptor");

            migrationBuilder.DropColumn("BlocksSql", "BlockDescriptor");

            migrationBuilder.AlterColumn<ushort>("VendorId", "UsbVendors", nullable: false, oldClrType: typeof(int),
                                                 oldType: "int");

            migrationBuilder.AlterColumn<ushort>("ProductId", "UsbProducts", nullable: false, oldClrType: typeof(int),
                                                 oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>("VendorId", "UsbVendors", "int", nullable: false,
                                              oldClrType: typeof(ushort));

            migrationBuilder.AlterColumn<int>("ProductId", "UsbProducts", "int", nullable: false,
                                              oldClrType: typeof(ushort));

            migrationBuilder.AddColumn<short>("ProductIDSql", "Usb", "smallint", nullable: false,
                                              defaultValue: (short)0);

            migrationBuilder.AddColumn<short>("VendorIDSql", "Usb", "smallint", nullable: false,
                                              defaultValue: (short)0);

            migrationBuilder.AddColumn<int>("BlockSizeSql", "TestedMedia", "int", nullable: true);

            migrationBuilder.AddColumn<long>("BlocksSql", "TestedMedia", "bigint", nullable: true);

            migrationBuilder.AddColumn<long>("LBA48SectorsSql", "TestedMedia", "bigint", nullable: true);

            migrationBuilder.AddColumn<int>("LBASectorsSql", "TestedMedia", "int", nullable: true);

            migrationBuilder.AddColumn<short>("LogicalAlignmentSql", "TestedMedia", "smallint", nullable: true);

            migrationBuilder.AddColumn<int>("LongBlockSizeSql", "TestedMedia", "int", nullable: true);

            migrationBuilder.AddColumn<short>("NominalRotationRateSql", "TestedMedia", "smallint", nullable: true);

            migrationBuilder.AddColumn<int>("PhysicalBlockSizeSql", "TestedMedia", "int", nullable: true);

            migrationBuilder.AddColumn<short>("UnformattedBPSSql", "TestedMedia", "smallint", nullable: true);

            migrationBuilder.AddColumn<short>("UnformattedBPTSql", "TestedMedia", "smallint", nullable: true);

            migrationBuilder.AddColumn<int>("BitsPerMmSql", "SupportedDensity", "int", nullable: false,
                                            defaultValue: 0);

            migrationBuilder.AddColumn<int>("CapacitySql", "SupportedDensity", "int", nullable: false, defaultValue: 0);

            migrationBuilder.AddColumn<short>("TracksSql", "SupportedDensity", "smallint", nullable: false,
                                              defaultValue: (short)0);

            migrationBuilder.AddColumn<short>("WidthSql", "SupportedDensity", "smallint", nullable: false,
                                              defaultValue: (short)0);

            migrationBuilder.AddColumn<short>("LengthSql", "SscSupportedMedia", "smallint", nullable: false,
                                              defaultValue: (short)0);

            migrationBuilder.AddColumn<short>("WidthSql", "SscSupportedMedia", "smallint", nullable: false,
                                              defaultValue: (short)0);

            migrationBuilder.AddColumn<int>("MaxBlockLengthSql", "Ssc", "int", nullable: true);

            migrationBuilder.AddColumn<int>("MinBlockLengthSql", "Ssc", "int", nullable: true);

            migrationBuilder.AddColumn<short>("CardCodeSql", "Pcmcia", "smallint", nullable: true);

            migrationBuilder.AddColumn<short>("ManufacturerCodeSql", "Pcmcia", "smallint", nullable: true);

            migrationBuilder.AddColumn<short>("BlocksPerReadableUnitSql", "MmcFeatures", "smallint", nullable: true);

            migrationBuilder.AddColumn<int>("LogicalBlockSizeSql", "MmcFeatures", "int", nullable: true);

            migrationBuilder.AddColumn<int>("PhysicalInterfaceStandardNumberSql", "MmcFeatures", "int", nullable: true);

            migrationBuilder.AddColumn<short>("VolumeLevelsSql", "MmcFeatures", "smallint", nullable: true);

            migrationBuilder.AddColumn<int>("ProductIDSql", "FireWire", "int", nullable: false, defaultValue: 0);

            migrationBuilder.AddColumn<int>("VendorIDSql", "FireWire", "int", nullable: false, defaultValue: 0);

            migrationBuilder.AddColumn<short>("CylindersSql", "Chs", "smallint", nullable: false,
                                              defaultValue: (short)0);

            migrationBuilder.AddColumn<short>("HeadsSql", "Chs", "smallint", nullable: false, defaultValue: (short)0);

            migrationBuilder.AddColumn<short>("SectorsSql", "Chs", "smallint", nullable: false, defaultValue: (short)0);

            migrationBuilder.AddColumn<int>("BlockLengthSql", "BlockDescriptor", "int", nullable: true);

            migrationBuilder.AddColumn<long>("BlocksSql", "BlockDescriptor", "bigint", nullable: true);
        }
    }
}