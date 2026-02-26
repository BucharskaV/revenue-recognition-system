using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Interfaces;
using RegisterRequest = RevenueRecognitionSystem.Services.Contracts.Requests.RegisterRequest;

namespace RevenueRecognitionSystem.API.Controllers;

[ApiController]
[Route("")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> RegisterEmployee(RegisterRequest model)
    {
        try {
            await _employeeService.RegisterEmployeeAsync(model);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}