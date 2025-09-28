using HaloStats.Web.Server.Domain.Services;
using HaloStats.Web.Shared.Contracts.Player;
using Microsoft.AspNetCore.Mvc;

namespace HaloStats.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IGetPlayerService getPlayerService;
        private readonly IGetPlayerGameHistory getPlayerGameHistory;

        public PlayerController(IGetPlayerService getPlayerService, IGetPlayerGameHistory getPlayerGameHistory)
        {
            this.getPlayerService = getPlayerService;
            this.getPlayerGameHistory = getPlayerGameHistory;
        }


        [HttpGet("{gamertag}")]
        public async Task<IActionResult> GetPlayer(string gamertag, [FromQuery] string? gamesPlayedWith)
        {
            var playerSummary = await getPlayerService.GetPlayer(gamertag, gamesPlayedWith);
            if (playerSummary == null)
            {
                return NotFound();
            }
            return Ok(playerSummary);
        }

        [HttpGet("{gamertag}/games")]
        public async Task<IActionResult> GamesPlayed(string gamertag, [FromQuery] GetPlayerGamesRequest request)
        {
            var gamesPlayed = await getPlayerGameHistory.GetGamesPlayed(gamertag, request);
            return Ok(gamesPlayed);
        }

        [HttpGet("{gamertag}/games/played-with-gamertags")]
        public async Task<IActionResult> GamertagsPlayedWith(string gamertag)
        {
            var gamertags = await getPlayerGameHistory.GetGamertagsThatPlayedWith(gamertag);
            return Ok(gamertags);
        }
    }
}
