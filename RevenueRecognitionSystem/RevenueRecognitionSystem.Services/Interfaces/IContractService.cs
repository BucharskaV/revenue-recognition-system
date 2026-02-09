using RevenueRecognitionSystem.Services.Contracts.Requests;

namespace RevenueRecognitionSystem.Services.Interfaces;

public interface IContractService
{
    Task CreateUpfrontContractAsync(CreateUpfrontContractRequest request);
}