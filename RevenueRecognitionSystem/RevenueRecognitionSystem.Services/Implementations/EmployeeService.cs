using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions.Employee;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;
using RevenueRecognitionSystem.Services.Security;

namespace RevenueRecognitionSystem.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IEmployeeRepository employeeRepository, IConfiguration configuration, ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task RegisterEmployeeAsync(RegisterRequest model)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordsAndSalt(model.Password);

        var employee = new Employee()
        {
            Login = model.Login,
            Role = model.Role,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.UtcNow.AddDays(1)
        };
        
        await _employeeRepository.AddEmployeeAsync(employee);
        _logger.LogInformation("Employee {Login} registered successfully with role {Role}", model.Login, model.Role);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var employee = await _employeeRepository.GetEmployeeByLoginAsync(loginRequest.Login);
        if(employee == null)
        {
            _logger.LogWarning("Login failed: user {Login} not found", loginRequest.Login);
            throw new InvalidEmployeeLoginException();
        }
        
        string passwordHashFromDb = employee.Password;
        string currentHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, employee.Salt);

        if (passwordHashFromDb != currentHashedPassword)
        {
            _logger.LogWarning("Login failed: incorrect password for user {Login}", loginRequest.Login);
            throw new IncorrectPasswordException();
        }

        Claim[] employeeClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Name, employee.Login),
            new Claim(ClaimTypes.Role, employee.Role.ToString())
        };
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: employeeClaims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials
            );
        employee.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        employee.RefreshTokenExp = DateTime.Now.AddDays(1);
        await _employeeRepository.SaveChangesAsync();

        _logger.LogInformation("User {Login} logged in successfully", loginRequest.Login);
        return new LoginResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = employee.RefreshToken
        };
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var employee = await _employeeRepository.GetEmployeeByRefreshTokenAsync(refreshTokenRequest.RefreshToken);
        if(employee == null)
        {
            _logger.LogWarning("Invalid refresh token {RefreshToken}", refreshTokenRequest.RefreshToken);
            throw new InvalidEmployeeRefreshTokenException();
        }
        if (employee.RefreshTokenExp < DateTime.UtcNow)
        {
            _logger.LogWarning("Invalid refresh token {RefreshToken}", refreshTokenRequest.RefreshToken);
            throw new ExpiredRefreshTokenException();
        }

        Claim[] employeeClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Name, employee.Login),
            new Claim(ClaimTypes.Role, employee.Role.ToString())
        };
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: employeeClaims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials
        );
        employee.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        employee.RefreshTokenExp = DateTime.Now.AddDays(1);
        await _employeeRepository.SaveChangesAsync();

        _logger.LogInformation("Refresh token issued successfully for user {Login}", employee.Login);
        return new RefreshTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = employee.RefreshToken
        };
    }

    public async Task LogoutAsync(string login)
    {
        var employee = await _employeeRepository.GetEmployeeByLoginAsync(login);
        if (employee == null)
            throw new InvalidEmployeeLoginException();

        employee.RefreshToken = null;
        employee.RefreshTokenExp = null;
        await _employeeRepository.SaveChangesAsync();
        _logger.LogInformation("User {Login} logged out successfully", login);
    }
}