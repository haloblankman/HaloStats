using HaloStats.Web.Server.Domain.Services;
using HaloStats.Web.Shared.Contracts.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace HaloStats.Web.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IGetDashboardService getDashboardService;

    public DashboardController(IGetDashboardService getDashboardService)
    {
        this.getDashboardService = getDashboardService;
    }

    [HttpGet()]
    public async Task<DashboardResponse> GetDashboard()
    {
        return await getDashboardService.GetDashboard();
    }
}
