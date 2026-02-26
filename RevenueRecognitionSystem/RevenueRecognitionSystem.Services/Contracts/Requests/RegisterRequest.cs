using RevenueRecognitionSystem.Domain.Enums;

namespace RevenueRecognitionSystem.Services.Contracts.Requests;

public class RegisterRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public EmployeeRole Role { get; set; }
}