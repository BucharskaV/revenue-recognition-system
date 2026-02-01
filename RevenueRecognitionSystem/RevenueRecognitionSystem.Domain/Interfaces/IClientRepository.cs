using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(int id);
    Task<IEnumerable<Client>> GetClientsAsync();
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
}