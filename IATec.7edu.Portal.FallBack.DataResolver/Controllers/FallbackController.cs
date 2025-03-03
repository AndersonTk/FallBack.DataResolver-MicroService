using IATec._7edu.Portal.FallBack.DataResolver.Models;
using IATec._7edu.Portal.FallBack.DataResolver.Services;
using Microsoft.AspNetCore.Mvc;

namespace IATec._7edu.Portal.FallBack.DataResolver.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FallbackController : ControllerBase
{
    private readonly FallbackService _fallbackService;
    private readonly ILogger<FallbackController> _logger;

    public FallbackController(FallbackService fallbackService, ILogger<FallbackController> logger)
    {
        _fallbackService = fallbackService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> GetMissingData([FromBody] FallbackRequest request)
    {
        var data = await _fallbackService.HandleAsync(request);

        if (data.Data is null)
            return StatusCode((int)data.StatusCode);

        return Ok(data.Data);
    }
}
