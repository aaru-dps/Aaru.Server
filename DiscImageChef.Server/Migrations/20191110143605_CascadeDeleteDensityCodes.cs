using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscImageChef.Server.Migrations
{
    public partial class CascadeDeleteDensityCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_DensityCode_SscSupportedMedia_SscSupportedMediaId", "DensityCode");

            migrationBuilder.AddForeignKey("FK_DensityCode_SscSupportedMedia_SscSupportedMediaId", "DensityCode",
                                           "SscSupportedMediaId", "SscSupportedMedia", principalColumn: "Id",
                                           onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_DensityCode_SscSupportedMedia_SscSupportedMediaId", "DensityCode");

            migrationBuilder.AddForeignKey("FK_DensityCode_SscSupportedMedia_SscSupportedMediaId", "DensityCode",
                                           "SscSupportedMediaId", "SscSupportedMedia", principalColumn: "Id",
                                           onDelete: ReferentialAction.SetNull);
        }
    }
}