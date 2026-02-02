using RevenueRecognitionSystem.Domain.Enums;

namespace RevenueRecognitionSystem.Domain.Entities;

public class SoftwareSystem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
    public SoftwareCategory Category { get; set; }
    public decimal? UpfrontPrice { get; set; }
    public decimal? MonthlySubscriptionPrice { get; set; }
    public bool IsSubscriptionBased => MonthlySubscriptionPrice.HasValue;
    public bool HasUpfrontCost => UpfrontPrice.HasValue;
    
    public virtual Client Client { get; set; }
    public int IdClient { get; set; }
}