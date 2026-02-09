using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Enums;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;

namespace RevenueRecognitionSystem.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly ApplicationDbContext _context;

    public DiscountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Discount>> GetActiveDiscountsAsync(int softwareSystemId, DateTime contractStartDate)
    {
        return await _context.Discounts.Where(d => d.SoftwareSystemId == softwareSystemId &&
                                                   d.Target == DiscountTarget.Upfront &&
                                                   d.StartDate <= contractStartDate &&
                                                   d.EndDate >= contractStartDate).ToListAsync();
    }
}