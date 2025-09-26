using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloStats.Database.Migrations.StoredProcs;

public partial class AddMyStoredProc : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var sql = File.ReadAllText("Sql/StoredProcedures/GetTopTeammatePairs.sql");
        migrationBuilder.Sql(sql);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        var sql = File.ReadAllText("Sql/StoredProcedures/GetTopTeammatePairs_down.sql");
        migrationBuilder.Sql(sql);
    }
}
