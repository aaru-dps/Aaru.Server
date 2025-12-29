using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aaru.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUsbVendorIdToProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ushort>(
                name: "UsbVendorId",
                table: "UsbProducts",
                type: "smallint unsigned",
                nullable: false,
                defaultValue: (ushort)0);

            migrationBuilder.Sql(
                "UPDATE UsbProducts p " +
                "INNER JOIN UsbVendors v ON p.VendorId = v.Id " +
                "SET p.UsbVendorId = v.VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsbVendorId",
                table: "UsbProducts");
        }
    }
}
