using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscImageChef.Server.Migrations
{
    public partial class MakeFieldsUnsigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductIDSql",
                table: "Usb");

            migrationBuilder.DropColumn(
                name: "VendorIDSql",
                table: "Usb");

            migrationBuilder.DropColumn(
                name: "BlockSizeSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "BlocksSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "LBA48SectorsSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "LBASectorsSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "LogicalAlignmentSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "LongBlockSizeSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "NominalRotationRateSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "PhysicalBlockSizeSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "UnformattedBPSSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "UnformattedBPTSql",
                table: "TestedMedia");

            migrationBuilder.DropColumn(
                name: "BitsPerMmSql",
                table: "SupportedDensity");

            migrationBuilder.DropColumn(
                name: "CapacitySql",
                table: "SupportedDensity");

            migrationBuilder.DropColumn(
                name: "TracksSql",
                table: "SupportedDensity");

            migrationBuilder.DropColumn(
                name: "WidthSql",
                table: "SupportedDensity");

            migrationBuilder.DropColumn(
                name: "LengthSql",
                table: "SscSupportedMedia");

            migrationBuilder.DropColumn(
                name: "WidthSql",
                table: "SscSupportedMedia");

            migrationBuilder.DropColumn(
                name: "MaxBlockLengthSql",
                table: "Ssc");

            migrationBuilder.DropColumn(
                name: "MinBlockLengthSql",
                table: "Ssc");

            migrationBuilder.DropColumn(
                name: "CardCodeSql",
                table: "Pcmcia");

            migrationBuilder.DropColumn(
                name: "ManufacturerCodeSql",
                table: "Pcmcia");

            migrationBuilder.DropColumn(
                name: "BlocksPerReadableUnitSql",
                table: "MmcFeatures");

            migrationBuilder.DropColumn(
                name: "LogicalBlockSizeSql",
                table: "MmcFeatures");

            migrationBuilder.DropColumn(
                name: "PhysicalInterfaceStandardNumberSql",
                table: "MmcFeatures");

            migrationBuilder.DropColumn(
                name: "VolumeLevelsSql",
                table: "MmcFeatures");

            migrationBuilder.DropColumn(
                name: "ProductIDSql",
                table: "FireWire");

            migrationBuilder.DropColumn(
                name: "VendorIDSql",
                table: "FireWire");

            migrationBuilder.DropColumn(
                name: "CylindersSql",
                table: "Chs");

            migrationBuilder.DropColumn(
                name: "HeadsSql",
                table: "Chs");

            migrationBuilder.DropColumn(
                name: "SectorsSql",
                table: "Chs");

            migrationBuilder.DropColumn(
                name: "BlockLengthSql",
                table: "BlockDescriptor");

            migrationBuilder.DropColumn(
                name: "BlocksSql",
                table: "BlockDescriptor");

            migrationBuilder.AlterColumn<ushort>(
                name: "VendorId",
                table: "UsbVendors",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<ushort>(
                name: "ProductId",
                table: "UsbProducts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VendorId",
                table: "UsbVendors",
                type: "int",
                nullable: false,
                oldClrType: typeof(ushort));

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "UsbProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(ushort));

            migrationBuilder.AddColumn<short>(
                name: "ProductIDSql",
                table: "Usb",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "VendorIDSql",
                table: "Usb",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "BlockSizeSql",
                table: "TestedMedia",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BlocksSql",
                table: "TestedMedia",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LBA48SectorsSql",
                table: "TestedMedia",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LBASectorsSql",
                table: "TestedMedia",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "LogicalAlignmentSql",
                table: "TestedMedia",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LongBlockSizeSql",
                table: "TestedMedia",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "NominalRotationRateSql",
                table: "TestedMedia",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhysicalBlockSizeSql",
                table: "TestedMedia",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UnformattedBPSSql",
                table: "TestedMedia",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UnformattedBPTSql",
                table: "TestedMedia",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BitsPerMmSql",
                table: "SupportedDensity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CapacitySql",
                table: "SupportedDensity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "TracksSql",
                table: "SupportedDensity",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "WidthSql",
                table: "SupportedDensity",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "LengthSql",
                table: "SscSupportedMedia",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "WidthSql",
                table: "SscSupportedMedia",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "MaxBlockLengthSql",
                table: "Ssc",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinBlockLengthSql",
                table: "Ssc",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "CardCodeSql",
                table: "Pcmcia",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ManufacturerCodeSql",
                table: "Pcmcia",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "BlocksPerReadableUnitSql",
                table: "MmcFeatures",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LogicalBlockSizeSql",
                table: "MmcFeatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhysicalInterfaceStandardNumberSql",
                table: "MmcFeatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "VolumeLevelsSql",
                table: "MmcFeatures",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductIDSql",
                table: "FireWire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VendorIDSql",
                table: "FireWire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "CylindersSql",
                table: "Chs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "HeadsSql",
                table: "Chs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "SectorsSql",
                table: "Chs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "BlockLengthSql",
                table: "BlockDescriptor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BlocksSql",
                table: "BlockDescriptor",
                type: "bigint",
                nullable: true);
        }
    }
}
