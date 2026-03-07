using System.Net;
using System.Net.Http.Json;
using RevenueRecognitionSystem.Domain.Exceptions.Revenue;
using RevenueRecognitionSystem.Services.Interfaces;

namespace RevenueRecognitionSystem.Services.Implementations;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _client;

    public ExchangeRateService(HttpClient client)
    {
        _client = client;
    }

    public async Task<decimal> GetExchangeAsync(string currencyToExchange)
    {
        var response = await _client.GetAsync(
            $"https://api.nbp.pl/api/exchangerates/rates/a/{currencyToExchange}/?format=json");
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new InvalidCurrencyException($"Currency '{currencyToExchange}' is invalid.");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<NbpResponse>();
        if (result == null || result.Rates.Count == 0)
            throw new InvalidCurrencyException($"Exchange rate for '{currencyToExchange}' not found.");
        
        return result.Rates[0].Mid;
    }
}

public class NbpResponse
{
    public List<NbpRate> Rates { get; set; } = new();
}

public class NbpRate
{
    public decimal Mid { get; set; }
}