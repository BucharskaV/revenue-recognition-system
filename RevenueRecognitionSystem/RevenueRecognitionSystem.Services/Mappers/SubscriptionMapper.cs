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
    public static GetAllSubscriptionsResponse ToResponse(Subscription subscription)
    {
        return new GetAllSubscriptionsResponse
        {
            Id = subscription.Id,

            Client = new ClientDto
            {
                Id = subscription.Client.Id,
                Address = subscription.Client.Address,
                Email = subscription.Client.Email,
                PhoneNumber = subscription.Client.PhoneNumber
            },

            SoftwareSystem = new SoftwareSystemDto
            {
                Id = subscription.SoftwareSystem.Id,
                Name = subscription.SoftwareSystem.Name,
                Description = subscription.SoftwareSystem.Description,
                CurrentVersion = subscription.SoftwareSystem.CurrentVersion,
                Category = subscription.SoftwareSystem.Category,
                OneYearLicensePrice = subscription.SoftwareSystem.OneYearLicensePrice
            },

            Name = subscription.Name,
            RenewalPeriodInMonths = subscription.RenewalPeriodInMonths,
            StartDate = subscription.StartDate,
            RenewalPrice = subscription.RenewalPrice,
            IsCancelled = subscription.IsCancelled
        };
    }
}