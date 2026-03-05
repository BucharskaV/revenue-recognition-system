using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;

namespace RevenueRecognitionSystem.API.Controllers;

[ApiController]
[Route("api/revenue")]
[Authorize]
public class RevenueController : ControllerBase
{
    private readonly IRevenueCalculationService _revenueCalculationService;
    private readonly ILogger<RevenueController> _logger;

    public RevenueController(IRevenueCalculationService revenueCalculationService, ILogger<RevenueController> logger)
    {
        _revenueCalculationService = revenueCalculationService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RevenueResponse>> GetRevenueAsync([FromQuery] RevenueRequest request)
    {
        _logger.LogInformation("Calculating revenue");
        var response = await _revenueCalculationService.CalculateRevenueAsync(request);
        return Ok(response);
    }
}