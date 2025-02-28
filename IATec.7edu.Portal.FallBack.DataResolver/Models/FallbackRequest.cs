using IATec.Portal.Contracts.Enum;

namespace IATec._7edu.Portal.FallBack.DataResolver.Models;

public class FallbackRequest
{
    public Guid MissingDataId { get; set; }
    public SourceEnum SourceEnum { get; set; }
    public string TargetType { get; set; }
}