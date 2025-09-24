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

    public PostCarnageReportService(IGameRepository gameRepository)
    {
        this.gameRepository = gameRepository;
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
        await gameRepository.AddGame(game);

        return new PostCarnageReportResponse
        {
            IsDuplicate = isDuplicate,
            GameId = game.GameId
        };
    }
}
