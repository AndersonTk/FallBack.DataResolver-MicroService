using IATec._7edu.Portal.FallBack.DataResolver.Interfaces;
using IATec.Portal.Contracts.Base;
using IATec.Portal.Contracts.Enum;
using Newtonsoft.Json;

namespace IATec._7edu.Portal.FallBack.DataResolver.Handles;

public class LegacyApiHandler : IFallbackHandler
{
    private readonly IHttpClientFactory _clientFactory;

    public LegacyApiHandler(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public bool CanHandle(SourceEnum source)
    {
        return source == SourceEnum.WebEscola;
    }

    public async Task<T> HandleAsync<T>(Guid missingDataId, string route) where T : ContractBase
    {
        var client = _clientFactory.CreateClient("LegacyApi");
        var response = await client.GetAsync($"{route}/{missingDataId}");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(apiResponse);
        }

        return null;
    }
}
