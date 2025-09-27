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
    Task<TopTeammatePairs> GetTopTeammatePairs(string? gamertag, HaloGames game, int take = 10);
    Task<TopTeammatePairs> GetTopTeammatePairsByWins(string? gamertag, HaloGames game, int take = 10);
    Task<TopOpponents> GetTopOpponents(string? gamertag, HaloGames game, int take = 10);

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
            .GroupBy(gp => gp.Gamertag)
            .Select(g => new {
                Gamertag = g.Key,
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
                Gamertag = p.Gamertag,
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
            gamesFilter = gamesFilter.Where(g => gameEnum == null || g.GameEnum == gameEnum );
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
            .GroupBy(gp => gp.Gamertag)
            .Select(g => new {
                Gamertag = g.Key,
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
                Gamertag = p.Gamertag,
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

    public async Task<TopTeammatePairs> GetTopTeammatePairs(string? gamertag, HaloGames game, int take = 10)
    {
        var gameEnum = game.MapToGameEnum();
        var pairs = await db.Set<Usp_GetTopTeammatePairs>()
            .FromSqlRaw("SELECT * FROM usp_gettopteammatepairs({0},{1},{2})", gamertag, gameEnum, take)
            .ToListAsync();
        return new TopTeammatePairs
        {
            Pairs = pairs.Select((p, i) => p.ToTopTeammatePairs(i + 1)).ToList()
        };
    }

    public async Task<TopTeammatePairs> GetTopTeammatePairsByWins(string? gamertag, HaloGames game, int take = 10)
    {
        var gameEnum = game.MapToGameEnum();
        var pairs = await db.Set<Usp_GetTopTeammatePairs>()
            .FromSqlRaw("SELECT * FROM usp_gettopteammatepairsbywins({0},{1},{2})", gamertag, gameEnum, take)
            .ToListAsync();
        return new TopTeammatePairs
        {
            Pairs = pairs.Select((p, i) => p.ToTopTeammatePairs(i + 1)).ToList()
        };
    }


    public async Task<TopOpponents> GetTopOpponents(string? gamertag, HaloGames game, int take = 10)
    {
        var gameEnum = game.MapToGameEnum();
        var ops = await db.Set<Usp_GetTopOpponentPairs>()
            .FromSqlRaw("SELECT * FROM usp_gettopopponents({0},{1},{2})", gamertag, gameEnum, take)
            .ToListAsync();
        return new TopOpponents
        {
            Opponents = ops.Select((p, i) => p.ToTopOpponentRecord(i + 1)).ToList()
        };
    }
}
