using RevenueRecognitionSystem.Domain.Entities;

namespace RevenueRecognitionSystem.Domain.Interfaces;

public interface IDiscountRepository
{
    Task<List<Discount>> GetActiveDiscountsAsync(int softwareSystemId, DateTime contractStartDate);
}