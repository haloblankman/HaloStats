using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Constants;
using HaloStats.Web.Shared.Contracts.Leaderboard;
using HaloStats.Web.Shared.Enums;
using HaloStats.Web.Shared.Infrastructure;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGetLeaderboardService
{
    Task<GetLeaderboardResponse> GetLeaderboard(HaloGames game);
    Task<GetLeaderboardPlayersResponse> SearchTopPlayers(HaloGames game, GetLeaderboardPlayersRequest request);
}

public class GetLeaderboardService : IGetLeaderboardService
{
    private readonly IGameAnalyticsRepository gameAnalyticsRepo;

    public GetLeaderboardService(IGameAnalyticsRepository gameAnalyticsRepo)
    {
        this.gameAnalyticsRepo = gameAnalyticsRepo;
    }

    public async Task<GetLeaderboardResponse> GetLeaderboard(HaloGames game)
    {
        var topPlayersByKillCountThisMonth = await gameAnalyticsRepo.SearchTopPlayers(game,
            new GetLeaderboardPlayersRequest
            {
                IsMonthly = true,
                PageNumber = 1,
                PageSize = PageSettings.Leaderboard.DefaultLeaderboardPlayersPageSize,
                SortDirection = SortDirection.Descending,
                SortedBy = PageSettings.Leaderboard.DefaultSortBy
            });
        var topTeammatePairs = await gameAnalyticsRepo.GetTopTeammatePairs(null, game);
        var topTeammatePairsByWins = await gameAnalyticsRepo.GetTopTeammatePairsByWins(null, game);

        return new GetLeaderboardResponse
        {
            Game = game,
            TopPlayersThisMonth = topPlayersByKillCountThisMonth,
            TopTeammatePairs = topTeammatePairs,
            TopTeammatePairsByWins = topTeammatePairsByWins
        };
    }

    public async Task<GetLeaderboardPlayersResponse> SearchTopPlayers(HaloGames game, GetLeaderboardPlayersRequest request)
    {
        var response = await gameAnalyticsRepo.SearchTopPlayers(game, request);
        return response;
    }
}
