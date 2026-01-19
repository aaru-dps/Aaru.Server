using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aaru.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameLiteOnRawToReadBuffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupportsLiteOnReadRawDVD",
                table: "TestedMedia",
                newName: "SupportsReadBuffer3CRawDVD");

            migrationBuilder.RenameColumn(
                name: "LiteOnReadRawDVDData",
                table: "TestedMedia",
                newName: "ReadBuffer3CRawDVDData");

            migrationBuilder.CreateTable(
                name: "CompressedBufferRead",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommandVariant = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompressedData = table.Column<byte[]>(type: "longblob", nullable: true),
                    UncompressedSize = table.Column<uint>(type: "int unsigned", nullable: false),
                    TestedMediaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompressedBufferRead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompressedBufferRead_TestedMedia_TestedMediaId",
                        column: x => x.TestedMediaId,
                        principalTable: "TestedMedia",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CompressedBufferRead_TestedMediaId",
                table: "CompressedBufferRead",
                column: "TestedMediaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompressedBufferRead");

            migrationBuilder.RenameColumn(
                name: "SupportsReadBuffer3CRawDVD",
                table: "TestedMedia",
                newName: "SupportsLiteOnReadRawDVD");

            migrationBuilder.RenameColumn(
                name: "ReadBuffer3CRawDVDData",
                table: "TestedMedia",
                newName: "LiteOnReadRawDVDData");
        }
    }
}
