using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twinstartanimation_backend.API.Migrations
{
    /// <inheritdoc />
    public partial class AddComicFieldsToSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Authors",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genres",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authors",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Series");
        }
    }
}
