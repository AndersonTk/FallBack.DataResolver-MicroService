using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;

namespace IATec._7edu.Portal.FallBack.DataResolver.Configuration;

public static class AuthoriazationConfiguration
{
    public static void AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        #region JWT TOKEN
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://seu-dominio-de-autenticacao.com"; // Ex: IdentityServer4, Auth0, Azure AD
                    options.Audience = "fallback-api"; // Identificador do recurso protegido
                    options.RequireHttpsMetadata = false; // Defina como true em produção

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero // Evita atrasos na expiração do token
                    };
                });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("FallbackRead", policy => policy.RequireClaim("scope", "fallback.read"));
            options.AddPolicy("FallbackWrite", policy => policy.RequireClaim("scope", "fallback.write"));
        });
        #endregion

        #region CORS
        services.AddCors(options =>
        {
            options.AddPolicy("FallbackCorsPolicy", builder =>
            {
                builder.WithOrigins("https://localhost:7147")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });
        #endregion

        #region DDDoS Protect
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.Connection.RemoteIpAddress.ToString(),
                    a => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100, // 100 requisições por minuto
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });
        #endregion
    }
}
