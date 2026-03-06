using AutoMapper;
using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;
using RevenueRecognitionSystem.Services.Mappers;
using Microsoft.Extensions.Logging;

namespace RevenueRecognitionSystem.Services.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<ClientService> _logger;
    private readonly IMapper _mapper;

    public ClientService(IClientRepository clientRepository, ILogger<ClientService> logger, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Client?> GetClientAsync(int id)
    {
        var client = await _clientRepository.GetClientByIdAsync(id);
        if (client is null)
        {
            _logger.LogWarning("Client with ID {Id} not found", id);
            throw new ClientNotFoundException(id);
        }
        _logger.LogInformation("Client with ID {Id} found: {ClientType}", id, client.GetType().Name);
        return client;
    }

    public async Task<IEnumerable<GetAllClientsResponse>> GetAllClientsAsync()
    {
        var clients = await _clientRepository.GetClientsAsync();
        _logger.LogInformation("{Count} clients fetched", clients.Count());
        return _mapper.Map<IEnumerable<GetAllClientsResponse>>(clients);
    }

    public async Task AddIndividualAsync(AddIndividualRequest request)
    {
        if (await _clientRepository.GetIndividualByPeselAsync(request.PESEL) is not null)
        {
            _logger.LogWarning("Individual with PESEL {PESEL} already exists", request.PESEL);
            throw new IndividualAlreadyExistsException(request.PESEL);
        }
        var client =  _mapper.Map<Individual>(request);
        await _clientRepository.AddAsync(client);
        _logger.LogWarning("Individual with PESEL {PESEL} already exists", request.PESEL);
    }

    public async Task AddCompanyAsync(AddCompanyRequest request)
    {
        if (await _clientRepository.GetCompanyByKrsNumberAsync(request.KRSNumber) is not null)
        {
            _logger.LogWarning("Company with KRS {KRSNumber} already exists", request.KRSNumber);
            throw new CompanyAlreadyExistsException(request.KRSNumber);
        }
        var client = _mapper.Map<Company>(request);
        await _clientRepository.AddAsync(client);
        _logger.LogWarning("Company with KRS {KRSNumber} already exists", request.KRSNumber);
    }

    public async Task UpdateIndividualAsync(int id, UpdateIndividualRequest request)
    {
        var individual = await _clientRepository.GetIndividualByIdAsync(id);
        if (individual is null || individual.IsDeleted)
        {
            _logger.LogWarning("Individual client with ID {Id} not found or is deleted", id);
            throw new ClientNotFoundException(id);
        }
        individual.Address = request.Address;
        individual.Email = request.Email;
        individual.PhoneNumber = request.PhoneNumber;
        individual.FirstName = request.FirstName;
        individual.LastName = request.LastName;
        await _clientRepository.UpdateAsync(individual);
        _logger.LogInformation("Individual client with ID {Id} updated successfully", id);
    }

    public async Task UpdateCompanyAsync(int id, UpdateCompanyRequest request)
    {
        var company = await _clientRepository.GetCompanyByIdAsync(id);
        if (company is null)
        {
            _logger.LogInformation("Individual client with ID {Id} updated successfully", id);
            throw new ClientNotFoundException(id);
        }
        company.Address = request.Address;
        company.Email = request.Email;
        company.PhoneNumber = request.PhoneNumber;
        company.CompanyName = request.CompanyName;
        await _clientRepository.UpdateAsync(company);
        _logger.LogInformation("Company with ID {Id} updated successfully", id);
    }

    public async Task DeleteClientAsync(int id)
    {
        var client = await _clientRepository.GetClientByIdAsync(id);
        if (client is null)
        {
            _logger.LogWarning("Client with ID {Id} not found", id);
            throw new ClientNotFoundException(id);
        }
        if (client is Company)
        {
            _logger.LogWarning("Cannot delete company with ID {Id}", id);
            throw new CompanyCantBeRemovedException();
        }

        if (client is Individual individual)
        {
            individual.Address = "DELETED";
            individual.Email = "DELETED";
            individual.PhoneNumber = "DELETED";
            individual.FirstName = "DELETED";
            individual.LastName = "DELETED";
            individual.PESEL = "DELETED";
            individual.IsDeleted = true;
            await _clientRepository.UpdateAsync(individual);
        }
        _logger.LogInformation("Individual client with ID {Id} marked as deleted", id);
    }
}