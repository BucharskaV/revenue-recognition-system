using Microsoft.Extensions.Logging;
using Moq;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Implementations;
using RevenueRecognitionSystem.Services.Interfaces;

namespace RevenueRecognitionSystem.Tests;

public class RevenueCalculationServiceTests
{
    private readonly Mock<IRevenueRepository> _revenueRepositoryMock;
    private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
    private readonly Mock<ILogger<RevenueCalculationService>> _loggerMock;
    
    private readonly RevenueCalculationService _service;

    public RevenueCalculationServiceTests()
    {
        _revenueRepositoryMock = new Mock<IRevenueRepository>();
        _exchangeRateServiceMock = new Mock<IExchangeRateService>();
        _loggerMock = new Mock<ILogger<RevenueCalculationService>>();
        _service = new RevenueCalculationService(
            _revenueRepositoryMock.Object,
            _exchangeRateServiceMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task CalculateRevenueAsync_InDefaultCurrency_ReturnsCorrectResult()
    {
        var contracts = new List<Contract>
        {
            new Contract { Price = 1200m, ClientId = 1, SoftwareSystemId = 10 }
        }.AsQueryable();
        var subscriptionPayments = new List<SubscriptionPayment>
        {
            new SubscriptionPayment { Amount = 300m, Subscription = new Subscription { ClientId = 1, SoftwareSystemId = 10, RenewalPrice = 300m } }
        }.AsQueryable();
        var unpaidContracts = new List<Contract>
        {
            new Contract { Price = 400m, ClientId = 1, SoftwareSystemId = 10 }
        }.AsQueryable();
        var activeSubs = new List<Subscription>
        {
            new Subscription { ClientId = 1, SoftwareSystemId = 10, RenewalPrice = 100m }
        }.AsQueryable();

        _revenueRepositoryMock.Setup(r => r.GetPaidContracts()).Returns(contracts);
        _revenueRepositoryMock.Setup(r => r.GetSubscriptionPayments()).Returns(subscriptionPayments);
        _revenueRepositoryMock.Setup(r => r.GetUnpaidContracts()).Returns(unpaidContracts);
        _revenueRepositoryMock.Setup(r => r.GetActiveSubscriptions()).Returns(activeSubs);
        _exchangeRateServiceMock.Setup(e => e.GetExchangeAsync("USD")).ReturnsAsync(4m); // 1 USD = 4 PLN

        var request = new RevenueRequest { Currency = "USD" };

        var result = await _service.CalculateRevenueAsync(request);

        Assert.That(result.CurrentRevenue, Is.EqualTo(1500m / 4m));
        Assert.That(result.PredictedRevenue, Is.EqualTo(2000m / 4m));
        Assert.That(result.Currency, Is.EqualTo("USD"));
    }
    
    [Test]
    public async Task CalculateRevenueAsync_InUSDCurrency_ReturnsCorrectResult()
    {
        var contracts = new List<Contract>
        {
            new Contract { Price = 1200m, ClientId = 1, SoftwareSystemId = 10 }
        }.AsQueryable();
        var subscriptionPayments = new List<SubscriptionPayment>
        {
            new SubscriptionPayment { Amount = 300m, Subscription = new Subscription { ClientId = 1, SoftwareSystemId = 10, RenewalPrice = 300m } }
        }.AsQueryable();
        var unpaidContracts = new List<Contract>
        {
            new Contract { Price = 400m, ClientId = 1, SoftwareSystemId = 10 }
        }.AsQueryable();
        var activeSubs = new List<Subscription>
        {
            new Subscription { ClientId = 1, SoftwareSystemId = 10, RenewalPrice = 100m }
        }.AsQueryable();

        _revenueRepositoryMock.Setup(r => r.GetPaidContracts()).Returns(contracts);
        _revenueRepositoryMock.Setup(r => r.GetSubscriptionPayments()).Returns(subscriptionPayments);
        _revenueRepositoryMock.Setup(r => r.GetUnpaidContracts()).Returns(unpaidContracts);
        _revenueRepositoryMock.Setup(r => r.GetActiveSubscriptions()).Returns(activeSubs);
        _exchangeRateServiceMock.Setup(e => e.GetExchangeAsync("USD")).ReturnsAsync(4m); // 1 USD = 4 PLN

        var request = new RevenueRequest { Currency = "USD" };
        var result = await _service.CalculateRevenueAsync(request);
        Assert.That(result.CurrentRevenue, Is.EqualTo(1500m / 4m));
        Assert.That(result.PredictedRevenue, Is.EqualTo(2000m / 4m));
        Assert.That(result.Currency, Is.EqualTo("USD"));
    }
}