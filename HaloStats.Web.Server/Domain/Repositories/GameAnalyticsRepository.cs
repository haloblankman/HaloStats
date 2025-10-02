using HaloStats.Database;
using HaloStats.Database.Entities.StoredProcs;
using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Shared.Contracts.Leaderboard;
using HaloStats.Web.Shared.Contracts.Shared;
using HaloStats.Web.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGameAnalyticsRepository
{
    Task<List<Usp_SearchTopPlayers>> SearchTopPlayers(HaloGames game, GetLeaderboardPlayersRequest request);
    Task<List<Usp_SearchTopTeammatePairs>> SearchTopTeammatePairs(HaloGames game, GetLeaderboardTeammatePairsRequest request);
    Task<TopOpponents> GetTopOpponents(string? gamertag, HaloGames game, int take = 10);

}

public class GameAnalyticsRepository : IGameAnalyticsRepository
{
    private readonly HaloStatsDbContext db;
    public GameAnalyticsRepository(HaloStatsDbContext db)
    {
        this.db = db;
    }

    public async Task<List<Usp_SearchTopPlayers>> SearchTopPlayers(HaloGames game, GetLeaderboardPlayersRequest request)
    {
        var gameEnum = game.MapToGameEnum();
        string sortDir = request.SortDirectionStringDefaultDesc;
        var topPlayers = await db.Set<Usp_SearchTopPlayers>()
            .FromSqlRaw("SELECT * FROM usp_searchtopplayers({0}::smallint,{1},{2}::text,{3},{4},{5},{6})", 
                gameEnum, 
                request.IsMonthly, 
                request.SearchGamertag, 
                request.SortedBy, 
                sortDir, 
                request.PageNumber, 
                request.PageSize)
            .ToListAsync();

        return topPlayers;
    }

    public async Task<List<Usp_SearchTopTeammatePairs>> SearchTopTeammatePairs(HaloGames game, GetLeaderboardTeammatePairsRequest request)
    {
        var gameEnum = game.MapToGameEnum();
        string sortDir = request.SortDirectionStringDefaultDesc;
        var topPairs = await db.Set<Usp_SearchTopTeammatePairs>()
            .FromSqlRaw("SELECT * FROM usp_searchtopteammatepairs({0}::smallint,{1},{2},{3},{4}::text,{5},{6},{7},{8},{9})",
                gameEnum,
                request.IsMonthly ?? false,
                request.IsMatchmaking ?? false,
                request.IsCustoms ?? false,
                request.SearchGamertag,
                5,
                request.SortedBy ?? "WinRate",
                sortDir,
                request.PageNumber,
                request.PageSize)
            .ToListAsync();
        return topPairs;
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
