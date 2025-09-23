using HaloStats.Database;
using HaloStats.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGameRepository
{
    Task<Game?> GetGameByGameUniqueId(Guid gameUniqueId);
    Task AddGame(Game game);
}

public class GameRepository : IGameRepository
{
    private readonly HaloStatsDbContext db;

    public GameRepository(HaloStatsDbContext db)
    {
        this.db = db;
    }

    public async Task<Game?> GetGameByGameUniqueId(Guid gameUniqueId)
    {
        return await db.Games
            .Where(g => g.GameUniqueId == gameUniqueId)
            .Include(g => g.Players)
            .FirstOrDefaultAsync();
    }

    public async Task AddGame(Game game)
    {
        db.Games.Add(game);
        await db.SaveChangesAsync();
    }
}
