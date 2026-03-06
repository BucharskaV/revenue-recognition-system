using AutoMapper;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;

namespace RevenueRecognitionSystem.Services.Mappings;

public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<AddIndividualRequest, Individual>();
        CreateMap<AddCompanyRequest, Company>();

        CreateMap<Client, GetAllClientsResponse>();
    }
}