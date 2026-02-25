using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;

namespace RevenueRecognitionSystem.Services.Interfaces;

public interface IRevenueCalculationService
{
    Task<RevenueResponse> CalculateRevenueAsync(RevenueRequest request);
}