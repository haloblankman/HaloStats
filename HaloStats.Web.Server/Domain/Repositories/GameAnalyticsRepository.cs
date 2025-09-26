using HaloStats.Database;
using HaloStats.Database.Entities;
using HaloStats.Database.Entities.StoredProcs;
using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Shared.Contracts.Shared;
using HaloStats.Web.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGameAnalyticsRepository
{
    Task<TopPlayersByKillCount> GetTopPlayersByKillCount(HaloGames game);
    Task<TopPlayersByKillCount> GetTopPlayersByKillCountThisMonth(HaloGames game);
    Task<TopTeammatePairs> GetTopTeammatePairs(HaloGames game);
    Task<TopTeammatePairs> GetTopTeammatePairsByWins(HaloGames game);

}

public class GameAnalyticsRepository : IGameAnalyticsRepository
{
    private readonly HaloStatsDbContext db;
    public GameAnalyticsRepository(HaloStatsDbContext db)
    {
        this.db = db;
    }

    public async Task<TopPlayersByKillCount> GetTopPlayersByKillCount(HaloGames game)
    {
        var gamesFilter = GetGameFilter(game);
        var players = await db.GamePlayers
            .Join(gamesFilter,
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

    private IQueryable<Game> GetGameFilter(HaloGames game)
    {
        var gamesFilter = db.Games.AsNoTracking().Where(g => !g.IsDuplicateGame && !g.IsDeleted);
        if (game != HaloGames.HaloMccAll)
        {
            var gameEnum = game.MapToGameEnum();
            gamesFilter = gamesFilter.Where(g => g.GameEnum == gameEnum);
        }
        return gamesFilter;
    }

    public async Task<TopPlayersByKillCount> GetTopPlayersByKillCountThisMonth(HaloGames game)
    {
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
        var gameFilter = GetGameFilter(game);

        var players = await db.GamePlayers
            .Join(gameFilter.Where(g => g.ReportedAt >= oneMonthAgo),
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

    public async Task<TopTeammatePairs> GetTopTeammatePairs(HaloGames game)
    {
        var gameEnum = (int)game; // or -1 for all
        var pairs = await db.Usp_GetTopTeammatePairs
            .FromSqlRaw("SELECT * FROM GetTopTeammatePairs({0})", gameEnum)
            .ToListAsync();
        return new TopTeammatePairs
        {
            Pairs = pairs.Select((p, i) => p.ToTopTeammatePairs(i + 1)).ToList()
        };

        var gamesFilter = GetGameFilter(game);
        var gamePlayers = await db.GamePlayers
        .AsNoTracking()
        .Join(
            gamesFilter.Where(g => g.IsTeamsEnabled),
            gp => gp.GameId,
            g => g.GameId,
            (gp, g) => new { gp.GameId, gp.GamerTag, gp.TeamId, gp.IsWinner }
        )
        .ToListAsync();

        // Step 2: Group by GameId and TeamId (in memory)
        var pairStats = new Dictionary<(string, string), (int games, int wins, int losses)>();

        var grouped = gamePlayers
            .GroupBy(x => new { x.GameId, x.TeamId });

        foreach (var teamGroup in grouped)
        {
            var players = teamGroup.ToList();
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = i + 1; j < players.Count; j++)
                {
                    var p1 = players[i];
                    var p2 = players[j];
                    // Always order the pair alphabetically to avoid duplicates
                    var playerOne = string.Compare(p1.GamerTag, p2.GamerTag, StringComparison.Ordinal) < 0 ? p1.GamerTag : p2.GamerTag;
                    var playerTwo = string.Compare(p1.GamerTag, p2.GamerTag, StringComparison.Ordinal) < 0 ? p2.GamerTag : p1.GamerTag;
                    var key = (playerOne, playerTwo);

                    bool isWin = p1.IsWinner && p2.IsWinner;
                    if (!pairStats.TryGetValue(key, out var stats))
                        stats = (0, 0, 0);

                    stats.games++;
                    if (isWin) stats.wins++;
                    else stats.losses++;

                    pairStats[key] = stats;
                }
            }
        }

        // Step 3: Project and rank
        var rankedPairs = pairStats
            .Select((kv, idx) => new
            {
                PlayerOne = kv.Key.Item1,
                PlayerTwo = kv.Key.Item2,
                GamesPlayedTogether = kv.Value.games,
                WinsTogether = kv.Value.wins,
                LossesTogether = kv.Value.losses,
                WinRate = kv.Value.games == 0 ? 0 : (decimal)kv.Value.wins / kv.Value.games
            })
            .OrderByDescending(x => x.GamesPlayedTogether)
            .ThenByDescending(x => x.WinsTogether)
            .Take(10)
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

    public async Task<TopTeammatePairs> GetTopTeammatePairsByWins(HaloGames game)
    {
        var gamesFilter = GetGameFilter(game);
        var gamePlayers = await db.GamePlayers
            .AsNoTracking()
            .Join(
                gamesFilter.Where(g => g.IsTeamsEnabled),
                gp => gp.GameId,
                g => g.GameId,
                (gp, g) => new { gp.GameId, gp.GamerTag, gp.TeamId, gp.IsWinner }
            )
            .ToListAsync();

        // Step 2: Group by GameId and TeamId (in memory)
        var pairStats = new Dictionary<(string, string), (int games, int wins, int losses)>();

        var grouped = gamePlayers
            .GroupBy(x => new { x.GameId, x.TeamId });

        foreach (var teamGroup in grouped)
        {
            var players = teamGroup.ToList();
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = i + 1; j < players.Count; j++)
                {
                    var p1 = players[i];
                    var p2 = players[j];
                    // Always order the pair alphabetically to avoid duplicates
                    var playerOne = string.Compare(p1.GamerTag, p2.GamerTag, StringComparison.Ordinal) < 0 ? p1.GamerTag : p2.GamerTag;
                    var playerTwo = string.Compare(p1.GamerTag, p2.GamerTag, StringComparison.Ordinal) < 0 ? p2.GamerTag : p1.GamerTag;
                    var key = (playerOne, playerTwo);

                    bool isWin = p1.IsWinner && p2.IsWinner;
                    if (!pairStats.TryGetValue(key, out var stats))
                        stats = (0, 0, 0);

                    stats.games++;
                    if (isWin) stats.wins++;
                    else stats.losses++;

                    pairStats[key] = stats;
                }
            }
        }

        // Step 3: Project and rank by WinsTogether, then GamesPlayedTogether
        var rankedPairs = pairStats
            .Select((kv, idx) => new
            {
                PlayerOne = kv.Key.Item1,
                PlayerTwo = kv.Key.Item2,
                GamesPlayedTogether = kv.Value.games,
                WinsTogether = kv.Value.wins,
                LossesTogether = kv.Value.losses,
                WinRate = kv.Value.games == 0 ? 0 : (decimal)kv.Value.wins / kv.Value.games
            })
            .OrderByDescending(x => x.WinsTogether)
            .ThenByDescending(x => x.GamesPlayedTogether)
            .Take(10)
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
