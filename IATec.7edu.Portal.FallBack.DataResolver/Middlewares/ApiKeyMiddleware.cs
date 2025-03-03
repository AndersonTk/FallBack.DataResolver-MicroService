namespace IATec._7edu.Portal.FallBack.DataResolver.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyMiddleware> _logger;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var apiClients = _configuration.GetSection("ApiClients").Get<Dictionary<string, ApiClient>>();

        if (apiClients == null || !apiClients.Any())
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var message = "Configuração de clientes não encontrada.";
            _logger.LogError(message);

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                statusCode = StatusCodes.Status500InternalServerError,
                message = message
            });
            return;
        }

        var providedApiKey = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(providedApiKey) || !providedApiKey.StartsWith("Bearer "))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var message = "Token ausente ou inválido.";
            _logger.LogWarning(message);

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                statusCode = StatusCodes.Status401Unauthorized,
                message = message
            });
            return;
        }

        providedApiKey = providedApiKey.Replace("Bearer ", "").Trim();

        var client = apiClients.Values.FirstOrDefault(c => c.ApiKey == providedApiKey);
        if (client == null)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var message = "Acesso não autorizado. Token inválido.";
            _logger.LogWarning(message);

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                statusCode = StatusCodes.Status403Forbidden,
                message = message
            });
            return;
        }

        // Obtém a URL do endpoint requisitado
        var requestedEndpoint = context.Request.Path.ToString().ToLower();

        // Verifica se o cliente tem permissão para acessar a URL
        if (!client.AllowedEndpoints.Any(ep => requestedEndpoint.StartsWith(ep.ToLower())))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var message = $"Acesso negado ao endpoint '{requestedEndpoint}'.";
            _logger.LogWarning(message);

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                statusCode = StatusCodes.Status403Forbidden,
                message = message
            });
            return;
        }

        await _next(context);
    }
}

public class ApiClient
{
    public string ApiKey { get; set; }
    public List<string> AllowedEndpoints { get; set; }
}
