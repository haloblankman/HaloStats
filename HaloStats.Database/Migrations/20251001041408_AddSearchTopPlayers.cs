using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaloStats.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchTopPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = File.ReadAllText("Sql/StoredProcedures/Usp_SearchTopPlayers.sql");
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = File.ReadAllText("Sql/StoredProcedures/Usp_SearchTopPlayers_down.sql");
            migrationBuilder.Sql(sql);
        }
    }
}
