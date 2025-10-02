using HaloStats.Database;
using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.CarnageReport;

namespace HaloStats.Web.Server.Domain.Services;

public interface IPostCarnageReportService
{
    Task<PostCarnageReportResponse> SavePostCarnageReport(PostCarnageReportRequest request, string ipAddress);
    Task DeleteCarnageReport(Guid gameUniqueId, string ipAddress);
}

public class PostCarnageReportService : IPostCarnageReportService
{
    private readonly IGameRepository gameRepository;
    private readonly IGamertagRepository gamerTagRepository;
    private readonly HaloStatsDbContext db;

    public PostCarnageReportService(
        IGameRepository gameRepository, 
        IGamertagRepository gamerTagRepository,
        HaloStatsDbContext db)
    {
        this.gameRepository = gameRepository;
        this.gamerTagRepository = gamerTagRepository;
        this.db = db;
    }

    public async Task DeleteCarnageReport(Guid gameUniqueId, string ipAddress)
    {
        var games = await gameRepository.GetGameByGameUniqueId(gameUniqueId);
        if (games.Count == 0)
        {
            return;
        }
        foreach (var game in games)
        {
            game.Delete(ipAddress);
        }
        await db.SaveChangesAsync();
    }

    public async Task<PostCarnageReportResponse> SavePostCarnageReport(PostCarnageReportRequest request, string ipAddress)
    {
        var game = PostCarnageReportRequestMapper.MapToGame(request, ipAddress);
        var dupGames = await gameRepository.GetGameByGameUniqueId(request.GameUniqueId);
        bool isDuplicate = false;
        if (dupGames.Count() > 0)
        {
            if (game.IsGameDuplicate(dupGames))
            {
                return new PostCarnageReportResponse
                {
                    IsDuplicate = true,
                    GameId = dupGames.First().GameId
                };
            }

            if (game.IsLatestGame(dupGames))
            {
                dupGames.ForEach(dg => dg.Duplicate());
            }
            else
            {
                game.Duplicate();
                isDuplicate = true;
            }
        }

        game.SetCalculatedFields();
        db.Games.Add(game);

        var gamerTags = game.Players.Select(p => p.Gamertag).Distinct().ToList();
        var nonExistingGamertags = await gamerTagRepository.GetNonExistingGamertags(gamerTags);
        db.Gamertags.AddRange(nonExistingGamertags.Select(ngt => new Database.Entities.Gamertag
        {
            Name = ngt,
            XboxUserId = game.Players.First(p => p.Gamertag == ngt).XboxUserId
        }));

        await db.SaveChangesAsync();
        

        return new PostCarnageReportResponse
        {
            IsDuplicate = isDuplicate,
            GameId = game.GameId
        };
    }
}
