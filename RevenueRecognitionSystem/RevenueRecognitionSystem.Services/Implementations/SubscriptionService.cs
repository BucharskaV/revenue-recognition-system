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

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IContractRepository contractRepository, ISoftwareRepository softwareRepository,
        IDiscountRepository discountRepository, IClientRepository clientRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _contractRepository = contractRepository;
        _softwareRepository = softwareRepository;
        _discountRepository = discountRepository;
        _clientRepository = clientRepository;
    }
    
    public async Task CreateSubscriptionAsync(CreateSubscriptionRequest request)
    {
        var client = await _clientRepository.GetClientByIdAsync(request.ClientId);
        if (client is null)
        {
            throw new ClientNotFoundException(request.ClientId);
        }
        var software = await _softwareRepository.GetSoftwareSystemByIdAsync(request.SoftwareSystemId);
        if (software is null)
        {
            throw new SoftwareSystemNotFoundException(request.SoftwareSystemId);
        }
        
        var hasActiveContract = await _contractRepository.HasActiveContractsAsync(request.ClientId, request.SoftwareSystemId);
        if (hasActiveContract)
            throw new ClientAlreadyHasContractException();
        
        if (request.RenewalPeriodInMonths < 1 || request.RenewalPeriodInMonths > 24)
            throw new InvalidRenewalPeriodException();

        decimal price = software.OneYearLicensePrice / 12;
        
        var isReturning = await _clientRepository.IsLoyalClientAsync(request.ClientId);
        if (isReturning)
        {
            price -= price * 0.05m;
        }
        var subscription = SubscriptionMapper.ToEntity(request, price);
        await _subscriptionRepository.AddAsync(subscription);
        
        var now = DateTime.UtcNow;
        var discountsForFirstPayment = await _discountRepository.GetActiveDiscountsAsync(request.SoftwareSystemId, now);
        if (discountsForFirstPayment.Any())
        {
            var maxDiscount = discountsForFirstPayment.Max(d => d.Percentage);
            price -= price * (maxDiscount / 100m);
        }
        
        var payment = new SubscriptionPayment
        {
            SubscriptionId = subscription.Id,
            Amount = price,
            PaymentDate = DateTime.UtcNow
        };

        await _subscriptionRepository.AddPaymentAsync(payment);

        //await _revenueService.RecognizeRevenueAsync(renewalPrice);
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
            throw new SubscriptionNotFoundException(subscriptionId);
        
        if(subscription.IsCancelled)
            throw new SubscriptionIsCancelledException(subscriptionId);
        
        var now = DateTime.UtcNow;
        var totalMonths = ((now.Year - subscription.StartDate.Year) * 12)
            + now.Month - subscription.StartDate.Month;
        var currentPeriodNumber = totalMonths / subscription.RenewalPeriodInMonths;
        var currentPeriodStart = subscription.StartDate
            .AddMonths(currentPeriodNumber * subscription.RenewalPeriodInMonths);
        var currentPeriodEnd = currentPeriodStart
            .AddMonths(subscription.RenewalPeriodInMonths);
        
        if (now < currentPeriodStart || now >= currentPeriodEnd)
            throw new PaymentOutsidePeriodException();
        
        var alreadyPaid = subscription.SubscriptionPayments.Any(p =>
            p.PaymentDate >= currentPeriodStart &&
            p.PaymentDate < currentPeriodEnd);
        
        if (alreadyPaid)
            throw new AlreadyPaidPeriodException();
        
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
                throw new PreviousPeriodUnpaidException();
            }
            
            if (amount != subscription.RenewalPrice)
                throw new IncorrectPaymentAmountException();
            
            var payment = new SubscriptionPayment
            {
                SubscriptionId = subscription.Id,
                Amount = amount,
                PaymentDate = now
            };

            await _subscriptionRepository.AddPaymentAsync(payment);
            //await _revenueService.RecognizeRevenueAsync(amount);

            await _subscriptionRepository.SaveChangesAsync();
        }

    }
}