namespace RevenueRecognitionSystem.Services.Contracts.Requests;

public class AddCompanyRequest
{
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string KRSNumber { get; set; } = string.Empty;
}