using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Exceptions.Contract;
using RevenueRecognitionSystem.Domain.Exceptions.Payment;
using RevenueRecognitionSystem.Domain.Exceptions.SoftwareSystem;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;

namespace RevenueRecognitionSystem.API.Controllers;

[ApiController]
[Route("api/contracts")]
[Authorize]
public class ContractController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly ILogger<ContractController> _logger;

    public ContractController(IContractService contractService, ILogger<ContractController> logger)
    {
        _contractService = contractService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetAllContractsResponse>> GetContractsAsync()
    {
        _logger.LogDebug("Fetching contracts");
        var contracts = await _contractService.GetAllContractsAsync();
        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUpfrontContract(CreateUpfrontContractRequest request)
    {
        _logger.LogInformation("Adding new contract for the client with id {ClientId}", request.ClientId);
        await _contractService.CreateUpfrontContractAsync(request);
        return Ok();
    }

    [HttpDelete("{contractId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContract(int contractId)
    {
        _logger.LogInformation("Deleting contract with id {ContractId}", contractId);
        await _contractService.DeleteUpfrontContractAsync(contractId);
        return NoContent();
    }
    
    [HttpPost("{contractId:int}/payment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> IssueNewPayment(int contractId, [FromBody] decimal amount)
    {
        _logger.LogInformation("Issuing paying for the contract with id {ContractId}", contractId);
        await _contractService.IssuePaymentAsync(contractId, amount);
        return Ok();
    }
}