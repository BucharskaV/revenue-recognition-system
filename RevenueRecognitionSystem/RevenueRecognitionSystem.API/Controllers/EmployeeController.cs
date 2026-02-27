using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Domain.Exceptions.Employee;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;
using LoginRequest = RevenueRecognitionSystem.Services.Contracts.Requests.LoginRequest;
using RegisterRequest = RevenueRecognitionSystem.Services.Contracts.Requests.RegisterRequest;

namespace RevenueRecognitionSystem.API.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Login(LoginRequest model)
    {
        try
        {
            var response = await _employeeService.LoginAsync(model);
            return Ok(response);
        }
        catch (Exception ex)when (ex is InvalidEmployeeLoginException or IncorrectPasswordException)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Refresh(RefreshTokenRequest model)
    {
        try
        {
            var response = await _employeeService.RefreshTokenAsync(model);
            return Ok(response);
        }
        catch (InvalidEmployeeRefreshTokenException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ExpiredRefreshTokenException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var login = User.Identity?.Name;
            if (login == null)
                return Unauthorized();
            await _employeeService.LogoutAsync(login);
            return Ok();
        }
        catch (InvalidEmployeeLoginException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}