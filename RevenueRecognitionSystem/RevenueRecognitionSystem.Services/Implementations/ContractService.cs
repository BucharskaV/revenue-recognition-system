using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Exceptions.Contract;
using RevenueRecognitionSystem.Domain.Exceptions.Payment;
using RevenueRecognitionSystem.Domain.Exceptions.SoftwareSystem;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;
using RevenueRecognitionSystem.Services.Mappers;

namespace RevenueRecognitionSystem.Services.Implementations;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly ISoftwareRepository _softwareRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IClientRepository _clientRepository;

    public ContractService(IContractRepository contractRepository, ISoftwareRepository softwareRepository,
        IDiscountRepository discountRepository, IClientRepository clientRepository)
    {
        _contractRepository = contractRepository;
        _softwareRepository = softwareRepository;
        _discountRepository = discountRepository;
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<GetAllContractsResponse>> GetAllContractsAsync()
    {
        var contracts = await _contractRepository.GetAllAsync();
        return contracts.Select(ContractMapper.ToResponse);
    }

    public async Task CreateUpfrontContractAsync(CreateUpfrontContractRequest request)
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
        
        var contractDuration = (request.EndDate - request.StartDate).TotalDays;
        if (contractDuration < 3 || contractDuration > 30)
            throw new InvalidDurationException();

        if (request.ExtraSupportYears < 0 || request.ExtraSupportYears > 3)
            throw new InvalidSupportYearsNumberException();

        decimal price = software.OneYearLicensePrice;
        
        var discounts = await _discountRepository.GetActiveDiscountsAsync(request.SoftwareSystemId, request.StartDate);
        if (discounts.Any())
        {
            var maxDiscount = discounts.Max(d => d.Percentage);
            price -= price * (maxDiscount / 100m);
        }
        
        var isReturning = await _clientRepository.IsLoyalClientAsync(request.ClientId);
        if (isReturning)
        {
            price -= price * 0.05m;
        }
        
        price += request.ExtraSupportYears * 1000m;

        var contractEntity = ContractMapper.ToEntity(request, price);

        await _contractRepository.AddAsync(contractEntity);
    }

    public async Task DeleteUpfrontContractAsync(int contractId)
    {
        var contract = await _contractRepository.GetContractByIdAsync(contractId);
        if (contract is null)
        {
            throw new ContractNotFoundException(contractId);
        }
        await _contractRepository.DeleteAsync(contractId);
    }

    public async Task IssuePaymentAsync(int contractId, decimal amount)
    {
        var contract = await _contractRepository.GetContractWithPaymentByContractIdAsync(contractId);
        if(contract is null)
            throw new ContractNotFoundException(contractId);
        
        if(contract.IsCancelled)
            throw new ContractIsCancelledException(contractId);

        if (contract.IsPaid)
            throw new ContractIsFullyPaidException(contractId);
        
        var now = DateTime.UtcNow;
        if (now > contract.EndDate)
        {
            contract.IsCancelled = true;
            throw new ContractHasExpiredException(contract.EndDate);
        }

        if (amount < 0)
            throw new InvalidPaymentAmountException();
        
        var totalPaid = contract.Payments.Sum(p => p.Amount);
        var newTotal = totalPaid + amount;

        if (newTotal > contract.Price)
            throw new TotalPaymentExceedException();

        var payment = new Payment
        {
            ContractId = contract.Id,
            Amount = amount,
            PaymentDate = now
        };
        await _contractRepository.AddPaymentAsync(payment);
        if (newTotal == contract.Price)
        {
            contract.IsPaid = true;

            // Revenue recognition happens HERE

            await _contractRepository.SaveChangesAsync();
        }
    }
}