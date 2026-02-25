namespace RevenueRecognitionSystem.Services.Contracts.Requests;

public class RevenueRequest
{
    public string? Currency { get; set; }
    public int? ClientId { get; set; }
    public int? SoftwareSystemId { get; set; }
}