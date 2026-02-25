namespace RevenueRecognitionSystem.Services.Contracts.Responses;

public class RevenueResponse
{
    public decimal CurrentRevenue { get; set; }
    public decimal PredictedRevenue { get; set; }
    public string Currency { get; set; } = "PLN";
}