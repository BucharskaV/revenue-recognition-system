using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
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

    public EmployeeService(IEmployeeRepository employeeRepository, IConfiguration configuration)
    {
        _employeeRepository = employeeRepository;
        _configuration = configuration;
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
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var employee = await _employeeRepository.GetEmployeeByLoginAsync(loginRequest.Login);
        if(employee == null)
            throw new InvalidEmployeeLoginException();
        
        string passwordHashFromDb = employee.Password;
        string currentHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, employee.Salt);

        if (passwordHashFromDb != currentHashedPassword)
        {
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
            throw new InvalidEmployeeRefreshTokenException();
        if (employee.RefreshTokenExp < DateTime.UtcNow)
        {
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
    }
}