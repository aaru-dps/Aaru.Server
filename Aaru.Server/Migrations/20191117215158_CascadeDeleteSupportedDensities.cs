using Microsoft.EntityFrameworkCore.Migrations;

namespace Aaru.Server.Migrations
{
    public partial class CascadeDeleteSupportedDensities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_SupportedDensity_Ssc_SscId", "SupportedDensity");

            migrationBuilder.DropForeignKey("FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                                            "SupportedDensity");

            migrationBuilder.AddForeignKey("FK_SupportedDensity_Ssc_SscId", "SupportedDensity", "SscId", "Ssc",
                                           principalColumn: "Id", onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey("FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                                           "SupportedDensity", "TestedSequentialMediaId", "TestedSequentialMedia",
                                           principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_SupportedDensity_Ssc_SscId", "SupportedDensity");

            migrationBuilder.DropForeignKey("FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                                            "SupportedDensity");

            migrationBuilder.AddForeignKey("FK_SupportedDensity_Ssc_SscId", "SupportedDensity", "SscId", "Ssc",
                                           principalColumn: "Id", onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey("FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                                           "SupportedDensity", "TestedSequentialMediaId", "TestedSequentialMedia",
                                           principalColumn: "Id", onDelete: ReferentialAction.SetNull);
        }
    }
}