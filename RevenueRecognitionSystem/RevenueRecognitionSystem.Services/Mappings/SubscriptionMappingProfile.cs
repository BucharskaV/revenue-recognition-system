using AutoMapper;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.DTOs;

namespace RevenueRecognitionSystem.Services.Mappings;

public class SubscriptionMappingProfile : Profile
{
    public SubscriptionMappingProfile()
    {
        CreateMap<Subscription, GetAllSubscriptionsResponse>();
        CreateMap<Client, ClientDto>();
        CreateMap<SoftwareSystem, SoftwareSystemDto>();
        
        CreateMap<CreateSubscriptionRequest, Subscription>();
    }
}