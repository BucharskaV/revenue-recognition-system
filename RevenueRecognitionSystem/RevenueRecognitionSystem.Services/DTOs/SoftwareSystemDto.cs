using RevenueRecognitionSystem.Domain.Enums;

namespace RevenueRecognitionSystem.Services.DTOs;

public class SoftwareSystemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
    public SoftwareCategory Category { get; set; }
    public decimal OneYearLicensePrice { get; set; }
}