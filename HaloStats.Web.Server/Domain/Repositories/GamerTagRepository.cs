using HaloStats.Database;
using HaloStats.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGamertagRepository
{
    Task<List<Gamertag>> GetAllGamertags();
    Task<List<string>> GetNonExistingGamertags(List<string> gamerTags);
}

public class GamertagRepository : IGamertagRepository
{
    private readonly HaloStatsDbContext db;
    public GamertagRepository(HaloStatsDbContext db)
    {
        this.db = db;
    }

    public Task<List<Gamertag>> GetAllGamertags()
    {
        return db.Gamertags.ToListAsync();
    }

    public async Task<List<string>> GetNonExistingGamertags(List<string> gamertags)
    {
        var existing = await db.Gamertags
            .Where(gt => gamertags.Contains(gt.Name))
            .Select(gt => gt.Name)
            .ToListAsync();

        return gamertags.Except(existing).ToList();
    }
}
