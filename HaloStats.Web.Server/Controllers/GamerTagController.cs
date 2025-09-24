using HaloStats.Web.Server.Domain.Services;
using HaloStats.Web.Shared.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HaloStats.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamerTagController : ControllerBase
{
    private readonly IGamerTagService gamerTagService;

    public GamerTagController(IGamerTagService gamerTagService)
    {
        this.gamerTagService = gamerTagService;
    }

    [HttpGet]
    public async Task<List<GamerTagInfo>> GetGamerTags()
    {
        var gamerTags = await gamerTagService.GetGamerTags();
        return gamerTags;
    }
}
