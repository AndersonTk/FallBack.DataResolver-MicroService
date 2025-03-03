using IATec._7edu.Portal.FallBack.DataResolver.Configuration;
using IATec._7edu.Portal.FallBack.DataResolver.Middlewares;
using Serilog.Sinks.LogBee.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomHttpClients(builder.Configuration);
builder.Services.AddCustomAuthorization(builder.Configuration);
builder.Services.AddDependencyInjection(builder.Configuration);
builder.Services.AddLoggerConfig(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseLogBeeMiddleware();
app.UseMiddleware<ApiKeyMiddleware>();


app.MapControllers();

app.Run();
