using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.DTOs;

namespace RevenueRecognitionSystem.Services.Mappers;

public static class SubscriptionMapper
{
    public static Subscription ToEntity(CreateSubscriptionRequest request, decimal price)
    {
        Subscription subscription = new Subscription
        {
            ClientId = request.ClientId,
            SoftwareSystemId = request.SoftwareSystemId,
            StartDate = DateTime.UtcNow,
            Name = request.Name,
            RenewalPeriodInMonths = request.RenewalPeriodInMonths,
            RenewalPrice = price,
            IsCancelled = false
        };
        return subscription;
    }
}