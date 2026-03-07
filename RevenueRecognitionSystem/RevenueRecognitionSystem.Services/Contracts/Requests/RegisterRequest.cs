using RevenueRecognitionSystem.Domain.Enums;

namespace RevenueRecognitionSystem.Services.Contracts.Requests;

public class RegisterRequest
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public EmployeeRole Role { get; set; }
}