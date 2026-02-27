using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByLoginAsync(string login);
    Task<Employee?> GetEmployeeByRefreshTokenAsync(string token);
    Task AddEmployeeAsync(Employee employee);
    Task SaveChangesAsync();
}