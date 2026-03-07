using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Domain.Constants;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;

namespace RevenueRecognitionSystem.API.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
    {
        _clientService = clientService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetClientResponse>> GetClientsAsync()
    {
        _logger.LogDebug("Fetching clients");
        var clients = await _clientService.GetAllClientsAsync();
        return Ok(clients);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetClientResponse>> GetClientByIdAsync(int id)
    {
        _logger.LogInformation("Fetching client with id {ClientId}", id);
        var client = await _clientService.GetClientAsync(id);
        return Ok(client);
    }

    [HttpPost("individual")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddIndividualAsync(AddIndividualRequest request)
    {
        _logger.LogInformation("Adding new individual client with PESEL {Pesel}", request.PESEL);
        await _clientService.AddIndividualAsync(request);
        return Created();
    }
    
    [HttpPost("company")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCompanyAsync(AddCompanyRequest request)
    {
        _logger.LogInformation("Adding new company with KRS {KRS}", request.KRSNumber);
        await _clientService.AddCompanyAsync(request);
        return Created();
    }
    
    [Authorize(Roles = Roles.Admin)]
    [HttpPut("individual/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateIndividualAsync(int id, UpdateIndividualRequest request)
    {
        _logger.LogInformation("Updating individual client with id {id}", id);
        await _clientService.UpdateIndividualAsync(id, request);
        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("company/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCompanyAsync(int id, UpdateCompanyRequest request)
    {
        _logger.LogInformation("Updating company with id {id}", id);
        await _clientService.UpdateCompanyAsync(id, request);
        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClientAsync(int id)
    {
        _logger.LogInformation("Deleting client with id {id}", id);
        await _clientService.DeleteClientAsync(id);
        return NoContent();
    }
}