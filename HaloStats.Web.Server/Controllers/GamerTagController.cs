using HaloStats.Web.Server.Domain.Services;
using HaloStats.Web.Shared.Contracts.Player;
using HaloStats.Web.Shared.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HaloStats.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamertagController : ControllerBase
{
    private readonly IGamertagService gamertagService;

    public GamertagController(IGamertagService gamertagService)
    {
        this.gamertagService = gamertagService;
    }

    [HttpGet]
    public async Task<List<GamertagInfo>> GetGamertags()
    {
        var gamerTags = await gamertagService.GetGamertags();
        return gamerTags;
    }

    [HttpGet]
    [Route("search")]
    public async Task<List<string>> SearchGamertags([FromQuery] SearchGamertagsRequest request)
    {
        var gamertags = await gamertagService.SearchGamertags(request);
        return gamertags;
    }
}
