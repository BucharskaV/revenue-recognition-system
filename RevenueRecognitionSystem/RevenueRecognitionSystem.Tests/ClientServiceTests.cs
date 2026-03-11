using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Implementations;

namespace RevenueRecognitionSystem.Tests;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly Mock<ILogger<ClientService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ClientService _service;

    public ClientServiceTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _loggerMock = new Mock<ILogger<ClientService>>();
        _mapperMock = new Mock<IMapper>();
        _service = new ClientService(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Test]
    public async Task GetClientAsync_ClientExists_ReturnsClientResponse()
    {
        var client = new Individual{Id = 1, Address = "Kalynova 1, Kyiv" };
        var response = new GetClientResponse(){Id = 1, Address = "Kalynova 1, Kyiv" };
        
        _repositoryMock
            .Setup(r => r.GetClientByIdAsync(1))
            .ReturnsAsync(client);
        _mapperMock
            .Setup(m => m.Map<GetClientResponse>(client))
            .Returns(response);
        
        var result = await _service.GetClientAsync(1);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Address, Is.EqualTo(client.Address));
    }
    
    [Test]
    public void GetClientAsync_ClientNotExists_ThrowsClientNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Client?)null);

        Assert.ThrowsAsync<ClientNotFoundException>(() =>
            _service.GetClientAsync(1));
    }

    [Test]
    public async Task AddIndividualAsync_WhenPeselIsUnique_AddsClient()
    {
        var request = new AddIndividualRequest{PESEL = "11122233345"};
        var individual = new Individual();
        
        _repositoryMock
            .Setup(r => r.GetIndividualByPeselAsync(request.PESEL))
            .ReturnsAsync((Individual?)null);
        _mapperMock
            .Setup(m => m.Map<Individual>(request))
            .Returns(individual);
        
        await _service.AddIndividualAsync(request);
        _repositoryMock.Verify(r => r.AddAsync(individual), Times.Once);
    }
    
    [Test]
    public void AddIndividualAsync_WhenPeselIsDublicate_ThrowsIndividualAlreadyExistsException()
    {
        var request = new AddIndividualRequest{PESEL = "11122233345"};
        _repositoryMock
            .Setup(r => r.GetIndividualByPeselAsync(request.PESEL))
            .ReturnsAsync(new Individual());

        Assert.ThrowsAsync<IndividualAlreadyExistsException>(() =>
            _service.AddIndividualAsync(request));
    }
    
    [Test]
    public async Task AddCompanyAsync_WhenKRSNumberIsUnique_AddsClient()
    {
        var request = new AddCompanyRequest(){KRSNumber = "111222333"};
        var company = new Company();
        
        _repositoryMock
            .Setup(r => r.GetCompanyByKrsNumberAsync(request.KRSNumber))
            .ReturnsAsync((Company?)null);
        _mapperMock
            .Setup(m => m.Map<Company>(request))
            .Returns(company);
        
        await _service.AddCompanyAsync(request);
        _repositoryMock.Verify(r => r.AddAsync(company), Times.Once);
    }
    
    [Test]
    public void AddCompanyAsync_WhenKRSNumberIsDublicate_ThrowsCompanyAlreadyExistsException()
    {
        var request = new AddCompanyRequest(){KRSNumber = "111222333"};
        _repositoryMock
            .Setup(r => r.GetCompanyByKrsNumberAsync(request.KRSNumber))
            .ReturnsAsync(new Company());

        Assert.ThrowsAsync<CompanyAlreadyExistsException>(() =>
            _service.AddCompanyAsync(request));
    }

    [Test]
    public async Task DeleteClientAsync_WhenIndividualExists_SoftDeleteIndividual()
    {
        var individual = new Individual
        {
            Id = 1,
            Address = "A",
            Email = "B",
            PhoneNumber = "C",
            FirstName = "D",
            LastName = "E",
            PESEL = "F"
        };
        _repositoryMock
            .Setup(r => r.GetClientByIdAsync(1))
            .ReturnsAsync(individual);
        await _service.DeleteClientAsync(1);

        Assert.That(individual.IsDeleted, Is.True);
        Assert.That(individual.Address, Is.EqualTo("DELETED"));
        _repositoryMock.Verify(r => r.UpdateAsync(individual), Times.Once);
    }
    
    [Test]
    public void DeleteClientAsync_WhenClientISCompany_ThrowsCompanyCantBeRemovedException()
    {
        var company = new Company();

        _repositoryMock
            .Setup(r => r.GetClientByIdAsync(1))
            .ReturnsAsync(company);

        Assert.ThrowsAsync<CompanyCantBeRemovedException>(() =>
            _service.DeleteClientAsync(1));
    }
}