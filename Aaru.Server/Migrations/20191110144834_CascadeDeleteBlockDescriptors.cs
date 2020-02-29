using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscImageChef.Server.Migrations
{
    public partial class CascadeDeleteBlockDescriptors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_BlockDescriptor_ScsiMode_ScsiModeId", "BlockDescriptor");

            migrationBuilder.AddForeignKey("FK_BlockDescriptor_ScsiMode_ScsiModeId", "BlockDescriptor", "ScsiModeId",
                                           "ScsiMode", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_BlockDescriptor_ScsiMode_ScsiModeId", "BlockDescriptor");

            migrationBuilder.AddForeignKey("FK_BlockDescriptor_ScsiMode_ScsiModeId", "BlockDescriptor", "ScsiModeId",
                                           "ScsiMode", principalColumn: "Id", onDelete: ReferentialAction.SetNull);
        }
    }
}