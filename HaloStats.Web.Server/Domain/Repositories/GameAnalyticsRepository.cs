using HaloStats.Database;
using HaloStats.Database.Entities;
using HaloStats.Database.Entities.StoredProcs;
using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Shared.Contracts.Leaderboard;
using HaloStats.Web.Shared.Contracts.Shared;
using HaloStats.Web.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGameAnalyticsRepository
{
    Task<GetLeaderboardPlayersResponse> SearchTopPlayers(HaloGames game, GetLeaderboardPlayersRequest request);
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

    public async Task<GetLeaderboardPlayersResponse> SearchTopPlayers(HaloGames game, GetLeaderboardPlayersRequest request)
    {
        var gameEnum = game.MapToGameEnum();
        string sortDir = request.SortDirection is Shared.Infrastructure.SortDirection.None or Shared.Infrastructure.SortDirection.Descending ? "DESC" : "ASC";
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

        var totalPlayers = topPlayers.Count > 0 ? topPlayers[0].TotalCount : 0;

        var isNextPage = topPlayers.Count > request.PageSize;
        if (isNextPage)
            topPlayers.RemoveAt(topPlayers.Count - 1);

        var records = topPlayers.Select(p => new LeaderboardPlayer
        {
            Rank = p.Rank,
            Gamertag = p.Gamertag,
            Kills = p.Kills,
            Deaths = p.Deaths,
            KillDeathRatio = p.KillDeathRatio
        }).ToList();

        return new GetLeaderboardPlayersResponse
        {
            Records = records,
            TotalRecords = totalPlayers,
            Parameters = request,
            IsNextPage = isNextPage
        };
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
