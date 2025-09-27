using HaloStats.Web.Server.Domain.Services;
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
}
