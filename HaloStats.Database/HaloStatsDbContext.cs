using HaloStats.Database.Entities;
using HaloStats.Database.Entities.StoredProcs;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Database;

public class HaloStatsDbContext : DbContext
{
    public HaloStatsDbContext(DbContextOptions<HaloStatsDbContext> options) : base(options) { }
    public DbSet<Game> Games { get; set; }
    public DbSet<GamePlayer> GamePlayers { get; set; }
    public DbSet<GamePlayerCustomStat> GamePlayerCustomStats { get; set; }
    public DbSet<GamePlayerMedal> GamePlayerMedals { get; set; }
    public DbSet<Gamertag> Gamertags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasMany(g => g.Players)
            .WithOne()
            .HasForeignKey(gp => gp.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePlayer>()
            .HasMany(gp => gp.CustomStats)
            .WithOne()
            .HasForeignKey(c => c.GamePlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePlayer>()
            .HasMany(gp => gp.Medals)
            .WithOne()
            .HasForeignKey(m => m.GamePlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Game>()
            .HasIndex(g => g.GameUniqueId);

        modelBuilder.Entity<Game>()
            .HasIndex(g => g.ReportedAt);

        modelBuilder.Entity<GamePlayer>()
            .HasIndex(gp => gp.Gamertag);

        modelBuilder.Entity<Gamertag>()
            .HasIndex(gp => gp.Name);

        modelBuilder.Entity<Usp_SearchTopTeammatePairs>().HasNoKey().ToView(null);
        modelBuilder.Entity<Usp_GetTopOpponentPairs>().HasNoKey().ToView(null);
        modelBuilder.Entity<Usp_SearchTopPlayers>().HasNoKey().ToView(null);
    }
}