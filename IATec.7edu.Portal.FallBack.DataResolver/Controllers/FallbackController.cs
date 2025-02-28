using IATec._7edu.Portal.FallBack.DataResolver.Models;
using IATec._7edu.Portal.FallBack.DataResolver.Services;
using Microsoft.AspNetCore.Mvc;

namespace IATec._7edu.Portal.FallBack.DataResolver.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FallbackController : ControllerBase
{
    private readonly FallbackService _fallbackService;

    public FallbackController(FallbackService fallbackService)
    {
        _fallbackService = fallbackService;
    }

    [HttpPost]
    public async Task<IActionResult> GetMissingData([FromBody] FallbackRequest request)
    {
        var data = await _fallbackService.HandleAsync(request);
        if (data == null)
            return NotFound();
        return Ok(data);
    }
}
