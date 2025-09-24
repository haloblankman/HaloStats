using HaloStats.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaloStats.Web.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGetGameService getGameService;

    public GameController(IGetGameService getGameService)
    {
        this.getGameService = getGameService;
    }

    [HttpGet]
    [Route("{gameId}")]
    public async Task<IActionResult> GetGame(Guid gameId)
    {
        var response = await getGameService.GetGame(gameId);
        if (response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }
}
