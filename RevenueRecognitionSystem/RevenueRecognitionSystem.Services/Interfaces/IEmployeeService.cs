using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Services.Contracts.Requests;

namespace RevenueRecognitionSystem.Services.Interfaces;

public interface IEmployeeService
{
    Task RegisterEmployeeAsync(RegisterRequest model);
}