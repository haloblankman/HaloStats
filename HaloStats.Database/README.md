Add new migration
`dotnet ef migrations add <MigrationName> --project HaloStats.Database --startup-project HaloStats.Web.Server`

Apply migrations
`dotnet ef database update --project HaloStats.Database --startup-project HaloStats.Web.Server`

EF migrations are stored in the `Migrations` folder of the `HaloStats.Database` project.
Make sure to update the connection string in `appsettings.json` of the `HaloStats.Web.Server` project if necessary.

halostatsdeploy is the postgres user for database deployments.
halostatsapp is the postgres user for application access.
