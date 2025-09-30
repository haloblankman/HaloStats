using HaloStats.Database;
using HaloStats.Database.Entities;
using HaloStats.Database.Entities.StoredProcs;
using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Shared.Constants;
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
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
        var gamesFilter = GetGameFilter(game);
        if (request.IsMonthly)
        {
            gamesFilter = gamesFilter.Where(g => g.ReportedAt >= oneMonthAgo);
        }
        var query = db.GamePlayers
            .Join(gamesFilter,
                gp => gp.GameId,
                g => g.GameId,
                (gp, g) => gp)
            .Where(gp => string.IsNullOrEmpty(request.SearchGamertag) || gp.Gamertag == request.SearchGamertag)
            .GroupBy(gp => gp.Gamertag)
            .Select(g => new {
                Gamertag = g.Key,
                Kills = g.Sum(x => x.Kills),
                Deaths = g.Sum(x => x.Deaths),
                KillDeathRatio = g.Sum(x => x.Deaths) == 0
                    ? g.Sum(x => x.Kills)
                    : (decimal)g.Sum(x => x.Kills) / g.Sum(x => x.Deaths)
            });
        
        if (string.IsNullOrWhiteSpace(request.SortedBy) || request.SortedBy == PageSettings.Leaderboard.SortKills)
        {
            if (request.SortDirection is Shared.Infrastructure.SortDirection.None or Shared.Infrastructure.SortDirection.Descending)
                query = query.OrderByDescending(x => x.Kills);
            else
                query = query.OrderBy(x => x.Kills);
        }
        else
        {
            if (request.SortDirection is Shared.Infrastructure.SortDirection.None or Shared.Infrastructure.SortDirection.Descending)
                query = query.OrderByDescending(x => x.KillDeathRatio);
            else
                query = query.OrderBy(x => x.KillDeathRatio);
        }
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var totalPlayers = await query.CountAsync();
        var players = await query
            .Skip((pageNumber - 1) * request.PageSize)
            .Take(request.PageSize + 1)
            .ToListAsync();
        var rankedPlayers = players
            .Select((p, i) => new LeaderboardPlayer
            {
                Rank = request.SortDirection is Shared.Infrastructure.SortDirection.Descending or Shared.Infrastructure.SortDirection.None ? 
                    (pageNumber - 1) * request.PageSize + i + 1 :
                    totalPlayers - ((pageNumber - 1) * request.PageSize + i),
                Gamertag = p.Gamertag,
                Kills = p.Kills,
                Deaths = p.Deaths,
                KillDeathRatio = p.KillDeathRatio
            })
            .ToList();

        bool isNextPage = rankedPlayers.Count() > request.PageSize;
        if (isNextPage)
            rankedPlayers.RemoveAt(rankedPlayers.Count() - 1);

        return new GetLeaderboardPlayersResponse
        {
            Records = rankedPlayers,
            TotalRecords = totalPlayers,
            Parameters  = request,
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
