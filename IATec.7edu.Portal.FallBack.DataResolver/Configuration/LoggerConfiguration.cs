using Serilog;
using Serilog.Sinks.LogBee;

namespace IATec._7edu.Portal.FallBack.DataResolver.Configuration;

public static class LoggerConfiguration
{
    public static IServiceCollection AddLoggerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((services, lc) => lc
                     .WriteTo.LogBee(new LogBeeApiKey(
                             configuration["LogBee.OrganizationId"]!,
                             configuration["LogBee.ApplicationId"]!,
                             configuration["LogBee.ApiUrl"]!
         ),
         services
     ));
        return services;
    }
}