using Microsoft.EntityFrameworkCore.Migrations;

namespace TisaBackend.DAL.Migrations
{
    public partial class flights_dept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DalDepartmentPrices_Flights_FlightId",
                table: "DalDepartmentPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DalDepartmentPrices",
                table: "DalDepartmentPrices");

            migrationBuilder.RenameTable(
                name: "DalDepartmentPrices",
                newName: "DepartmentPrices");

            migrationBuilder.RenameIndex(
                name: "IX_DalDepartmentPrices_FlightId",
                table: "DepartmentPrices",
                newName: "IX_DepartmentPrices_FlightId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentPrices",
                table: "DepartmentPrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentPrices_Flights_FlightId",
                table: "DepartmentPrices",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentPrices_Flights_FlightId",
                table: "DepartmentPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentPrices",
                table: "DepartmentPrices");

            migrationBuilder.RenameTable(
                name: "DepartmentPrices",
                newName: "DalDepartmentPrices");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentPrices_FlightId",
                table: "DalDepartmentPrices",
                newName: "IX_DalDepartmentPrices_FlightId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DalDepartmentPrices",
                table: "DalDepartmentPrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DalDepartmentPrices_Flights_FlightId",
                table: "DalDepartmentPrices",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
