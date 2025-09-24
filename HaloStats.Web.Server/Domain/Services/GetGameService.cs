using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Game;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGetGameService
{
    Task<GetGameResponse?> GetGame(Guid gameId);
}

public class GetGameService : IGetGameService
{
    private readonly IGameRepository gameRepo;

    public GetGameService(IGameRepository gameRepo)
    {
        this.gameRepo = gameRepo;
    }

    public async Task<GetGameResponse?> GetGame(Guid gameId)
    {
        var game = await gameRepo.GetGameById(gameId);
        if (game == null)
        {
            return null;
        }
        var response = GameMapper.MapToGetGameResponse(game);
        return response;
    }
}
