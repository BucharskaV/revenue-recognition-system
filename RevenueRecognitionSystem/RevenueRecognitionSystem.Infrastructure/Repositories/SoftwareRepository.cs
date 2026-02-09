using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;

namespace RevenueRecognitionSystem.Infrastructure.Repositories;

public class SoftwareRepository : ISoftwareRepository
{
    private readonly ApplicationDbContext _context;

    public SoftwareRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SoftwareSystem?> GetSoftwareSystemByIdAsync(int id) => await _context.SoftwareSystems.FirstOrDefaultAsync(s => s.Id == id);
}