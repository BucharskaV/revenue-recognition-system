using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;

namespace RevenueRecognitionSystem.Services.Interfaces;

public interface IContractService
{
    public Task<IEnumerable<GetAllContractsResponse>> GetAllContractsAsync(); 
    Task CreateUpfrontContractAsync(CreateUpfrontContractRequest request);
    Task DeleteUpfrontContractAsync(int contractId);
    Task IssuePaymentAsync(int contractId, decimal amount);
}