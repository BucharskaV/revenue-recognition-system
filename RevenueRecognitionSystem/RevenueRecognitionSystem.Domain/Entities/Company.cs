namespace RevenueRecognitionSystem.Domain.Entities;

public class Company : Client
{
    public string CompanyName { get; set; } = string.Empty;
    public string KRSNumber { get; set; } = string.Empty;
}