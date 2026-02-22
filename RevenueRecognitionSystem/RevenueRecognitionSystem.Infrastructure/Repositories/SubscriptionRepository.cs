using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;

namespace RevenueRecognitionSystem.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription?> GetSubscriptionWithPaymentByIdAsync(int id)
    {
        return await _context.Subscriptions
            .Include(c => c.SubscriptionPayments)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Subscription>> GetAllAsync()
    {
        return await _context.Subscriptions
            .Include(c => c.Client)
            .Include(c => c.SoftwareSystem)
            .ToListAsync();
    }

    public async Task AddAsync(Subscription subscription)
    {
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task AddPaymentAsync(SubscriptionPayment payment)
    {
        _context.SubscriptionPayments.Add(payment);
        await _context.SaveChangesAsync();
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}