using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaloStats.Database.Migrations
{
    /// <inheritdoc />
    public partial class GamertagRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GamerTags",
                table: "GamerTags");

            migrationBuilder.RenameTable(
                name: "GamerTags",
                newName: "Gamertags");

            migrationBuilder.RenameColumn(
                name: "GamerTagId",
                table: "Gamertags",
                newName: "GamertagId");

            migrationBuilder.RenameIndex(
                name: "IX_GamerTags_Name",
                table: "Gamertags",
                newName: "IX_Gamertags_Name");

            migrationBuilder.RenameColumn(
                name: "GamerTag",
                table: "GamePlayers",
                newName: "Gamertag");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayers_GamerTag",
                table: "GamePlayers",
                newName: "IX_GamePlayers_Gamertag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gamertags",
                table: "Gamertags",
                column: "GamertagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Gamertags",
                table: "Gamertags");

            migrationBuilder.RenameTable(
                name: "Gamertags",
                newName: "GamerTags");

            migrationBuilder.RenameColumn(
                name: "GamertagId",
                table: "GamerTags",
                newName: "GamerTagId");

            migrationBuilder.RenameIndex(
                name: "IX_Gamertags_Name",
                table: "GamerTags",
                newName: "IX_GamerTags_Name");

            migrationBuilder.RenameColumn(
                name: "Gamertag",
                table: "GamePlayers",
                newName: "GamerTag");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayers_Gamertag",
                table: "GamePlayers",
                newName: "IX_GamePlayers_GamerTag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamerTags",
                table: "GamerTags",
                column: "GamerTagId");
        }
    }
}
