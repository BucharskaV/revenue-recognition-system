using RevenueRecognitionSystem.Domain.Entities;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Services.Contracts.Requests;
using RevenueRecognitionSystem.Services.Contracts.Responses;
using RevenueRecognitionSystem.Services.Interfaces;
using RevenueRecognitionSystem.Services.Mappers;

namespace RevenueRecognitionSystem.Services.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Client?> GetClientAsync(int id)
    {
        var client = await _clientRepository.GetClientByIdAsync(id);
        if (client is null)
        {
            throw new ClientNotFoundException(id);
        }
        return client;
    }

    public async Task<IEnumerable<GetAllClientsResponse>> GetAllClientsAsync()
    {
        var clients = await _clientRepository.GetClientsAsync();
        return clients.Select(ClientMapper.ToResponse);
    }

    public async Task AddIndividualAsync(AddIndividualRequest request)
    {
        if (await _clientRepository.GetIndividualByPeselAsync(request.PESEL) is not null)
            throw new IndividualAlreadyExistsException(request.PESEL); 
        var client = ClientMapper.ToEntity(request);
        await _clientRepository.AddAsync(client);
    }

    public async Task AddCompanyAsync(AddCompanyRequest request)
    {
        if (await _clientRepository.GetCompanyByKrsNumberAsync(request.KRSNumber) is not null)
            throw new CompanyAlreadyExistsException(request.KRSNumber); 
        var client = ClientMapper.ToEntity(request);
        await _clientRepository.AddAsync(client);
    }

    public async Task UpdateIndividualAsync(int id, UpdateIndividualRequest request)
    {
        var individual = await _clientRepository.GetIndividualByIdAsync(id);
        if (individual is null || individual.IsDeleted)
        {
            throw new ClientNotFoundException(id);
        }
        individual.Address = request.Address;
        individual.Email = request.Email;
        individual.PhoneNumber = request.PhoneNumber;
        individual.FirstName = request.FirstName;
        individual.LastName = request.LastName;
        await _clientRepository.UpdateAsync(individual);
    }

    public async Task UpdateCompanyAsync(int id, UpdateCompanyRequest request)
    {
        var company = await _clientRepository.GetCompanyByIdAsync(id);
        if (company is null)
        {
            throw new ClientNotFoundException(id);
        }
        company.Address = request.Address;
        company.Email = request.Email;
        company.PhoneNumber = request.PhoneNumber;
        company.CompanyName = request.CompanyName;
        await _clientRepository.UpdateAsync(company);
    }

    public async Task DeleteClientAsync(int id)
    {
        var client = await _clientRepository.GetClientByIdAsync(id);
        if (client is null)
        {
            throw new ClientNotFoundException(id);
        }
        if (client is Company)
        {
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
    }
}