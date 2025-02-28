using IATec.Portal.Contracts.Base;
using IATec.Portal.Contracts.Enum;

namespace IATec._7edu.Portal.FallBack.DataResolver.Interfaces;

public interface IFallbackHandler
{
    Task<T> HandleAsync<T>(Guid missingDataId, string route) where T : ContractBase;
    bool CanHandle(SourceEnum source);
}