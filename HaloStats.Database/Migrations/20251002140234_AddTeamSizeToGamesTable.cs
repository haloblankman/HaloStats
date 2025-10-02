using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaloStats.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamSizeToGamesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamSize",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KilledMostGamertag",
                table: "GamePlayers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MostKilledByGamertag",
                table: "GamePlayers",
                type: "text",
                nullable: true);

            // 2. Backfill data using SQL
            var sql = File.ReadAllText("Sql/Migrations/20251002_AddTeamSizeAndMostKilledGamertagToGames.sql");
            migrationBuilder.Sql(sql);

            migrationBuilder.Sql(@"UPDATE ""Games"" SET ""TeamSize"" = 0 WHERE ""TeamSize"" IS NULL;");

            // 3. Make column non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "TeamSize",
                table: "Games",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS Usp_GetTopTeammatePairs(text, int, int);");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS Usp_GetTopTeammatePairsByWins(text, int, int);");
            
            var sql2 = File.ReadAllText("Sql/StoredProcedures/Usp_SearchTopTeammatePairs.sql");
            migrationBuilder.Sql(sql2);

            var sql3 = File.ReadAllText("Sql/StoredProcedures/Usp_SearchTopPlayers.sql");
            migrationBuilder.Sql(sql3);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamSize",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "KilledMostGamertag",
                table: "GamePlayers");

            migrationBuilder.DropColumn(
                name: "MostKilledByGamertag",
                table: "GamePlayers");

            var sql = File.ReadAllText("Sql/StoredProcedures/Usp_SearchTopTeammatePairs_down.sql");
            migrationBuilder.Sql(sql);
        }
    }
}
