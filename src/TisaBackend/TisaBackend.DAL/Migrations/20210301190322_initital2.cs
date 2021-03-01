using Microsoft.EntityFrameworkCore.Migrations;

namespace TisaBackend.DAL.Migrations
{
    public partial class initital2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirplaneDepartmentTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirplaneDepartmentTypes",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirplaneDepartmentTypes", x => x.Name);
                });
        }
    }
}
