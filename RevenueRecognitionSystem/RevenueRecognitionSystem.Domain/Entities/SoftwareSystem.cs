using RevenueRecognitionSystem.Domain.Enums;

namespace RevenueRecognitionSystem.Domain.Entities;

public class SoftwareSystem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
    public SoftwareCategory Category { get; set; }
    public decimal OneYearLicensePrice { get; set; }
    public virtual ICollection<Discount> Discounts { get; set; } = [];
    public virtual ICollection<Contract> Contracts { get; set; } = [];
    public virtual ICollection<Subscription> Subscriptions { get; set; } = [];
}