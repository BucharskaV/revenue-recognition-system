using RevenueRecognitionSystem.Domain.Enums;

namespace RevenueRecognitionSystem.Services.Contracts.Responses;

public class GetAllClientsResponse
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<SoftwareSystemDto> SoftwareSystems { get; set; } = [];
}

public class SoftwareSystemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
    public SoftwareCategory Category { get; set; }
    public decimal? UpfrontPrice { get; set; }
    public decimal? MonthlySubscriptionPrice { get; set; }
}