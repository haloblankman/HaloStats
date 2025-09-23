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
        return new DashboardResponse();
    }
}
