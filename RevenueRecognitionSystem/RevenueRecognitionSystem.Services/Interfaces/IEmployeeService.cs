using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;

namespace RevenueRecognitionSystem.Services.Interfaces;

public interface IEmployeeService
{
    Task RegisterEmployeeAsync(RegisterRequest model);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task LogoutAsync(string login);
}