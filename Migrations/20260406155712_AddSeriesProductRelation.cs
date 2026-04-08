using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twinstartanimation_backend.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSeriesProductRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Series",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Series_ProductId",
                table: "Series",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Products_ProductId",
                table: "Series",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_Products_ProductId",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_ProductId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Series");
        }
    }
}
