namespace IATec._7edu.Portal.FallBack.DataResolver.Interfaces;

public interface IRouteConfigurationService
{
    string GetRouteForType(Type contractType);
}