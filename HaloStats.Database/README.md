Add new migration
`dotnet ef migrations add <MigrationName> --project HaloStats.Database --startup-project HaloStats.Web.Server`

View migrations
`dotnet ef migrations list --project HaloStats.Database --startup-project HaloStats.Web.Server`

Apply migrations
`dotnet ef database update --project HaloStats.Database --startup-project HaloStats.Web.Server`

EF migrations are stored in the `Migrations` folder of the `HaloStats.Database` project.
Make sure to update the connection string in `appsettings.json` of the `HaloStats.Web.Server` project if necessary.

halostatsdeploy is the postgres user for database deployments.
halostatsapp is the postgres user for application access.

Adding a stored procedure:
1. Place the stored procedure SQL file in the HaloStats.Web.Server/Sql/StoredProcedures directory. This is because the migrations are applied from the HaloStats.Web.Server project on load.
2. Add a new migration using the command above. The migration will *not* pick up the new stored procedure automatically.
3. Open the newly created migration file in the HaloStats.Database project and add the following code to the 'Up' method:
   ```csharp
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
   ```
4. Apply the migration using the command above.