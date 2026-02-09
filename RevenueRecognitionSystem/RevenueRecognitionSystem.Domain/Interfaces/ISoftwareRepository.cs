using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface ISoftwareRepository
{
    Task<SoftwareSystem?> GetSoftwareSystemByIdAsync(int id);
}