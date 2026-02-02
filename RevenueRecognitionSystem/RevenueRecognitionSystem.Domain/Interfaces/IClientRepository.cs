using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(int id);
    Task<Individual?> GetIndividualByIdAsync(int id);
    Task<Company?> GetCompanyByIdAsync(int id);
    Task<Individual?> GetIndividualByPeselAsync(string pesel);
    Task<Company?> GetCompanyByKrsNumberAsync(string krs);
    Task<IEnumerable<Client>> GetClientsWithSoftwareSystemsAsync();
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
}