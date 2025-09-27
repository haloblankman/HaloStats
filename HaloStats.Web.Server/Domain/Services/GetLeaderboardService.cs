using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Leaderboard;
using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGetLeaderboardService
{
    Task<GetLeaderboardResponse> GetLeaderboard(HaloGames game);
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
        var topPlayersByKillCount = await gameAnalyticsRepo.GetTopPlayersByKillCount(game);
        var topPlayersByKillCountThisMonth = await gameAnalyticsRepo.GetTopPlayersByKillCountThisMonth(game);
        var topTeammatePairs = await gameAnalyticsRepo.GetTopTeammatePairs(null, game);
        var topTeammatePairsByWins = await gameAnalyticsRepo.GetTopTeammatePairsByWins(null, game);

        return new GetLeaderboardResponse
        {
            Game = game,
            TopPlayersByKillCount = topPlayersByKillCount,
            TopPlayersByKillCountThisMonth = topPlayersByKillCountThisMonth,
            TopTeammatePairs = topTeammatePairs,
            TopTeammatePairsByWins = topTeammatePairsByWins
        };
    }
}
