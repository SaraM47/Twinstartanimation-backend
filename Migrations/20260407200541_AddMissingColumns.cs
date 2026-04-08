using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twinstartanimation_backend.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Chapters_ChapterId",
                table: "Videos");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "Videos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Videos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                table: "Videos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Series",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PageNumber",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StripeSessionId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "ExternalLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Chapters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Videos_SeriesId",
                table: "Videos",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Chapters_ChapterId",
                table: "Videos",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Series_SeriesId",
                table: "Videos",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Chapters_ChapterId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Series_SeriesId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_SeriesId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "PageNumber",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "StripeSessionId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "ExternalLinks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Chapters");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Chapters_ChapterId",
                table: "Videos",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
