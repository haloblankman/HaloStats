using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaloStats.Database.Migrations
{
    /// <inheritdoc />
    public partial class Usp_TopTeammatePairs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usp_GetTopTeammatePairs",
                columns: table => new
                {
                    PlayerOneGamerTag = table.Column<string>(type: "text", nullable: false),
                    PlayerTwoGamerTag = table.Column<string>(type: "text", nullable: false),
                    GamesPlayedTogether = table.Column<int>(type: "integer", nullable: false),
                    WinsTogether = table.Column<int>(type: "integer", nullable: false),
                    LossesTogether = table.Column<int>(type: "integer", nullable: false),
                    WinRate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usp_GetTopTeammatePairs");
        }
    }
}
