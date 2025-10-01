using HaloStats.Database;
using HaloStats.Database.Entities;
using HaloStats.Database.Enums;
using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Shared.Contracts.Player;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGamertagRepository
{
    Task<List<Gamertag>> GetAllGamertags();
    Task<List<string>> GetNonExistingGamertags(List<string> gamerTags);
    Task<List<string>> SearchGamertags(SearchGamertagsRequest request);
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

    public async Task<List<string>> SearchGamertags(SearchGamertagsRequest request)
    {
        GameEnum? game = request.Game == null ? null : request.Game.Value.MapToGameEnum();

        var gamertags = from gp in db.GamePlayers
                        join g in db.Games.Where(g => !g.IsDuplicateGame && !g.IsDeleted) on gp.GameId equals g.GameId
                        join gp2 in db.GamePlayers on g.GameId equals gp2.GameId
                        where string.IsNullOrEmpty(request.PlayedWithGamertag) || gp.Gamertag == request.PlayedWithGamertag
                        where string.IsNullOrEmpty(request.PlayedWithGamertag) || gp2.Gamertag != request.PlayedWithGamertag
                        where game == null || g.GameEnum == game
                        select gp2.Gamertag;

        return await gamertags.Distinct().Order().ToListAsync();
    }
}
