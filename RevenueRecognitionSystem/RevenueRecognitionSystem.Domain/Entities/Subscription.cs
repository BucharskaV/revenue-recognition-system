namespace RevenueRecognitionSystem.Domain.Entities;

public class Subscription
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public virtual Client Client { get; set; }
    public int SoftwareSystemId { get; set; }
    public virtual SoftwareSystem SoftwareSystem { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RenewalPeriodInMonths { get; set; } //[1-24]
    public decimal RenewalPrice { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public bool IsCancelled { get; set; } = false;
    public virtual ICollection<SubscriptionPayment> SubscriptionPayments { get; set; } = [];
}