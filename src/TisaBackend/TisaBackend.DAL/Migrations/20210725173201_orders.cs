using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TisaBackend.DAL.Migrations
{
    public partial class orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    FlightId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    SeatsQuantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightOrders_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentPrices_DepartmentId",
                table: "DepartmentPrices",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightOrders_FlightId",
                table: "FlightOrders",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentPrices_DepartmentTypes_DepartmentId",
                table: "DepartmentPrices",
                column: "DepartmentId",
                principalTable: "DepartmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentPrices_DepartmentTypes_DepartmentId",
                table: "DepartmentPrices");

            migrationBuilder.DropTable(
                name: "FlightOrders");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentPrices_DepartmentId",
                table: "DepartmentPrices");
        }
    }
}
