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

    public RevenueController(IRevenueCalculationService revenueCalculationService)
    {
        _revenueCalculationService = revenueCalculationService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RevenueResponse>> GetRevenueAsync([FromQuery] RevenueRequest request)
    {
        try {
            var response = await _revenueCalculationService.CalculateRevenueAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}