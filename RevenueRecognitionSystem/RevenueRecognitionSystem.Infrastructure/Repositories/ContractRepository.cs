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

    public async Task<IEnumerable<Contract>> GetAllAsync()
    {
        return await _context.Contracts
            .Include(c => c.Client)
            .Include(c => c.SoftwareSystem)
            .ToListAsync();
    }

    public async Task<Contract?> GetContractWithPaymentByContractIdAsync(int contractId)
    {
        return await _context.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == contractId);
    }

    public async Task AddAsync(Contract contract)
    {
        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
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

    public async Task<Contract?> GetContractByIdAsync(int contractId)
    {
        return await _context.Contracts.FirstOrDefaultAsync(c => c.Id == contractId);
    }

    public async Task DeleteAsync(int contractId)
    {
        var contract = await _context.Contracts.FindAsync(contractId);
        if (contract is not null)
        {
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}