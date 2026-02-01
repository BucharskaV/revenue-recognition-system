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
    
    public async Task<IEnumerable<Client>> GetClientsAsync() => await _context.Clients.ToListAsync();

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