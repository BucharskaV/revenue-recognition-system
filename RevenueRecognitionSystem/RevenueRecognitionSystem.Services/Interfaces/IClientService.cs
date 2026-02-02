using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;

namespace RevenueRecognitionSystem.Services.Interfaces;

public interface IClientService
{
    Task<Client?> GetClientAsync(int id);
    Task<IEnumerable<GetAllClientsResponse>> GetAllClientsAsync();
    Task AddIndividualAsync(AddIndividualRequest request);
    Task AddCompanyAsync(AddCompanyRequest request);
    Task UpdateIndividualAsync(int id, UpdateIndividualRequest request);
    Task UpdateCompanyAsync(int id, UpdateCompanyRequest request);
    Task DeleteClientAsync(int id);
}