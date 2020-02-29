using Microsoft.EntityFrameworkCore.Migrations;

namespace Aaru.Server.Migrations
{
    public partial class NameCountModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) =>
            migrationBuilder.Sql("ALTER TABLE Versions CHANGE `Value` `Name` LONGTEXT");

        protected override void Down(MigrationBuilder migrationBuilder) =>
            migrationBuilder.Sql("ALTER TABLE Versions CHANGE `Name` `Value` LONGTEXT");
    }
}