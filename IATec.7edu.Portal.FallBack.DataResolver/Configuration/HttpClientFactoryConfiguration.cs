using Polly;

namespace IATec._7edu.Portal.FallBack.DataResolver.Configuration;

public static class HttpClientFactoryConfiguration
{
    public static void AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("LegacyApi", client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalApis:LegacyApi"]);
        })
        .AddTransientHttpErrorPolicy(policyBuilder =>
            policyBuilder.WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            ))
        .AddTransientHttpErrorPolicy(policyBuilder =>
            policyBuilder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)));

        services.AddHttpClient("EventApi", client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalApis:EventApi"]);
        });
    }
}
