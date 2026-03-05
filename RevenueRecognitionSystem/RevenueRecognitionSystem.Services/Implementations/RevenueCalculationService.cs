using Microsoft.Extensions.Logging;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;

namespace RevenueRecognitionSystem.Services.Implementations;

public class RevenueCalculationService : IRevenueCalculationService
{
    private readonly IRevenueRepository _revenueRepository;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<RevenueCalculationService> _logger;

    public RevenueCalculationService(IRevenueRepository repo, IExchangeRateService exchange, ILogger<RevenueCalculationService> logger)
    {
        _revenueRepository = repo;
        _exchangeRateService = exchange;
        _logger = logger;
    }
    
    public async Task<RevenueResponse> CalculateRevenueAsync(RevenueRequest request)
    {
        var currency = request.Currency?.ToUpper() ?? "PLN";
        var currentRevenue = 0m;
        
        var paidContracts = _revenueRepository.GetPaidContracts();
        if (request.SoftwareSystemId.HasValue)
        {
            paidContracts = paidContracts.Where(c => c.SoftwareSystemId == request.SoftwareSystemId);
        }
        if (request.ClientId.HasValue)
        {
            paidContracts = paidContracts.Where(c => c.ClientId == request.ClientId);
        }
        currentRevenue += paidContracts.Sum(c => c.Price);
        var subscriptionPayments = _revenueRepository.GetSubscriptionPayments();
        if (request.SoftwareSystemId.HasValue)
        {
            subscriptionPayments = subscriptionPayments.Where(p => p.Subscription.SoftwareSystemId == request.SoftwareSystemId);
        }
        if (request.ClientId.HasValue)
        {
            subscriptionPayments = subscriptionPayments.Where(p => p.Subscription.ClientId == request.ClientId);
        }
        currentRevenue += subscriptionPayments.Sum(s => s.Amount);

        var predictedRevenue = currentRevenue;
        var unpaidContracts = _revenueRepository.GetUnpaidContracts();
        if (request.SoftwareSystemId.HasValue)
        {
            unpaidContracts = unpaidContracts.Where(c => c.SoftwareSystemId == request.SoftwareSystemId);
        }
        if (request.ClientId.HasValue)
        {
            unpaidContracts = unpaidContracts.Where(c => c.ClientId == request.ClientId);
        }
        predictedRevenue += unpaidContracts.Sum(c => c.Price);
        var activeSubs = _revenueRepository.GetActiveSubscriptions();
        if (request.SoftwareSystemId.HasValue)
        {
            activeSubs = activeSubs.Where(s => s.SoftwareSystemId == request.SoftwareSystemId);
        }
        if (request.ClientId.HasValue)
        {
            activeSubs = activeSubs.Where(s => s.ClientId == request.ClientId);
        }
        foreach (var s in activeSubs)
            predictedRevenue += s.RenewalPrice;

        if (currency != "PLN")
        {
            var rate = await _exchangeRateService.GetExchangeAsync(currency);
            currentRevenue /= rate;
            predictedRevenue /= rate;
        }
        
        _logger.LogInformation("Revenue calculated successfully");
        return new RevenueResponse
        {
            CurrentRevenue = currentRevenue,
            PredictedRevenue = predictedRevenue,
            Currency = currency
        };
    }
}