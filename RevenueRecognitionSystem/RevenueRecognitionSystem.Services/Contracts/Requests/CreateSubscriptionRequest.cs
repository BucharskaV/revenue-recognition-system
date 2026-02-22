namespace RevenueRecognitionSystem.Services.Contracts.Requests;

public class CreateSubscriptionRequest
{
    public int ClientId { get; set; }
    public int SoftwareSystemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RenewalPeriodInMonths { get; set; }
}