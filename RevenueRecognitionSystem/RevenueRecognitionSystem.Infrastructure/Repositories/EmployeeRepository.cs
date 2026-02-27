using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;

namespace RevenueRecognitionSystem.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetEmployeeByLoginAsync(string login) => await _context.Employees.FirstOrDefaultAsync(e => e.Login == login);
    public async Task<Employee?> GetEmployeeByRefreshTokenAsync(string token) => await _context.Employees.FirstOrDefaultAsync(e => e.RefreshToken == token);
    
    public async Task AddEmployeeAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}