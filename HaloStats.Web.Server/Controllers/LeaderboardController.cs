using HaloStats.Web.Server.Domain.Services;
using HaloStats.Web.Shared.Contracts.Leaderboard;
using HaloStats.Web.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HaloStats.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {

        private readonly IGetLeaderboardService getLeaderboardService;

        public LeaderboardController(IGetLeaderboardService getLeaderboardService)
        {
            this.getLeaderboardService = getLeaderboardService;
        }

        [HttpGet]
        [Route("{game}")]
        public Task<GetLeaderboardResponse> GetLeaderboard(HaloGames game)
        {
            return getLeaderboardService.GetLeaderboard(game);
        }

        [HttpGet]
        [Route("{game}/top-players")]
        public Task<GetLeaderboardPlayersResponse> SearchTopPlayers(HaloGames game, [FromQuery] GetLeaderboardPlayersRequest request)
        {
            return getLeaderboardService.SearchTopPlayers(game, request);
        }
    }
}
