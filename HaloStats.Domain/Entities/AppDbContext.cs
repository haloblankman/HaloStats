using Microsoft.EntityFrameworkCore;

namespace HaloStats.Domain.Entities
{
    public class AppDbContext : DbContext
    {
        //public DbSet<Game> Games { get; set; }
        //public DbSet<PlayerScore> PlayerScores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=myAppData.db");
        }
    }
}
