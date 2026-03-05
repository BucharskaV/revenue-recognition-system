using Microsoft.Extensions.Logging;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Exceptions.Contract;
using RevenueRecognitionSystem.Domain.Exceptions.SoftwareSystem;
using RevenueRecognitionSystem.Domain.Exceptions.Subscription;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;
using RevenueRecognitionSystem.Services.Mappers;

namespace RevenueRecognitionSystem.Services.Implementations;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IContractRepository _contractRepository;
    private readonly ISoftwareRepository _softwareRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IContractRepository contractRepository, ISoftwareRepository softwareRepository,
        IDiscountRepository discountRepository, IClientRepository clientRepository, ILogger<SubscriptionService> logger)
    {
        _subscriptionRepository = subscriptionRepository;
        _contractRepository = contractRepository;
        _softwareRepository = softwareRepository;
        _discountRepository = discountRepository;
        _clientRepository = clientRepository;
        _logger = logger;
    }
    
    public async Task CreateSubscriptionAsync(CreateSubscriptionRequest request)
    {
        var client = await _clientRepository.GetClientByIdAsync(request.ClientId);
        if (client is null)
        {
            _logger.LogWarning("Client with id {ClientId} not found", request.ClientId);
            throw new ClientNotFoundException(request.ClientId);
        }
        var software = await _softwareRepository.GetSoftwareSystemByIdAsync(request.SoftwareSystemId);
        if (software is null)
        {
            _logger.LogWarning("Software system with id {SoftwareId} not found", request.SoftwareSystemId);
            throw new SoftwareSystemNotFoundException(request.SoftwareSystemId);
        }
        
        var hasActiveContract = await _contractRepository.HasActiveContractsAsync(request.ClientId, request.SoftwareSystemId);
        if (hasActiveContract)
        {
            _logger.LogWarning("Client {ClientId} already has active contract for software {SoftwareId}", request.ClientId, request.SoftwareSystemId);
            throw new ClientAlreadyHasContractException();
        }
        
        if (request.RenewalPeriodInMonths < 1 || request.RenewalPeriodInMonths > 24)
        {
            _logger.LogWarning("Invalid renewal period {RenewalPeriod} months for client {ClientId}", request.RenewalPeriodInMonths, request.ClientId);
            throw new InvalidRenewalPeriodException();
        }

        decimal price = software.OneYearLicensePrice / 12;
        
        var isReturning = await _clientRepository.IsLoyalClientAsync(request.ClientId);
        if (isReturning)
        {
            price -= price * 0.05m;
            _logger.LogWarning("Invalid renewal period {RenewalPeriod} months for client {ClientId}", request.RenewalPeriodInMonths, request.ClientId);
        }
        var subscription = SubscriptionMapper.ToEntity(request, price);
        await _subscriptionRepository.AddAsync(subscription);
        _logger.LogInformation("Subscription created successfully for client {ClientId}", request.ClientId);
        
        var now = DateTime.UtcNow;
        var discountsForFirstPayment = await _discountRepository.GetActiveDiscountsAsync(request.SoftwareSystemId, now);
        if (discountsForFirstPayment.Any())
        {
            var maxDiscount = discountsForFirstPayment.Max(d => d.Percentage);
            price -= price * (maxDiscount / 100m);
            _logger.LogInformation("Applied first payment discount {Discount}% for client {ClientId}", maxDiscount, request.ClientId);
        }
        
        var payment = new SubscriptionPayment
        {
            SubscriptionId = subscription.Id,
            Amount = price,
            PaymentDate = DateTime.UtcNow
        };

        await _subscriptionRepository.AddPaymentAsync(payment);
        _logger.LogInformation("First subscription payment recorded for client {ClientId}, amount {Amount}", request.ClientId, price);
    }

    public async Task<IEnumerable<GetAllSubscriptionsResponse>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetAllAsync();
        return subscriptions.Select(SubscriptionMapper.ToResponse);
    }

    public async Task PayForRenewalAsync(int subscriptionId, decimal amount)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionWithPaymentByIdAsync(subscriptionId);
        if(subscription is null)
        {
            _logger.LogWarning("Subscription {SubscriptionId} not found", subscriptionId);
            throw new SubscriptionNotFoundException(subscriptionId);
        }
        
        if(subscription.IsCancelled)
        {
            _logger.LogWarning("Subscription {SubscriptionId} is cancelled", subscriptionId);
            throw new SubscriptionIsCancelledException(subscriptionId);
        }
        
        var now = DateTime.UtcNow;
        var totalMonths = ((now.Year - subscription.StartDate.Year) * 12)
            + now.Month - subscription.StartDate.Month;
        var currentPeriodNumber = totalMonths / subscription.RenewalPeriodInMonths;
        var currentPeriodStart = subscription.StartDate
            .AddMonths(currentPeriodNumber * subscription.RenewalPeriodInMonths);
        var currentPeriodEnd = currentPeriodStart
            .AddMonths(subscription.RenewalPeriodInMonths);
        
        if (now < currentPeriodStart || now >= currentPeriodEnd)
        {
            _logger.LogWarning("Payment attempted outside current period for subscription {SubscriptionId}", subscriptionId);
            throw new PaymentOutsidePeriodException();
        }
        
        var alreadyPaid = subscription.SubscriptionPayments.Any(p =>
            p.PaymentDate >= currentPeriodStart &&
            p.PaymentDate < currentPeriodEnd);
        
        if (alreadyPaid)
        {
            _logger.LogWarning("Payment already made for current period of subscription {SubscriptionId}", subscriptionId);
            throw new AlreadyPaidPeriodException();
        }
        
        if (currentPeriodNumber > 0)
        {
            var previousPeriodStart = subscription.StartDate
                .AddMonths((currentPeriodNumber - 1) * subscription.RenewalPeriodInMonths);
            var previousPeriodEnd = previousPeriodStart
                .AddMonths(subscription.RenewalPeriodInMonths);
            
            var previousPaid = subscription.SubscriptionPayments.Any(p =>
                p.PaymentDate >= previousPeriodStart &&
                p.PaymentDate < previousPeriodEnd);
            if (!previousPaid)
            {
                subscription.IsCancelled = true;
                _logger.LogWarning("Previous period not paid for subscription {SubscriptionId}. Subscription cancelled.", subscriptionId);
                throw new PreviousPeriodUnpaidException();
            }
            
            if (amount != subscription.RenewalPrice)
            {
                _logger.LogWarning("Incorrect payment amount {Amount} for subscription {SubscriptionId}", amount, subscriptionId);
                throw new IncorrectPaymentAmountException();
            }
            
            var payment = new SubscriptionPayment
            {
                SubscriptionId = subscription.Id,
                Amount = amount,
                PaymentDate = now
            };

            await _subscriptionRepository.AddPaymentAsync(payment);
            await _subscriptionRepository.SaveChangesAsync();
            _logger.LogInformation("Renewal payment of {Amount} recorded for subscription {SubscriptionId}", amount, subscriptionId);
        }

    }
}