using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Player;

namespace HaloStats.Web.Server.Domain.Services
{
    public interface IGetPlayerGameHistory
    {
        Task<GetPlayerGamesResponse> GetGamesPlayed(string gamerTag, GetPlayerGamesRequest request);
        
    }

    public class GetPlayerGameHistory : IGetPlayerGameHistory
    {
        private readonly IPlayerRepository playerRepo;

        public GetPlayerGameHistory(IPlayerRepository playerRepo)
        {
            this.playerRepo = playerRepo;
        }

        public async Task<GetPlayerGamesResponse> GetGamesPlayed(string gamerTag, GetPlayerGamesRequest request)
        {
            var gameHistory = await playerRepo.GetPlayerGameHistory(gamerTag, request);
            return gameHistory;
        }
    }
}
