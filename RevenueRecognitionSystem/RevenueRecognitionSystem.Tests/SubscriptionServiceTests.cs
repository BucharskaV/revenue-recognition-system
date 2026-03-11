using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Exceptions.Contract;
using RevenueRecognitionSystem.Domain.Exceptions.SoftwareSystem;
using RevenueRecognitionSystem.Domain.Exceptions.Subscription;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Implementations;

namespace RevenueRecognitionSystem.Tests;

public class SubscriptionServiceTests
{
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock;
    private readonly Mock<IContractRepository> _contractRepositoryMock;
    private readonly Mock<ISoftwareRepository> _softwareRepositoryMock;
    private readonly Mock<IDiscountRepository> _discountRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<ILogger<SubscriptionService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    
    private readonly SubscriptionService _service;

    public SubscriptionServiceTests()
    {
        _subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
        _contractRepositoryMock = new Mock<IContractRepository>();
        _softwareRepositoryMock = new Mock<ISoftwareRepository>();
        _discountRepositoryMock = new Mock<IDiscountRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _loggerMock = new Mock<ILogger<SubscriptionService>>();
        _mapperMock = new Mock<IMapper>();
        _service = new SubscriptionService(
            _subscriptionRepositoryMock.Object,
            _contractRepositoryMock.Object,
            _softwareRepositoryMock.Object,
            _discountRepositoryMock.Object,
            _clientRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Test]
    public async Task CreateSubscriptionAsync_ClientDoesNotExist_ThrowsClientNotFoundException()
    {
        var request = new CreateSubscriptionRequest
        {
            ClientId = 1,
            Name = "test",
            RenewalPeriodInMonths = 3,
            SoftwareSystemId = 1
        };
        
        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync((Client?)null);

        Assert.ThrowsAsync<ClientNotFoundException>(() => _service.CreateSubscriptionAsync(request));
    }
    
    [Test]
    public async Task CreateSubscriptionAsync_SoftwareDoesNotExist_ThrowsSoftwareSystemNotFoundException()
    {
        var request = new CreateSubscriptionRequest
        {
            ClientId = 1,
            Name = "test",
            RenewalPeriodInMonths = 3,
            SoftwareSystemId = 1
        };
        
        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync(new Individual { Id = request.ClientId });
        _softwareRepositoryMock.Setup(c => c.GetSoftwareSystemByIdAsync(request.SoftwareSystemId))
            .ReturnsAsync((SoftwareSystem?)null);

        Assert.ThrowsAsync<SoftwareSystemNotFoundException>(() => _service.CreateSubscriptionAsync(request));
    }
    
    [Test]
    public async Task CreateSubscriptionAsync_ClientAlreadyHasContract_ThrowsClientAlreadyHasContractException()
    {
        var request = new CreateSubscriptionRequest
        {
            ClientId = 1,
            Name = "test",
            RenewalPeriodInMonths = 3,
            SoftwareSystemId = 1
        };
        
        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync(new Individual { Id = request.ClientId });
        _softwareRepositoryMock.Setup(c => c.GetSoftwareSystemByIdAsync(request.SoftwareSystemId))
            .ReturnsAsync(new SoftwareSystem { Id = request.SoftwareSystemId });
        _contractRepositoryMock.Setup(c => c.HasActiveContractsAsync(request.ClientId, request.SoftwareSystemId))
            .ReturnsAsync(true);

        Assert.ThrowsAsync<ClientAlreadyHasContractException>(() => _service.CreateSubscriptionAsync(request));
    }
    
    [Test]
    public async Task CreateSubscriptionAsync_InvalidRenewalPeriod_ThrowsInvalidRenewalPeriodException()
    {
        var request = new CreateSubscriptionRequest
        {
            ClientId = 1,
            Name = "test",
            RenewalPeriodInMonths = -1,
            SoftwareSystemId = 1
        };
        
        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync(new Individual { Id = request.ClientId });
        _softwareRepositoryMock.Setup(c => c.GetSoftwareSystemByIdAsync(request.SoftwareSystemId))
            .ReturnsAsync(new SoftwareSystem { Id = request.SoftwareSystemId });
        _contractRepositoryMock.Setup(c => c.HasActiveContractsAsync(request.ClientId, request.SoftwareSystemId))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<InvalidRenewalPeriodException>(() => _service.CreateSubscriptionAsync(request));
    }
    
    [Test]
    public async Task CreateSubscriptionAsync_ValidRequest_AddSubscription()
    {
        var request = new CreateSubscriptionRequest
        {
            ClientId = 1,
            Name = "test",
            RenewalPeriodInMonths = 1,
            SoftwareSystemId = 1
        };
        
        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync(new Individual { Id = request.ClientId });
        _softwareRepositoryMock.Setup(c => c.GetSoftwareSystemByIdAsync(request.SoftwareSystemId))
            .ReturnsAsync(new SoftwareSystem { Id = 10, OneYearLicensePrice = 1200m });
        _contractRepositoryMock.Setup(c => c.HasActiveContractsAsync(request.ClientId, request.SoftwareSystemId))
            .ReturnsAsync(false);
        _discountRepositoryMock.Setup(r => r.GetActiveDiscountsAsync(request.SoftwareSystemId, It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Discount>()); 
        _clientRepositoryMock.Setup(c => c.IsLoyalClientAsync(request.ClientId))
            .ReturnsAsync(true);

        Subscription? addedSubscription = null;
        _subscriptionRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Subscription>()))
            .Callback<Subscription>(s => addedSubscription = s)
            .Returns(Task.CompletedTask);
        SubscriptionPayment? addedPayment = null;
        _subscriptionRepositoryMock.Setup(r => r.AddPaymentAsync(It.IsAny<SubscriptionPayment>()))
            .Callback<SubscriptionPayment>(p => addedPayment = p)
            .Returns(Task.CompletedTask);

        await _service.CreateSubscriptionAsync(request);

        decimal expectedPrice = 1200m / 12m;
        expectedPrice -= expectedPrice * 0.05m;
        Assert.That(addedSubscription, Is.Not.Null);
        Assert.That(addedSubscription.ClientId, Is.EqualTo(request.ClientId));
        Assert.That(addedSubscription.SoftwareSystemId, Is.EqualTo(request.SoftwareSystemId));
        Assert.That(addedPayment.Amount, Is.EqualTo(expectedPrice));
    }
    
    [Test]
    public void PayForRenewalAsync_SubscriptionNotFound_ThrowsSubscriptionNotFoundException()
    {
        _subscriptionRepositoryMock.Setup(r => r.GetSubscriptionWithPaymentByIdAsync(1))
            .ReturnsAsync((Subscription)null);

        Assert.ThrowsAsync<SubscriptionNotFoundException>(() => _service.PayForRenewalAsync(1, 100));
    }
    
    [Test]
    public void PayForRenewalAsync_SubscriptionCancelled_ThrowsSubscriptionIsCancelledException()
    {
        var subscription = new Subscription { Id = 1, IsCancelled = true };
        _subscriptionRepositoryMock.Setup(r => r.GetSubscriptionWithPaymentByIdAsync(1))
            .ReturnsAsync(subscription);

        Assert.ThrowsAsync<SubscriptionIsCancelledException>(() => _service.PayForRenewalAsync(1, 100));
    }
    
    [Test]
    public void PayForRenewalAsync_PreviousPeriodUnpaid_ThrowsPreviousPeriodUnpaidException()
    {
        var now = DateTime.UtcNow;
        var subscription = new Subscription
        {
            Id = 1,
            StartDate = now.AddMonths(-2),
            RenewalPeriodInMonths = 1,
            SubscriptionPayments = new List<SubscriptionPayment>()
        };
        _subscriptionRepositoryMock.Setup(r => r.GetSubscriptionWithPaymentByIdAsync(1))
            .ReturnsAsync(subscription);

        Assert.ThrowsAsync<PreviousPeriodUnpaidException>(() => _service.PayForRenewalAsync(1, 50));
    }
}