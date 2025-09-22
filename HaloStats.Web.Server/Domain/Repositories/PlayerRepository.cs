using HaloStats.Database;
using HaloStats.Web.Shared.Contracts.Player;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IPlayerRepository
{
    Task<PlayerSummary> GetPlayerSummary(string gamerTag);
    Task<GamesPlayed> GetPlayerGameHistory(string gamerTag, int pageNumber, int pageSize);
}

public class PlayerRepository : IPlayerRepository
{
    private readonly HaloStatsDbContext db;
    public PlayerRepository(HaloStatsDbContext db)
    {
        this.db = db;
    }

    public async Task<PlayerSummary> GetPlayerSummary(string gamerTag)
    {
        var summary = await db.GamePlayers
            .Join(db.Games.Where(g => !g.IsDuplicateGame),
                gp => gp.GameId,
                g => g.GameId,
                (gp, g) => gp)
            .Where(gp => gp.GamerTag == gamerTag)
            .GroupBy(gp => gp.GamerTag)
            .Select(g => new
            {
                GamesPlayed = g.Count(),
                TotalWins = g.Count(x => x.IsWinner),
                TotalKills = g.Sum(x => x.Kills),
                TotalDeaths = g.Sum(x => x.Deaths),
                TotalAssists = g.Sum(x => x.Assists)
            })
            .FirstOrDefaultAsync();

        decimal winPercentage = 0;
        decimal killDeathRatio = 0;
        decimal kadRatio = 0;
        if (summary != null)
        {
            winPercentage = summary.GamesPlayed == 0 ? 0 : (decimal)summary.TotalWins / summary.GamesPlayed;
            killDeathRatio = summary.TotalDeaths == 0 ? summary.TotalKills : (decimal)summary.TotalKills / summary.TotalDeaths;
            kadRatio = summary.TotalDeaths == 0 ? (summary.TotalKills + summary.TotalAssists) : (decimal)(summary.TotalKills + summary.TotalAssists) / summary.TotalDeaths;
        }

        return new PlayerSummary
        {
            GamesPlayedCount = summary?.GamesPlayed ?? 0,
            TotalWins = summary?.TotalWins ?? 0,
            TotalKills = summary?.TotalKills ?? 0,
            TotalAssists = summary?.TotalAssists ?? 0,
            TotalDeaths = summary?.TotalDeaths ?? 0,
            WinPercentage = Math.Round(winPercentage * 100, 2),
            KillDeathRatio = Math.Round(killDeathRatio, 2),
            KillAssistDeathRatio = Math.Round(kadRatio, 2)
        };
    }

    public async Task<GamesPlayed> GetPlayerGameHistory(string gamerTag, int pageNumber, int pageSize)
    {
        var gamesPlayed = await db.GamePlayers
            .Join(db.Games.Where(g => !g.IsDuplicateGame),
                gp => gp.GameId,
                g => g.GameId,
                (gp, g) => new { gp, g })
            .Where(g => g.gp.GamerTag == gamerTag)
            .OrderByDescending(x => x.g.ReportedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize + 1)
            .Select(x => new GamePlayed
            {
                GameId = x.gp.GameId,
                ReportedAt = x.g.ReportedAt,
                Score = x.gp.Score,
                Kills = x.gp.Kills,
                Assists = x.gp.Assists,
                Deaths = x.gp.Deaths
            }).ToListAsync();


        return new GamesPlayed
        {
            Games = gamesPlayed.Take(pageSize).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            IsNextPage = gamesPlayed.Count() > pageSize
        };
    }
}
