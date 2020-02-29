using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aaru.Server.Migrations
{
    public partial class RemoteStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("RemoteApplications", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name  = table.Column<string>(nullable: true), Version = table.Column<string>(nullable: true),
                Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_RemoteApplications", x => x.Id);
            });

            migrationBuilder.CreateTable("RemoteArchitectures", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(nullable: true), Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_RemoteArchitectures", x => x.Id);
            });

            migrationBuilder.CreateTable("RemoteOperatingSystems", table => new
            {
                Id = table.Column<int>().
                           Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name  = table.Column<string>(nullable: true), Version = table.Column<string>(nullable: true),
                Count = table.Column<long>()
            }, constraints: table =>
            {
                table.PrimaryKey("PK_RemoteOperatingSystems", x => x.Id);
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("RemoteApplications");

            migrationBuilder.DropTable("RemoteArchitectures");

            migrationBuilder.DropTable("RemoteOperatingSystems");
        }
    }
}