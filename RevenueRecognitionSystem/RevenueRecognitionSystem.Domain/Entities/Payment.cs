namespace RevenueRecognitionSystem.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int ContractId { get; set; }
    public virtual Contract Contract { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
}