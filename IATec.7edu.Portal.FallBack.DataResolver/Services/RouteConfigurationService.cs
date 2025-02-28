using IATec._7edu.Portal.FallBack.DataResolver.Interfaces;

namespace IATec._7edu.Portal.FallBack.DataResolver.Services;

public class RouteConfigurationService : IRouteConfigurationService
{
    private readonly Dictionary<Type, string> _routeMappings = new Dictionary<Type, string>
    {
        #region SevenEdu
            { typeof(IATec.Portal.Contracts.AcademicTermContract), "api/v1/academicTerm/get-academic-term-contract" },
            { typeof(IATec.Portal.Contracts.OccurrenceContract), "api/v1/occurrence/get-occurrence-contract" }
        #endregion
    };

    public string GetRouteForType(Type contractType)
    {
        if (_routeMappings.TryGetValue(contractType, out var route))
        {
            return route;
        }
        throw new InvalidOperationException($"Rota não encontrada para o tipo {contractType.Name}");
    }
}