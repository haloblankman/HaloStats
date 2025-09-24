using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaloStats.Database.Migrations
{
    /// <inheritdoc />
    public partial class GameIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Games",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByIp",
                table: "Games",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ReportedAt",
                table: "Games",
                column: "ReportedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_ReportedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DeletedByIp",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Games");
        }
    }
}
