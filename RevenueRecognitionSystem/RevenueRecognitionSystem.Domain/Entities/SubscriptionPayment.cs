namespace RevenueRecognitionSystem.Domain.Entities;

public class SubscriptionPayment
{
    public int Id { get; set; }
    public int SubscriptionId { get; set; }
    public virtual Subscription Subscription { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
}