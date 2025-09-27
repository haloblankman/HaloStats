using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Constants;
using HaloStats.Web.Shared.Contracts.Player;
using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Server.Domain.Services
{
    public interface IGetPlayerService
    {
        Task<GetPlayerResponse?> GetPlayer(string gamerTag);
    }

    public class GetPlayerService : IGetPlayerService
    {
        private readonly IPlayerRepository playerRepo;
        private readonly IGameAnalyticsRepository gameAnalyticsRepository;

        public GetPlayerService(IPlayerRepository playerRepo, IGameAnalyticsRepository gameAnalyticsRepository)
        {
            this.playerRepo = playerRepo;
            this.gameAnalyticsRepository = gameAnalyticsRepository;
        }

        public async Task<GetPlayerResponse?> GetPlayer(string gamertag)
        {
            var playerSummary = await playerRepo.GetPlayerSummary(gamertag);
            if (playerSummary == null)
            {
                return null;
            }

            var gameHistory = await playerRepo.GetPlayerGameHistory(gamertag, new GetPlayerGamesRequest { PageNumber = 1, PageSize = PageSettings.DefaultPlayerGamesPageSize });

            var topTeammates = await gameAnalyticsRepository.GetTopTeammatePairs(gamertag, HaloGames.HaloMccAll, 10);
            var topTeammatesByWins = await gameAnalyticsRepository.GetTopTeammatePairsByWins(gamertag, HaloGames.HaloMccAll, 10);
            var topOpponents = await gameAnalyticsRepository.GetTopOpponents(gamertag, HaloGames.HaloMccAll, 10);

            return new GetPlayerResponse
            {
                Gamertag = gamertag,
                PlayerSummary = playerSummary,
                GamesPlayed = gameHistory,
                TopTeammates = topTeammates.ToTopTeammates(),
                TopTeammatesByWins = topTeammatesByWins.ToTopTeammates(),
                TopOpponents = topOpponents
            };
        }
    }
}
