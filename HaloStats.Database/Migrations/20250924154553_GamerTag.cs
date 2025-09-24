using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaloStats.Database.Migrations
{
    /// <inheritdoc />
    public partial class GamerTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "GameEnum",
                table: "Games",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "GamerTags",
                columns: table => new
                {
                    GamerTagId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    XboxUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamerTags", x => x.GamerTagId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamerTags_Name",
                table: "GamerTags",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamerTags");

            migrationBuilder.AlterColumn<int>(
                name: "GameEnum",
                table: "Games",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");
        }
    }
}
