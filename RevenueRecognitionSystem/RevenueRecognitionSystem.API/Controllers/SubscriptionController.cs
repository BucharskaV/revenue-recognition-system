using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Exceptions.Contract;
using RevenueRecognitionSystem.Domain.Exceptions.SoftwareSystem;
using RevenueRecognitionSystem.Domain.Exceptions.Subscription;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;

namespace RevenueRecognitionSystem.API.Controllers;

[ApiController]
[Route("api/subscriptions")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(ISubscriptionService subscriptionService, ILogger<SubscriptionController> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetAllSubscriptionsResponse>> GetSubscriptionsAsync()
    {
        _logger.LogDebug("Fetch subscriptions");
        var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
        return Ok(subscriptions);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUpfrontContract(CreateSubscriptionRequest request)
    {
        _logger.LogInformation("Creating new upfront contract for client with id {ClientId}", request.ClientId);
        await _subscriptionService.CreateSubscriptionAsync(request);
        return Ok();
    }
    
    [HttpPost("{subscriptionId:int}/payment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> IssueNewPayment(int subscriptionId, [FromBody] decimal amount)
    {
        _logger.LogInformation("Adding new payment for subscription with id {SubscriptionId}", subscriptionId);
        await _subscriptionService.PayForRenewalAsync(subscriptionId, amount);
        return Ok();
    }
}