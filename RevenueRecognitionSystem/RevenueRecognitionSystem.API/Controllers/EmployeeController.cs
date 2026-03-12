using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Services.Contracts.Requests;
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
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RegisterEmployee(RegisterRequest model)
    {
        _logger.LogInformation("Registering new employee");
        await _employeeService.RegisterEmployeeAsync(model);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Login(LoginRequest model)
    {
        _logger.LogInformation("Logining employee");
        var response = await _employeeService.LoginAsync(model);
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Refresh(RefreshTokenRequest model)
    {
        _logger.LogInformation("Refreshing tocken");
        var response = await _employeeService.RefreshTokenAsync(model);
        return Ok(response);
    }
    
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout()
    {
        var login = User.Identity?.Name;
        if (login == null)
            return Unauthorized();
        await _employeeService.LogoutAsync(login);
        return Ok();
    }
}