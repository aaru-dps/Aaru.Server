using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscImageChef.Server.Migrations
{
    public partial class CascadeDeleteMmcFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_TestedMedia_Mmc_MmcId", "TestedMedia");

            migrationBuilder.AddForeignKey("FK_TestedMedia_Mmc_MmcId", "TestedMedia", "MmcId", "Mmc",
                                           principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_TestedMedia_Mmc_MmcId", "TestedMedia");

            migrationBuilder.AddForeignKey("FK_TestedMedia_Mmc_MmcId", "TestedMedia", "MmcId", "Mmc",
                                           principalColumn: "Id", onDelete: ReferentialAction.SetNull);
        }
    }
}