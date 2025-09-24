using HaloStats.Database;
using HaloStats.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGamerTagRepository
{
    Task<List<GamerTag>> GetAllGamerTags();
    Task<List<string>> GetNonExistingGamerTags(List<string> gamerTags);
}

public class GamerTagRepository : IGamerTagRepository
{
    private readonly HaloStatsDbContext db;
    public GamerTagRepository(HaloStatsDbContext db)
    {
        this.db = db;
    }

    public Task<List<GamerTag>> GetAllGamerTags()
    {
        return db.GamerTags.ToListAsync();
    }

    public async Task<List<string>> GetNonExistingGamerTags(List<string> gamerTags)
    {
        var existing = await db.GamerTags
            .Where(gt => gamerTags.Contains(gt.Name))
            .Select(gt => gt.Name)
            .ToListAsync();

        return gamerTags.Except(existing).ToList();
    }
}
