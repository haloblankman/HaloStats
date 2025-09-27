using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGamertagService
{
    Task<List<GamertagInfo>> GetGamertags();
}

public class GamertagService : IGamertagService
{
    private readonly IGamertagRepository gamerTagRepository;

    public GamertagService(IGamertagRepository gamerTagRepository)
    {
        this.gamerTagRepository = gamerTagRepository;
    }

    public async Task<List<GamertagInfo>> GetGamertags()
    {
        var gamerTags = await gamerTagRepository.GetAllGamertags();
        return gamerTags.Select(gt => new GamertagInfo
        {
            GamertagId = gt.GamertagId,
            Name = gt.Name,
            XboxUserId = gt.XboxUserId
        }).OrderBy(p => p.Name).ToList();
    }
}
