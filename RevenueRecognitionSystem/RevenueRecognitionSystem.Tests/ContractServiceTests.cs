using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Exceptions.Contract;
using RevenueRecognitionSystem.Domain.Exceptions.Payment;
using RevenueRecognitionSystem.Domain.Exceptions.SoftwareSystem;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Implementations;

namespace RevenueRecognitionSystem.Tests;

public class ContractServiceTests
{
    private readonly Mock<IContractRepository> _contractRepositoryMock;
    private readonly Mock<ISoftwareRepository> _softwareRepositoryMock;
    private readonly Mock<IDiscountRepository> _discountRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<ILogger<ContractService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    
    private readonly ContractService _service;

    public ContractServiceTests()
    {
        _contractRepositoryMock = new Mock<IContractRepository>();
        _softwareRepositoryMock = new Mock<ISoftwareRepository>();
        _discountRepositoryMock = new Mock<IDiscountRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _loggerMock = new Mock<ILogger<ContractService>>();
        _mapperMock = new Mock<IMapper>();

        _service = new ContractService(_contractRepositoryMock.Object,
            _softwareRepositoryMock.Object,
            _discountRepositoryMock.Object,
            _clientRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }
    
    [Test]
    public void CreateUpfrontContractAsync_ClientDoesNotExist_ThrowsClientNotFoundException()
    {
        var request = new CreateUpfrontContractRequest
        {
            ClientId = 1,
            SoftwareSystemId = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(5),
            ExtraSupportYears = 1
        };

        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync((Client?)null);

        Assert.ThrowsAsync<ClientNotFoundException>(() => _service.CreateUpfrontContractAsync(request));
    }
    
    [Test]
    public void CreateUpfrontContractAsync_SoftwareDoesNotExist_ThrowsSoftwareSystemNotFoundException()
    {
        var request = new CreateUpfrontContractRequest
        {
            ClientId = 1,
            SoftwareSystemId = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(5),
            ExtraSupportYears = 1
        };
        
        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync(new Individual { Id = request.ClientId });
        _softwareRepositoryMock.Setup(c => c.GetSoftwareSystemByIdAsync(request.SoftwareSystemId))
            .ReturnsAsync((SoftwareSystem?)null);

        Assert.ThrowsAsync<SoftwareSystemNotFoundException>(() => _service.CreateUpfrontContractAsync(request));
    }
    
    [Test]
    public void CreateUpfrontContractAsync_InvalidDuration_ThrowsInvalidDurationException()
    {
        var request = new CreateUpfrontContractRequest
        {
            ClientId = 1,
            SoftwareSystemId = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(2),
            ExtraSupportYears = 1
        };
        
        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync(new Individual { Id = request.ClientId });
        _softwareRepositoryMock.Setup(c => c.GetSoftwareSystemByIdAsync(request.SoftwareSystemId))
            .ReturnsAsync(new SoftwareSystem { Id = request.SoftwareSystemId });

        Assert.ThrowsAsync<InvalidDurationException>(() => _service.CreateUpfrontContractAsync(request));
    }
    
    [Test]
    public void CreateUpfrontContractAsync_InvalidExtraSupportYearsNumber_ThrowsInvalidSupportYearsNumberException()
    {
        var request = new CreateUpfrontContractRequest
        {
            ClientId = 1,
            SoftwareSystemId = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(3),
            ExtraSupportYears = 10
        };
        
        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync(new Individual { Id = request.ClientId });
        _softwareRepositoryMock.Setup(c => c.GetSoftwareSystemByIdAsync(request.SoftwareSystemId))
            .ReturnsAsync(new SoftwareSystem { Id = request.SoftwareSystemId });

        Assert.ThrowsAsync<InvalidSupportYearsNumberException>(() => _service.CreateUpfrontContractAsync(request));
    }
    
    [Test]
    public async Task CreateUpfrontContractAsync_ValidRequest_AddsContract()
    {
        var request = new CreateUpfrontContractRequest
        {
            ClientId = 1,
            SoftwareSystemId = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(10),
            ExtraSupportYears = 2
        };

        _clientRepositoryMock.Setup(c => c.GetClientByIdAsync(request.ClientId))
            .ReturnsAsync(new Individual { Id = request.ClientId });
        _softwareRepositoryMock.Setup(s => s.GetSoftwareSystemByIdAsync(request.SoftwareSystemId))
            .ReturnsAsync(new SoftwareSystem { Id = request.SoftwareSystemId, OneYearLicensePrice = 1000 });
        _contractRepositoryMock.Setup(c => c.HasActiveContractsAsync(request.ClientId, request.SoftwareSystemId))
            .ReturnsAsync(false);
        _discountRepositoryMock.Setup(d => d.GetActiveDiscountsAsync(request.SoftwareSystemId, request.StartDate))
            .ReturnsAsync(new List<Discount>
            {
                new Discount { Percentage = 10 }
            });
        _clientRepositoryMock.Setup(c => c.IsLoyalClientAsync(request.ClientId))
            .ReturnsAsync(true);

        Contract? capturedContract = null;
        _contractRepositoryMock.Setup(c => c.AddAsync(It.IsAny<Contract>()))
            .Callback<Contract>(c => capturedContract = c)
            .Returns(Task.CompletedTask);

        await _service.CreateUpfrontContractAsync(request);

        _contractRepositoryMock.Verify(c => c.AddAsync(It.IsAny<Contract>()), Times.Once);
        Assert.That(capturedContract, Is.Not.Null);
        decimal expectedPrice = 2855m;
        Assert.That(capturedContract.Price, Is.EqualTo(expectedPrice));
    }

    [Test]
    public void IssuePaymentAsync_ContractIsCancelled_ThrowsContractIsCancelledException()
    {
        var contract = new Contract { Id = 1, IsCancelled = true };
        _contractRepositoryMock.Setup(r => r.GetContractWithPaymentByContractIdAsync(1))
            .ReturnsAsync(contract);

        Assert.ThrowsAsync<ContractIsCancelledException>(() => _service.IssuePaymentAsync(1, 100));
    }
    
    [Test]
    public void IssuePaymentAsync_ContractIsPaid_ThrowsContractIsFullyPaidException()
    {
        var contract = new Contract { Id = 1, IsCancelled = true };
        _contractRepositoryMock.Setup(r => r.GetContractWithPaymentByContractIdAsync(1))
            .ReturnsAsync(contract);

        Assert.ThrowsAsync<ContractIsCancelledException>(() => _service.IssuePaymentAsync(1, 100));
    }
    
    [Test]
    public void IssuePaymentAsync_ContractExpired_ThrowsContractHasExpiredException()
    {
        var contract = new Contract
        {
            Id = 1,
            EndDate = DateTime.UtcNow.AddDays(-1),
            Payments = new List<Payment>(),
            Price = 100
        };
        _contractRepositoryMock.Setup(r => r.GetContractWithPaymentByContractIdAsync(1))
            .ReturnsAsync(contract);

        Assert.ThrowsAsync<ContractHasExpiredException>(() => _service.IssuePaymentAsync(1, 50));
    }

    [Test]
    public void IssuePaymentAsync_InvalidAmount_ThrowsInvalidPaymentAmountException()
    {
        var contract = new Contract
        {
            Id = 1,
            EndDate = DateTime.UtcNow.AddDays(1),
            Payments = new List<Payment>(),
            Price = 100
        };
        _contractRepositoryMock.Setup(r => r.GetContractWithPaymentByContractIdAsync(1))
            .ReturnsAsync(contract);

        Assert.ThrowsAsync<InvalidPaymentAmountException>(() => _service.IssuePaymentAsync(1, 0));
        Assert.ThrowsAsync<InvalidPaymentAmountException>(() => _service.IssuePaymentAsync(1, -50));
    }
}