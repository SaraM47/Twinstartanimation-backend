using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twinstartanimation_backend.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderToSortOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Chapters",
                newName: "SortOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "Chapters",
                newName: "Order");
        }
    }
}
