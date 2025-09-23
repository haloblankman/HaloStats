using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Server.Domain.Services;

namespace HaloStats.Web.Server.Infrastructure;

public static class CompositionRoot
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        services.AddScoped<IGameAnalyticsRepository, GameAnalyticsRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IGetDashboardService, GetDashboardService>();
        services.AddScoped<IPostCarnageReportService, PostCarnageReportService>();
        services.AddScoped<IGetPlayerService, GetPlayerService>();
        services.AddScoped<IGetPlayerGameHistory, GetPlayerGameHistory>();

        return services;
    }
}
