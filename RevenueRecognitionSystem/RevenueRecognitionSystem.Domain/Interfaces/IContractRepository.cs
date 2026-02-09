using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface IContractRepository
{
    Task AddAsync(Contract contract);
    Task<bool> HasAnyPreviousContractsAsync(int clientId);
    Task<bool> HasActiveContractsAsync(int clientId, int softwareId);
}