using HaloStats.Database;
using HaloStats.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaloStats.Web.Server.Domain.Repositories;

public interface IGameRepository
{
    Task<List<Game>> GetGameByGameUniqueId(Guid gameUniqueId);
    Task<Game?> GetGameById(Guid gameId);
    Task<List<Game>> GetRecentGames(int count);
    Task<int> TotalAmountOfGames();
}

public class GameRepository : IGameRepository
{
    private readonly HaloStatsDbContext db;

    public GameRepository(HaloStatsDbContext db)
    {
        this.db = db;
    }

    public async Task<List<Game>> GetGameByGameUniqueId(Guid gameUniqueId)
    {
        return await db.Games
            .Where(g => g.GameUniqueId == gameUniqueId)
            .Include(g => g.Players)
            .ToListAsync();
    }

    public async Task<Game?> GetGameById(Guid gameId)
    {
        return await db.Games
            .Where(g => g.GameId == gameId)
            .Include(g => g.Players)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Game>> GetRecentGames(int count) 
    {         
        return await db.Games
            .Where(g => !g.IsDuplicateGame && !g.IsDeleted)
            .OrderByDescending(g => g.ReportedAt)
            .Take(count)
            .Include(g => g.Players)
            .ToListAsync();
    }

    public async Task<int> TotalAmountOfGames()
    {
        return await db.Games.Where(g => !g.IsDuplicateGame && !g.IsDeleted).CountAsync();
    }
}
