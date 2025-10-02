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
        [Route("{game}/top-players")]
        public Task<GetLeaderboardPlayersResponse> SearchTopPlayers(HaloGames game, [FromQuery] GetLeaderboardPlayersRequest request)
        {
            return getLeaderboardService.SearchTopPlayers(game, request);
        }

        [HttpGet]
        [Route("{game}/top-teammate-pairs")]
        public Task<GetLeaderboardTeammatePairsResponse> SearchTopTeammatePairs(HaloGames game, [FromQuery] GetLeaderboardTeammatePairsRequest request)
        {
            return getLeaderboardService.SearchTopTeammatePairs(game, request);
        }
    }
}
