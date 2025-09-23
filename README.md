# Halo Post Carnage Report Tracker

The **Halo Post Carnage Report (PCR) Tracker** is an application for tracking and viewing your Halo Post Carnage Reports.

## Download

To get started:
1. Visit the [Releases](https://github.com/haloblankman/HaloStats/releases) page.
2. Download the latest version.

Enjoy tracking your carnage reports!

---

## Contributing

### Prerequisites
- [PostgreSQL](https://www.postgresql.org/download/)  
- [.NET SDK](https://dotnet.microsoft.com/download)

### Setup Instructions
1. **Database Initialization**  
   - Install PostgreSQL.  
   - Run the [start.sql](HaloStats.Database/Sql/start.sql) script.  
     > ⚠️ Make sure to review the comments in `start.sql` before running it.

2. **Entity Framework Migrations**  
   - Open a PowerShell terminal in the solution root directory.  
   - Install the EF Core CLI tools (if not already installed):  
     ```bash
     dotnet tool install --global dotnet-ef
     ```
   - Update `appsettings.json` in the `HaloStats.Web.Server` project.  
     Set the `DefaultConnection` string to use the `halostatsdeploy` user.  
   - Apply migrations:  
     ```bash
     dotnet ef database update --project HaloStats.Database --startup-project HaloStats.Web.Server
     ```

3. **Verify Database**  
   The command should create all required tables in the `halostatsdb` PostgreSQL database.

4. **Run the Application**  
   You can now run the app locally.

---

## License
This project is licensed under the [MIT License](License).
