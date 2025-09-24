using HaloStats.Web.Server.Domain.Mappers;
using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Dashboard;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGetDashboardService
{
    Task<DashboardResponse> GetDashboard();
}

public class GetDashboardService : IGetDashboardService
{
    private readonly IGameRepository gameRepo;

    public GetDashboardService(IGameRepository gameRepo)
    {
        this.gameRepo = gameRepo;
    }

    public async Task<DashboardResponse> GetDashboard()
    {
        var recentGames = await gameRepo.GetRecentGames(50);
        var totalAmountOfGames = await gameRepo.TotalAmountOfGames();
        var response = DashboardMapper.MapToDashboardResponse(recentGames, totalAmountOfGames);
        return response;
    }
}
