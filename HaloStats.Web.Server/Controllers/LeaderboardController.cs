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
        public async Task<GetLeaderboardResponse> GetLeaderboard(HaloGames game)
        {
            return await getLeaderboardService.GetLeaderboard(game);
        }
    }
}
