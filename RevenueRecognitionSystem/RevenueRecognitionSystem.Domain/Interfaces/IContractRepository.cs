using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface IContractRepository
{
    Task<IEnumerable<Contract>> GetAllAsync();
    Task<Contract?> GetContractWithPaymentByContractIdAsync(int contractId);
    Task AddAsync(Contract contract);
    Task AddPaymentAsync(Payment payment);
    Task<bool> HasActiveContractsAsync(int clientId, int softwareId);
    Task<Contract?> GetContractByIdAsync(int contractId);
    Task DeleteAsync(int contractId);
    Task SaveChangesAsync();
}