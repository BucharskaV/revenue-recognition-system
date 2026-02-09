using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;

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
}