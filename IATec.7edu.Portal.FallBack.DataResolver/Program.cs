using IATec._7edu.Portal.FallBack.DataResolver.Configuration;
using IATec._7edu.Portal.FallBack.DataResolver.Handles;
using IATec._7edu.Portal.FallBack.DataResolver.Interfaces;
using IATec._7edu.Portal.FallBack.DataResolver.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomHttpClients(builder.Configuration);

// Adicionar Handlers e FallbackService
builder.Services.AddScoped<IFallbackHandler, LegacyApiHandler>();
builder.Services.AddScoped<IFallbackHandler, EventApiHandler>();
builder.Services.AddScoped<FallbackService>();

// Registrar RouteConfigurationService
builder.Services.AddSingleton<IRouteConfigurationService, RouteConfigurationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
