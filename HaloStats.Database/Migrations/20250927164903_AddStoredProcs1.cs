using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaloStats.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcs1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = File.ReadAllText("Sql/StoredProcedures/Usp_GetTopOpponents.sql");
            migrationBuilder.Sql(sql);
            var sql2 = File.ReadAllText("Sql/StoredProcedures/Usp_GetTopTeammatePairs.sql");
            migrationBuilder.Sql(sql2);
            var sql3 = File.ReadAllText("Sql/StoredProcedures/Usp_GetTopTeammatePairsByWins.sql");
            migrationBuilder.Sql(sql3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = File.ReadAllText("Sql/StoredProcedures/Usp_GetTopOpponents_down.sql");
            migrationBuilder.Sql(sql);
            var sql2 = File.ReadAllText("Sql/StoredProcedures/Usp_GetTopTeammatePairs_down.sql");
            migrationBuilder.Sql(sql2);
            var sql3 = File.ReadAllText("Sql/StoredProcedures/Usp_GetTopTeammatePairsByWins_down.sql");
            migrationBuilder.Sql(sql3);
        }
    }
}
