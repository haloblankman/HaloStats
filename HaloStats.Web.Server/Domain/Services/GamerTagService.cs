using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGamerTagService
{
    Task<List<GamerTagInfo>> GetGamerTags();
}

public class GamerTagService : IGamerTagService
{
    private readonly IGamerTagRepository gamerTagRepository;

    public GamerTagService(IGamerTagRepository gamerTagRepository)
    {
        this.gamerTagRepository = gamerTagRepository;
    }

    public async Task<List<GamerTagInfo>> GetGamerTags()
    {
        var gamerTags = await gamerTagRepository.GetAllGamerTags();
        return gamerTags.Select(gt => new GamerTagInfo
        {
            GamerTagId = gt.GamerTagId,
            Name = gt.Name,
            XboxUserId = gt.XboxUserId
        }).OrderBy(p => p.Name).ToList();
    }
}
