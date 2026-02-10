using RevenueRecognitionSystem.Services.DTOs;

namespace RevenueRecognitionSystem.Services.Contracts.Responses;

public class GetAllContractsResponse
{
    public int Id { get; set; }
    public ClientDto Client { get; set; }
    public SoftwareSystemDto SoftwareSystem { get; set; }
    public string SoftwareVersion { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; } 
    public int UpdateYears { get; set; } 
    public bool IsPaid { get; set; } = false;
    public bool IsCancelled { get; set; } = false;
}
