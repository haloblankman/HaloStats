using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Player;

namespace HaloStats.Web.Server.Domain.Services
{
    public interface IGetPlayerService
    {
        Task<GetPlayerResponse?> GetPlayer(string gamerTag);
    }

    public class GetPlayerService : IGetPlayerService
    {
        private readonly IPlayerRepository playerRepo;

        public GetPlayerService(IPlayerRepository playerRepo)
        {
            this.playerRepo = playerRepo;
        }

        public async Task<GetPlayerResponse?> GetPlayer(string gamerTag)
        {
            var playerSummary = await playerRepo.GetPlayerSummary(gamerTag);
            if (playerSummary == null)
            {
                return null;
            }

            var gameHistory = await playerRepo.GetPlayerGameHistory(gamerTag, 1, 100);

            return new GetPlayerResponse
            {
                Gamertag = gamerTag,
                PlayerSummary = playerSummary,
                GamesPlayed = gameHistory
            };
        }
    }
}
