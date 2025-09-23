using HaloStats.Web.Server.Domain.Repositories;
using HaloStats.Web.Server.Domain.Services;
using System.Reflection;

namespace HaloStats.Web.Server.Infrastructure;

public static class CompositionRoot
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        //services.AddScoped<IGameAnalyticsRepository, GameAnalyticsRepository>();
        //services.AddScoped<IGameRepository, GameRepository>();
        //services.AddScoped<IPlayerRepository, PlayerRepository>();

        //services.AddScoped<IGetDashboardService, GetDashboardService>();
        //services.AddScoped<IPostCarnageReportService, PostCarnageReportService>();
        //services.AddScoped<IGetPlayerService, GetPlayerService>();
        //services.AddScoped<IGetPlayerGameHistory, GetPlayerGameHistory>();

        var assembly = typeof(CompositionRoot).Assembly;

        // Register Repositories
        RegisterByConvention(
            services,
            assembly,
            "HaloStats.Web.Server.Domain.Repositories"
        );

        // Register Services
        RegisterByConvention(
            services,
            assembly,
            "HaloStats.Web.Server.Domain.Services"
        );

        return services;
    }

    private static void RegisterByConvention(
        IServiceCollection services,
        Assembly assembly,
        string @namespace
    )
    {
        var types = assembly.GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.Namespace == @namespace
            );

        foreach (var implementationType in types)
        {
            var interfaceType = implementationType.GetInterfaces().FirstOrDefault(i =>
                i.Namespace == @namespace
            );
            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}
