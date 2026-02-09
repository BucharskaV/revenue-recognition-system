using RevenueRecognitionSystem.Domain.Enums;

namespace RevenueRecognitionSystem.Domain.Entities;

public class Discount
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DiscountTarget Target { get; set; }
    public int SoftwareSystemId { get; set; }
    public virtual SoftwareSystem SoftwareSystem { get; set; }
}