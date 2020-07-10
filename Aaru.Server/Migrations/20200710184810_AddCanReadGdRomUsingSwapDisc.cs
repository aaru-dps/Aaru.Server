using Microsoft.EntityFrameworkCore.Migrations;

namespace Aaru.Server.Migrations
{
    public partial class AddCanReadGdRomUsingSwapDisc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) =>
            migrationBuilder.AddColumn<bool>("CanReadGdRomUsingSwapDisc", "Devices", nullable: true, defaultValue: null);

        protected override void Down(MigrationBuilder migrationBuilder) =>
            migrationBuilder.DropColumn("CanReadGdRomUsingSwapDisc", "Devices");
    }
}