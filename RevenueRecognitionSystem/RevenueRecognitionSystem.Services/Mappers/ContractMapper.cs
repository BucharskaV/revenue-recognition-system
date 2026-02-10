using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.DTOs;
using RevenueRecognitionSystem.Services.Implementations;

namespace RevenueRecognitionSystem.Services.Mappers;

public static class ContractMapper
{
    public static Contract ToEntity(CreateUpfrontContractRequest request, decimal price)
    {
        Contract contract = new Contract
        {
            ClientId = request.ClientId,
            SoftwareSystemId = request.SoftwareSystemId,
            SoftwareVersion = request.SoftwareVersion,
            CreatedAt = DateTime.UtcNow,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Price = price,
            UpdateYears = 1 + request.ExtraSupportYears,
            IsPaid = false,
            IsCancelled = false
        };
        return contract;
    }
    
    public static GetAllContractsResponse ToResponse(Contract contract)
    {
        return new GetAllContractsResponse
        {
            Id = contract.Id,

            Client = new ClientDto
            {
                Id = contract.Client.Id,
                Address = contract.Client.Address,
                Email = contract.Client.Email,
                PhoneNumber = contract.Client.PhoneNumber
            },

            SoftwareSystem = new SoftwareSystemDto
            {
                Id = contract.SoftwareSystem.Id,
                Name = contract.SoftwareSystem.Name,
                Description = contract.SoftwareSystem.Description,
                CurrentVersion = contract.SoftwareSystem.CurrentVersion,
                Category = contract.SoftwareSystem.Category,
                OneYearLicensePrice = contract.SoftwareSystem.OneYearLicensePrice
            },

            SoftwareVersion = contract.SoftwareVersion,
            CreatedAt = contract.CreatedAt,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Price = contract.Price,
            UpdateYears = contract.UpdateYears,
            IsPaid = contract.IsPaid,
            IsCancelled = contract.IsCancelled
        };
    }
}