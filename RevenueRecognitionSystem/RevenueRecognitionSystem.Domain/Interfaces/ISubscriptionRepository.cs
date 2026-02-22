using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetSubscriptionWithPaymentByIdAsync(int id);
    Task<IEnumerable<Subscription>> GetAllAsync();
    Task AddAsync(Subscription? subscription);
    Task AddPaymentAsync(SubscriptionPayment payment);
    Task SaveChangesAsync();
}