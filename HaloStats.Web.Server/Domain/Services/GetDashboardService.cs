using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Shared.Contracts.Dashboard;

namespace HaloStats.Web.Server.Domain.Services;

public interface IGetDashboardService
{
    Task<DashboardResponse> GetDashboard();
}

public class GetDashboardService : IGetDashboardService
{
    private readonly IGameAnalyticsRepository gameAnalyticsRepo;

    public GetDashboardService(IGameAnalyticsRepository gameAnalyticsRepo)
    {
        this.gameAnalyticsRepo = gameAnalyticsRepo;
    }

    public async Task<DashboardResponse> GetDashboard()
    {
        var topPlayersByKillCount = await gameAnalyticsRepo.GetTopPlayersByKillCount();
        var topPlayersByKillCountThisMonth = await gameAnalyticsRepo.GetTopPlayersByKillCountThisMonth();
        var topTeammatePairs = await gameAnalyticsRepo.GetTopTeammatePairs();
        var topTeammatePairsByWins = await gameAnalyticsRepo.GetTopTeammatePairsByWins();
        return new DashboardResponse
        {
            TopPlayersByKillCount = topPlayersByKillCount,
            TopPlayersByKillCountThisMonth = topPlayersByKillCountThisMonth,
            TopTeammatePairs = topTeammatePairs,
            TopTeammatePairsByWins = topTeammatePairsByWins
        };
    }
}
