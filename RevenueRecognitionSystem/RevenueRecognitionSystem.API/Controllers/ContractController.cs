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
[Route("/contracts")]
public class ContractController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractController(IContractService contractService)
    {
        _contractService = contractService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetAllContractsResponse>> GetClientsAsync()
    {
        try {
            var contracts = await _contractService.GetAllContractsAsync();
            return Ok(contracts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUpfrontContract(CreateUpfrontContractRequest request)
    {
        try
        {
            await _contractService.CreateUpfrontContractAsync(request);
            return Ok();
        }
        catch (Exception ex) when (ex is ClientNotFoundException or SoftwareSystemNotFoundException)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex) when (ex is ClientAlreadyHasContractException or InvalidDurationException or InvalidSupportYearsNumberException)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{contractId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContract(int contractId)
    {
        try
        {
            await _contractService.DeleteUpfrontContractAsync(contractId);
            return NoContent();
        }
        catch (ContractNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("{contractId:int}/payment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> IssueNewPayment(int contractId, [FromBody] decimal amount)
    {
        try
        {
            await _contractService.IssuePaymentAsync(contractId, amount);
            return Ok();
        }
        catch (ContractNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex) when (ex is ContractIsCancelledException or ContractIsFullyPaidException or ContractHasExpiredException or 
                                       InvalidPaymentAmountException or TotalPaymentExceedException)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}