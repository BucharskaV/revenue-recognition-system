using AutoMapper;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<ContractService> _logger;
    private readonly IMapper _mapper;

    public ContractService(IContractRepository contractRepository, ISoftwareRepository softwareRepository,
        IDiscountRepository discountRepository, IClientRepository clientRepository, ILogger<ContractService> logger, IMapper mapper)
    {
        _contractRepository = contractRepository;
        _softwareRepository = softwareRepository;
        _discountRepository = discountRepository;
        _clientRepository = clientRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllContractsResponse>> GetAllContractsAsync()
    {
        var contracts = await _contractRepository.GetAllAsync();
        _logger.LogInformation("{Count} contracts fetched", contracts.Count());
        return _mapper.Map<IEnumerable<GetAllContractsResponse>>(contracts);
    }

    public async Task CreateUpfrontContractAsync(CreateUpfrontContractRequest request)
    {
        var client = await _clientRepository.GetClientByIdAsync(request.ClientId);
        if (client is null)
        {
            _logger.LogWarning("Client with ID {ClientId} not found", request.ClientId);
            throw new ClientNotFoundException(request.ClientId);
        }
        var software = await _softwareRepository.GetSoftwareSystemByIdAsync(request.SoftwareSystemId);
        if (software is null)
        {
            _logger.LogWarning("Software with ID {SoftwareSystemId} not found", request.SoftwareSystemId);
            throw new SoftwareSystemNotFoundException(request.SoftwareSystemId);
        }
        
        var hasActiveContract = await _contractRepository.HasActiveContractsAsync(request.ClientId, request.SoftwareSystemId);
        if (hasActiveContract)
        {
            _logger.LogWarning(
                "Client with ID {ClientId} already has an active contract for SoftwareSystemId {SoftwareSystemId}",
                request.ClientId, request.SoftwareSystemId);
            throw new ClientAlreadyHasContractException();
        }
        
        var contractDuration = (request.EndDate - request.StartDate).TotalDays;
        if (contractDuration < 3 || contractDuration > 30)
        {
            _logger.LogWarning("Invalid contract duration: {Duration} days", contractDuration);
            throw new InvalidDurationException();
        }

        if (request.ExtraSupportYears < 0 || request.ExtraSupportYears > 3)
        {
            _logger.LogWarning("Invalid extra support years: {ExtraSupportYears}", request.ExtraSupportYears);
            throw new InvalidSupportYearsNumberException();
        }

        decimal price = software.OneYearLicensePrice;
        
        var discounts = await _discountRepository.GetActiveDiscountsAsync(request.SoftwareSystemId, request.StartDate);
        if (discounts.Any())
        {
            var maxDiscount = discounts.Max(d => d.Percentage);
            price -= price * (maxDiscount / 100m);
            _logger.LogInformation("Applied discount: {Discount}% for contract", maxDiscount);
        }
        
        var isReturning = await _clientRepository.IsLoyalClientAsync(request.ClientId);
        if (isReturning)
        {
            price -= price * 0.05m;
            _logger.LogInformation("Applied 5% loyal client discount for ClientId {ClientId}", request.ClientId);
        }
        
        price += request.ExtraSupportYears * 1000m;

        var contractEntity = ContractMapper.ToEntity(request, price);
        await _contractRepository.AddAsync(contractEntity);
        _logger.LogInformation("Applied 5% loyal client discount for ClientId {ClientId}", request.ClientId);
    }

    public async Task DeleteUpfrontContractAsync(int contractId)
    {
        var contract = await _contractRepository.GetContractByIdAsync(contractId);
        if (contract is null)
        {
            _logger.LogWarning("Contract with ID {ContractId} not found", contractId);
            throw new ContractNotFoundException(contractId);
        }
        await _contractRepository.DeleteAsync(contractId);
        _logger.LogInformation("Contract with ID {ContractId} deleted successfully", contractId);
    }

    public async Task IssuePaymentAsync(int contractId, decimal amount)
    {
        var contract = await _contractRepository.GetContractWithPaymentByContractIdAsync(contractId);
        if(contract is null)
        {
            _logger.LogWarning("Contract with ID {ContractId} not found", contractId);
            throw new ContractNotFoundException(contractId);
        }
        
        if(contract.IsCancelled)
        {
            _logger.LogWarning("Contract with ID {ContractId} is cancelled", contractId);
            throw new ContractIsCancelledException(contractId);
        }

        if (contract.IsPaid)
        {
            _logger.LogWarning("Contract with ID {ContractId} is cancelled", contractId);
            throw new ContractIsFullyPaidException(contractId);
        }
        
        var now = DateTime.UtcNow;
        if (now > contract.EndDate)
        {
            contract.IsCancelled = true;
            _logger.LogWarning("Contract with ID {ContractId} has expired at {EndDate}", contractId, contract.EndDate);
            throw new ContractHasExpiredException(contract.EndDate);
        }

        if (amount <= 0)
        {
            _logger.LogWarning("Invalid payment amount: {Amount}", amount);
            throw new InvalidPaymentAmountException();
        }
        
        var totalPaid = contract.Payments.Sum(p => p.Amount);
        var newTotal = totalPaid + amount;

        if (newTotal > contract.Price)
        {
            _logger.LogWarning("Total payment {NewTotal} exceeds contract price {ContractPrice}", newTotal, contract.Price);
            throw new TotalPaymentExceedException();
        }

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
            await _contractRepository.SaveChangesAsync();
        }
    }
}