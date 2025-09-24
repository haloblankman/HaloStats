using HaloStats.Web.Server.Domain.Services;
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
        public async Task<IActionResult> GetPlayer(string gamertag)
        {
            var playerSummary = await getPlayerService.GetPlayer(gamertag);
            if (playerSummary == null)
            {
                return NotFound();
            }
            return Ok(playerSummary);
        }

        [HttpGet("{gamertag}/games")]
        public async Task<IActionResult> GamesPlayed(string gamertag, int pageNumber = 1, int pageSize = 100)
        {
            var gamesPlayed = await getPlayerGameHistory.GetGamesPlayed(gamertag, pageNumber, pageSize);
            return Ok(gamesPlayed);
        }

        //[HttpGet("search")]
        //public async Task<IActionResult> SearchPlayers(string query)
        //{

        //}
    }
}
