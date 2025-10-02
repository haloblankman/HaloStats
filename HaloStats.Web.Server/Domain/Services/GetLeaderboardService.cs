using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Leaderboard;
using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGetLeaderboardService
{
    Task<GetLeaderboardPlayersResponse> SearchTopPlayers(HaloGames game, GetLeaderboardPlayersRequest request);
    Task<GetLeaderboardTeammatePairsResponse> SearchTopTeammatePairs(HaloGames game, GetLeaderboardTeammatePairsRequest request);
}

public class GetLeaderboardService : IGetLeaderboardService
{
    private readonly IGameAnalyticsRepository gameAnalyticsRepo;

    public GetLeaderboardService(IGameAnalyticsRepository gameAnalyticsRepo)
    {
        this.gameAnalyticsRepo = gameAnalyticsRepo;
    }

    public async Task<GetLeaderboardPlayersResponse> SearchTopPlayers(HaloGames game, GetLeaderboardPlayersRequest request)
    {
        var topPlayers = await gameAnalyticsRepo.SearchTopPlayers(game, request);
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

    public async Task<GetLeaderboardTeammatePairsResponse> SearchTopTeammatePairs(HaloGames game, GetLeaderboardTeammatePairsRequest request)
    {
        var pairs = await gameAnalyticsRepo.SearchTopTeammatePairs(game, request);
        var totalRecords = pairs.Count > 0 ? pairs[0].TotalCount : 0;

        var isNextPage = pairs.Count > request.PageSize;
        if (isNextPage)
            pairs.RemoveAt(pairs.Count - 1);

        var records = pairs.Select(p => new LeaderboardTeammatePair
        {
            Rank = p.Rank,
            PlayerOneGamertag = p.PlayerOneGamertag,
            PlayerTwoGamertag = p.PlayerTwoGamertag,
            GamesPlayedTogether = p.GamesPlayedTogether,
            WinsTogether = p.WinsTogether,
            LossesTogether = p.LossesTogether,
            WinRate = p.WinRate,
        }).ToList();

        return new GetLeaderboardTeammatePairsResponse
        {
            Records = records,
            TotalRecords = totalRecords,
            Parameters = request,
            IsNextPage = isNextPage
        };
    }
}
