using IATec._7edu.Portal.FallBack.DataResolver.Interfaces;
using IATec._7edu.Portal.FallBack.DataResolver.Models;

namespace IATec._7edu.Portal.FallBack.DataResolver.Services;

public class FallbackService
{
    private readonly IEnumerable<IFallbackHandler> _handlers;
    private readonly IRouteConfigurationService _routes;

    public FallbackService(IEnumerable<IFallbackHandler> handlers,
                           IRouteConfigurationService routes)
    {
        _handlers = handlers;
        _routes = routes;
    }

    public async Task<object> HandleAsync(FallbackRequest request)
    {
        var handler = _handlers.FirstOrDefault(h => h.CanHandle(request.SourceEnum));
        if (handler == null)
            throw new InvalidOperationException("Handler não encontrado para a fonte especificada.");

        // Monta o nome completo do assembly
        string fullTypeName = $"{request.TargetType}, IATec.Portal.Contracts";

        // Tenta carregar o tipo pelo nome completo do assembly
        var targetType = Type.GetType(fullTypeName);
        if (targetType == null)
        {
            throw new InvalidOperationException($"Tipo '{fullTypeName}' não foi encontrado.");
        }

        var route = _routes.GetRouteForType(targetType);

        // Usa reflexão para chamar o método genérico
        var method = handler.GetType().GetMethod("HandleAsync").MakeGenericMethod(targetType);
        var task = (Task)method.Invoke(handler, new object[] { request.MissingDataId, route });

        await task.ConfigureAwait(false);

        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task);
    }
}

