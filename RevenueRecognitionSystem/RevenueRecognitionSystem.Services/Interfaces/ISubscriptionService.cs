using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;

namespace RevenueRecognitionSystem.Services.Interfaces;

public interface ISubscriptionService
{
    Task CreateSubscriptionAsync(CreateSubscriptionRequest request);
    public Task<IEnumerable<GetAllSubscriptionsResponse>> GetAllSubscriptionsAsync();
    public Task PayForRenewalAsync(int subscriptionId, decimal amount);
}