namespace RevenueRecognitionSystem.Domain.Entities;

public class Contract
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public virtual Client Client { get; set; }
    public int SoftwareSystemId { get; set; }
    public virtual SoftwareSystem SoftwareSystem { get; set; }
    public string SoftwareVersion { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; } // Price including discounts
    public int UpdateYears { get; set; } // Minimum 1, extra support 1–3 years
    public bool IsPaid { get; set; } = false;
    public bool IsCancelled { get; set; } = false;
}