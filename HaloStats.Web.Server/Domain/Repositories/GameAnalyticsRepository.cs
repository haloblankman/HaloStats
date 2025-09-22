using HaloStats.Database;
using HaloStats.Web.Shared.Contracts.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGameAnalyticsRepository
{
    Task<TopPlayersByKillCount> GetTopPlayersByKillCount();
    Task<TopPlayersByKillCount> GetTopPlayersByKillCountThisMonth();
    Task<TopTeammatePairs> GetTopTeammatePairs();
    Task<TopTeammatePairs> GetTopTeammatePairsByWins();

}

public class GameAnalyticsRepository : IGameAnalyticsRepository
{
    private readonly HaloStatsDbContext db;
    public GameAnalyticsRepository(HaloStatsDbContext db)
    {
        this.db = db;
    }

    public async Task<TopPlayersByKillCount> GetTopPlayersByKillCount()
    {
        var players = await db.GamePlayers
            .Join(db.Games.Where(g => !g.IsDuplicateGame),
                gp => gp.GameId,
                g => g.GameId,
                (gp, g) => gp)
            .GroupBy(gp => gp.GamerTag)
            .Select(g => new {
                GamerTag = g.Key,
                Kills = g.Sum(x => x.Kills),
                Deaths = g.Sum(x => x.Deaths),
                KillDeathRatio = g.Sum(x => x.Deaths) == 0
                    ? g.Sum(x => x.Kills)
                    : (decimal)g.Sum(x => x.Kills) / g.Sum(x => x.Deaths)
            })
            .OrderByDescending(x => x.Kills)
            .Take(10)
            .ToListAsync();

        var rankedPlayers = players
            .Select((p, i) => new PlayerByKillCount
            {
                Rank = i + 1,
                GamerTag = p.GamerTag,
                Kills = p.Kills,
                Deaths = p.Deaths,
                KillDeathRatio = p.KillDeathRatio
            })
            .ToList();

        var result = new TopPlayersByKillCount
        {
            Players = rankedPlayers
        };
        return result;
    }

    public async Task<TopPlayersByKillCount> GetTopPlayersByKillCountThisMonth()
    {
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

        var players = await db.GamePlayers
            .Join(db.Games.Where(g => !g.IsDuplicateGame && g.ReportedAt >= oneMonthAgo),
                gp => gp.GameId,
                g => g.GameId,
                (gp, g) => gp)
            .GroupBy(gp => gp.GamerTag)
            .Select(g => new {
                GamerTag = g.Key,
                Kills = g.Sum(x => x.Kills),
                Deaths = g.Sum(x => x.Deaths),
                KillDeathRatio = g.Sum(x => x.Deaths) == 0
                    ? g.Sum(x => x.Kills)
                    : (decimal)g.Sum(x => x.Kills) / g.Sum(x => x.Deaths)
            })
            .OrderByDescending(x => x.Kills)
            .Take(10)
            .ToListAsync();

        var rankedPlayers = players
            .Select((p, i) => new PlayerByKillCount
            {
                Rank = i + 1,
                GamerTag = p.GamerTag,
                Kills = p.Kills,
                Deaths = p.Deaths,
                KillDeathRatio = p.KillDeathRatio
            })
            .ToList();

        var result = new TopPlayersByKillCount
        {
            Players = rankedPlayers
        };
        return result;
    }

    public async Task<TopTeammatePairs> GetTopTeammatePairs()
    {
        var teammatePairs = await db.GamePlayers
            .Join(db.Games.Where(g => !g.IsDuplicateGame && g.IsTeamsEnabled),
                gp => gp.GameId,
                g => g.GameId,
                (gp, g) => new { gp, g })
            .GroupBy(x => x.gp.GameId)
            .SelectMany(g => g
                .SelectMany((p1, i) => g.Skip(i + 1)
                    .Where(p2 => p1.gp.TeamId == p2.gp.TeamId)
                    .Select(p2 => new
                    {
                        PlayerOne = string.Compare(p1.gp.GamerTag, p2.gp.GamerTag) < 0 ? p1.gp.GamerTag : p2.gp.GamerTag,
                        PlayerTwo = string.Compare(p1.gp.GamerTag, p2.gp.GamerTag) < 0 ? p2.gp.GamerTag : p1.gp.GamerTag,
                        GameId = p1.gp.GameId,
                        IsWin = p1.gp.IsWinner == true
                    })
                )
            )
            .GroupBy(x => new { x.PlayerOne, x.PlayerTwo })
            .Select(g => new
            {
                PlayerOne = g.Key.PlayerOne,
                PlayerTwo = g.Key.PlayerTwo,
                GamesPlayedTogether = g.Count(),
                WinsTogether = g.Count(x => x.IsWin),
                LossesTogether = g.Count(x => !x.IsWin),
                WinRate = g.Count() == 0 ? 0 : (decimal)g.Count(x => x.IsWin) / g.Count()
            })
            .OrderByDescending(x => x.GamesPlayedTogether)
            .ThenByDescending(x => x.WinsTogether)
            .Take(10)
            .ToListAsync();

        var rankedPairs = teammatePairs
            .Select((p, i) => new TeammatePair
            {
                Rank = i + 1,
                PlayerOneGamerTag = p.PlayerOne,
                PlayerTwoGamerTag = p.PlayerTwo,
                GamesPlayedTogether = p.GamesPlayedTogether,
                WinsTogether = p.WinsTogether,
                WinRate = p.WinRate,
                LossesTogether = p.LossesTogether
            })
            .ToList();

        return new TopTeammatePairs
        {
            Pairs = rankedPairs
        };
    }

    public async Task<TopTeammatePairs> GetTopTeammatePairsByWins()
    {
        var teammatePairs = await db.GamePlayers
            .Join(db.Games.Where(g => !g.IsDuplicateGame && g.IsTeamsEnabled),
                gp => gp.GameId,
                g => g.GameId,
                (gp, g) => new { gp, g })
            .GroupBy(x => x.gp.GameId)
            .SelectMany(g => g
                .SelectMany((p1, i) => g.Skip(i + 1)
                    .Where(p2 => p1.gp.TeamId == p2.gp.TeamId)
                    .Select(p2 => new
                    {
                        PlayerOne = string.Compare(p1.gp.GamerTag, p2.gp.GamerTag) < 0 ? p1.gp.GamerTag : p2.gp.GamerTag,
                        PlayerTwo = string.Compare(p1.gp.GamerTag, p2.gp.GamerTag) < 0 ? p2.gp.GamerTag : p1.gp.GamerTag,
                        GameId = p1.gp.GameId,
                        IsWin = p1.gp.IsWinner == true
                    })
                )
            )
            .GroupBy(x => new { x.PlayerOne, x.PlayerTwo })
            .Select(g => new
            {
                PlayerOne = g.Key.PlayerOne,
                PlayerTwo = g.Key.PlayerTwo,
                GamesPlayedTogether = g.Count(),
                WinsTogether = g.Count(x => x.IsWin),
                LossesTogether = g.Count(x => !x.IsWin),
                WinRate = g.Count() == 0 ? 0 : (decimal)g.Count(x => x.IsWin) / g.Count()
            })
            .OrderByDescending(x => x.WinsTogether)
            .ThenByDescending(x => x.GamesPlayedTogether)
            .Take(10)
            .ToListAsync();

        var rankedPairs = teammatePairs
            .Select((p, i) => new TeammatePair
            {
                Rank = i + 1,
                PlayerOneGamerTag = p.PlayerOne,
                PlayerTwoGamerTag = p.PlayerTwo,
                GamesPlayedTogether = p.GamesPlayedTogether,
                WinsTogether = p.WinsTogether,
                LossesTogether = p.LossesTogether,
                WinRate = p.WinRate
            })
            .ToList();

        return new TopTeammatePairs
        {
            Pairs = rankedPairs
        };
    }
}
