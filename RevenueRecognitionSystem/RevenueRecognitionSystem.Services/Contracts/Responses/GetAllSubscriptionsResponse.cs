using RevenueRecognitionSystem.Services.DTOs;

namespace RevenueRecognitionSystem.Services.Contracts.Responses;

public class GetAllSubscriptionsResponse
{
    public int Id { get; set; }
    public ClientDto Client { get; set; }
    public SoftwareSystemDto SoftwareSystem { get; set; }
    public string Name { get; set; }
    public int RenewalPeriodInMonths { get; set; }
    public decimal RenewalPrice { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsCancelled { get; set; }
}