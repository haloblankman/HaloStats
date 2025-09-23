using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Player;

namespace HaloStats.Web.Server.Domain.Services
{
    public interface IGetPlayerGameHistory
    {
        Task<GamesPlayed> GetGamesPlayed(string gamerTag, int pageNumber, int pageSize);
    }

    public class GetPlayerGameHistory : IGetPlayerGameHistory
    {
        private readonly IPlayerRepository playerRepo;

        public GetPlayerGameHistory(IPlayerRepository playerRepo)
        {
            this.playerRepo = playerRepo;
        }

        public async Task<GamesPlayed> GetGamesPlayed(string gamerTag, int pageNumber, int pageSize)
        {
            var gameHistory = await playerRepo.GetPlayerGameHistory(gamerTag, pageNumber, pageSize);
            return gameHistory;
        }
    }
}
