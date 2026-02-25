namespace RevenueRecognitionSystem.Services.Interfaces;

public interface IExchangeRateService
{
    Task<decimal> GetExchangeAsync(string currencyToExchange);
}