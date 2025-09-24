using HaloStats.Database;
using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.CarnageReport;

namespace HaloStats.Web.Server.Domain.Services;

public interface IPostCarnageReportService
{
    Task<PostCarnageReportResponse> SavePostCarnageReport(PostCarnageReportRequest request, string ipAddress);
}

public class PostCarnageReportService : IPostCarnageReportService
{
    private readonly IGameRepository gameRepository;
    private readonly IGamerTagRepository gamerTagRepository;
    private readonly HaloStatsDbContext db;

    public PostCarnageReportService(
        IGameRepository gameRepository, 
        IGamerTagRepository gamerTagRepository,
        HaloStatsDbContext db)
    {
        this.gameRepository = gameRepository;
        this.gamerTagRepository = gamerTagRepository;
        this.db = db;
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

        game.CalcualtePlayerStandings();
        db.Games.Add(game);

        var gamerTags = game.Players.Select(p => p.GamerTag).Distinct().ToList();
        var nonExistingGamerTags = await gamerTagRepository.GetNonExistingGamerTags(gamerTags);
        db.GamerTags.AddRange(nonExistingGamerTags.Select(ngt => new Database.Entities.GamerTag
        {
            Name = ngt,
            XboxUserId = game.Players.First(p => p.GamerTag == ngt).XboxUserId
        }));

        await db.SaveChangesAsync();
        

        return new PostCarnageReportResponse
        {
            IsDuplicate = isDuplicate,
            GameId = game.GameId
        };
    }
}
