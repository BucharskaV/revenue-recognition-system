using RevenueRecognitionSystem.Domain.Enums;

namespace RevenueRecognitionSystem.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExp { get; set; } 
    public EmployeeRole Role { get; set; }
}