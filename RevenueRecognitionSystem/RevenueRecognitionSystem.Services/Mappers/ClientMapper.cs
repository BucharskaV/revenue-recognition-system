using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;

namespace RevenueRecognitionSystem.Services.Mappers;

public static class ClientMapper
{
    public static Individual ToEntity(AddIndividualRequest request)
    {
        Individual individual = new Individual
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PESEL = request.PESEL
        };
        return individual;
    }
    
    public static Company ToEntity(AddCompanyRequest request)
    {
        Company company = new Company
        {
            CompanyName = request.CompanyName,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            KRSNumber = request.KRSNumber
        };
        return company;
    }

    public static GetAllClientsResponse ToResponse(Client client)
    {
        return new GetAllClientsResponse()
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber
        };
    }
}