using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;

namespace RevenueRecognitionSystem.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Client?> GetClientByIdAsync(int id) => await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
    public async Task<Individual?> GetIndividualByIdAsync(int id)=> await _context.Individuals.FirstOrDefaultAsync(c => c.Id == id);
    public async Task<Company?> GetCompanyByIdAsync(int id)=> await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

    public Task<Individual?> GetIndividualByPeselAsync(string pesel) => _context.Individuals.FirstOrDefaultAsync(c => c.PESEL == pesel);

    public Task<Company?> GetCompanyByKrsNumberAsync(string krs) => _context.Companies.FirstOrDefaultAsync(c => c.KRSNumber == krs);

    public async Task<IEnumerable<Client>> GetClientsWithSoftwareSystemsAsync()
    {
        return await _context.Clients.Include(c => c.SoftwareSystems).ToListAsync();
    }

    public async Task AddAsync(Client client)
    {
        _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }
}