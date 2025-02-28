using IATec.Portal.Contracts.Base;

namespace IATec._7edu.Portal.FallBack.DataResolver.Models;

public class DataModel<T> where T : ContractBase
{
    public Guid Id { get; set; }
    public T Data { get; set; }
}