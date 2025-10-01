using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Player;
using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGamertagService
{
    Task<List<GamertagInfo>> GetGamertags();
    Task<List<string>> SearchGamertags(SearchGamertagsRequest request);
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

    public async Task<List<string>> SearchGamertags(SearchGamertagsRequest request)
    {
        var gamertags = await gamerTagRepository.SearchGamertags(request);
        return gamertags;
    }
}
