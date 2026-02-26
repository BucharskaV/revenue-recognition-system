using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task AddEmployeeAsync(Employee employee);
}