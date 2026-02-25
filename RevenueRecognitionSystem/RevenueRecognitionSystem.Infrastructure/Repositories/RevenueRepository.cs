using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;

namespace RevenueRecognitionSystem.Infrastructure.Repositories;

public class RevenueRepository : IRevenueRepository
{
    private readonly ApplicationDbContext _context;

    public RevenueRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<Contract> GetPaidContracts() => _context.Contracts.Where(c => c.IsPaid == true);

    public IQueryable<SubscriptionPayment> GetSubscriptionPayments()
    {
        return _context.SubscriptionPayments
            .Include(s => s.Subscription);
    }

    public IQueryable<Contract> GetUnpaidContracts() => _context.Contracts.Where(c => c.IsPaid == false && c.IsCancelled == false);

    public IQueryable<Subscription> GetActiveSubscriptions() => _context.Subscriptions.Where(s => s.IsCancelled == false);
}