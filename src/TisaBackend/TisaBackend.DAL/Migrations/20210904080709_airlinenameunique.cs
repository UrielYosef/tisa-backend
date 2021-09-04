using Microsoft.EntityFrameworkCore.Migrations;

namespace TisaBackend.DAL.Migrations
{
    public partial class airlinenameunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AirlineId",
                table: "Reviews",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_Name",
                table: "Airlines",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Airlines_AirlineId",
                table: "Reviews",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Airlines_AirlineId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AirlineId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Airlines_Name",
                table: "Airlines");
        }
    }
}
