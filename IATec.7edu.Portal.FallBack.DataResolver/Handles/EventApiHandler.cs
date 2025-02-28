using IATec._7edu.Portal.FallBack.DataResolver.Interfaces;
using IATec.Portal.Contracts.Base;
using IATec.Portal.Contracts.Enum;
using Newtonsoft.Json;
using System.Net;

namespace IATec._7edu.Portal.FallBack.DataResolver.Handles;

public class EventApiHandler : IFallbackHandler
{
    private readonly IHttpClientFactory _clientFactory;

    public EventApiHandler(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public bool CanHandle(SourceEnum source)
    {
        return source == SourceEnum.SevenEdu;
    }

    public async Task<T> HandleAsync<T>(Guid missingDataId, string route) where T : ContractBase
    {
        var client = _clientFactory.CreateClient("EventApi");
        var response = await client.GetAsync($"{route}/{missingDataId}");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(apiResponse);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.InternalServerError:
                return null;
            case HttpStatusCode.BadGateway:
                return null;
            case HttpStatusCode.NoContent:
                return null;
            case HttpStatusCode.BadRequest:
                return null;
            case HttpStatusCode.Forbidden:
                return null;
        }

        return null;
    }
}