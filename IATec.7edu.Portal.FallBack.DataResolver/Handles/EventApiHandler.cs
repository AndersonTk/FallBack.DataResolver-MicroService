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

    public async Task<(object, int)> HandleAsync<T>(Guid missingDataId, string route) where T : ContractBase
    {
        var client = _clientFactory.CreateClient("EventApi");
        var response = await client.GetAsync($"{route}/{missingDataId}");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return (JsonConvert.DeserializeObject<T>(apiResponse), StatusCodes.Status200OK);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.InternalServerError:
                return (null, (int)HttpStatusCode.OK);
            case HttpStatusCode.BadGateway:
                return (null, (int)HttpStatusCode.BadGateway);
            case HttpStatusCode.NoContent:
                return (null, (int)HttpStatusCode.NoContent);
            case HttpStatusCode.BadRequest:
                return (null, (int)HttpStatusCode.BadRequest);
            case HttpStatusCode.Forbidden:
                return (null, (int)HttpStatusCode.Forbidden);
            case HttpStatusCode.NotFound:
                return (null, (int)HttpStatusCode.NotFound);
        }

        return (null, (int)HttpStatusCode.InternalServerError);
    }
}