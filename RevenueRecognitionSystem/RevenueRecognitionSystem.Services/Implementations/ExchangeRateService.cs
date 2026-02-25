using System.Net.Http.Json;
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
        currencyToExchange = currencyToExchange.ToUpper();
        var response = await _client.GetFromJsonAsync<NbpResponse>(
            $"https://api.nbp.pl/api/exchangerates/rates/a/{currencyToExchange}/?format=json");
        if (response == null || response.Rates.Count == 0)
            throw new Exception($"Exchange rate for {currencyToExchange} not found.");
        return response.Rates[0].Mid;
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