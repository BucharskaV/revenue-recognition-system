using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface IRevenueRepository
{
    IQueryable<Contract> GetPaidContracts();
    IQueryable<SubscriptionPayment> GetSubscriptionPayments();
    IQueryable<Contract> GetUnpaidContracts();
    IQueryable<Subscription> GetActiveSubscriptions();
}