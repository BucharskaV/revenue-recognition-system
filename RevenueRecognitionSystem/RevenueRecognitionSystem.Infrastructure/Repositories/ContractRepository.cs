using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;

namespace RevenueRecognitionSystem.Infrastructure.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly ApplicationDbContext _context;

    public ContractRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
    } 

    public async Task<bool> HasAnyPreviousContractsAsync(int clientId)
    {
        return await _context.Contracts.AnyAsync(c =>
            c.ClientId == clientId && c.IsPaid);
    }

    public async Task<bool> HasActiveContractsAsync(int clientId, int softwareId)
    {
        var now = DateTime.UtcNow;

        return await _context.Contracts.AnyAsync(c =>
            c.ClientId == clientId && c.SoftwareSystemId == softwareId && !c.IsCancelled && (
                !c.IsPaid 
                || (c.IsPaid && c.EndDate >= now)
                )); 
    }
}