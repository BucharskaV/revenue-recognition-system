using AutoMapper;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.DTOs;

namespace RevenueRecognitionSystem.Services.Mappings;

public class ContractMappingProfile : Profile
{
    public ContractMappingProfile()
    {
        CreateMap<Contract, GetAllContractsResponse>();
        CreateMap<Client, ClientDto>();
        CreateMap<SoftwareSystem, SoftwareSystemDto>();
        
        CreateMap<CreateUpfrontContractRequest, Contract>();
    }
}