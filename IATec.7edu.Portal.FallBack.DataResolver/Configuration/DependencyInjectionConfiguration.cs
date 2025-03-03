using IATec._7edu.Portal.FallBack.DataResolver.Handles;
using IATec._7edu.Portal.FallBack.DataResolver.Interfaces;
using IATec._7edu.Portal.FallBack.DataResolver.Services;

namespace IATec._7edu.Portal.FallBack.DataResolver.Configuration;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        // Adicionar Handlers e FallbackService
        services.AddScoped<IFallbackHandler, LegacyApiHandler>();
        services.AddScoped<IFallbackHandler, EventApiHandler>();
        services.AddScoped<FallbackService>();

        // Registrar RouteConfigurationService
        services.AddSingleton<IRouteConfigurationService, RouteConfigurationService>();
    }
}
