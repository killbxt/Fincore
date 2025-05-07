using FinCore.Data.Repositories.ClientsRepository;
using FinCore.Domain.Models.Clients;
using FinCore.Domain.Models.Clients.Clients.DTO;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FinCore.Services.ClientsService
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientService> _logger;

        public ClientService(IClientRepository clientRepository, ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<bool> CreateClientAsync(AddClientDTO clientDto, CancellationToken cancellationToken = default)
        {
            if(!(await _clientRepository.IsEmailUniqueAsync(clientDto.Email)) && !(await _clientRepository.IsPhoneNumberUniqueAsync(clientDto.PhoneNumber)))
            {
                var result = await _clientRepository.AddAsync(clientDto, cancellationToken);
                if (result)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task DeleteClientByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _clientRepository.DeleteByIdAsync(id, cancellationToken);
        }

        public async Task<List<ClientDTO>> FindClientsAsync(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _clientRepository.FindAsync(predicate, cancellationToken);
        }

        public async Task<List<ClientDTO>> GetAllClientsAsync(CancellationToken cancellationToken = default)
        {
            return await _clientRepository.GetAllAsync(cancellationToken);
        }

        public async Task<ClientDTO?> GetClientByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _clientRepository.GetByEmailAsync(email, cancellationToken);
        }

        public async Task<ClientDTO?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _clientRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<ClientDTO?> GetClientByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _clientRepository.GetByPhoneNumberAsync(phoneNumber, cancellationToken);
        }

        public async Task<string> GetClientPasswordForVerificationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _clientRepository.GetPasswordForVerificationAsync(id, cancellationToken);
        }

        public async Task<List<ClientDTO>> GetClientsWithBirthdayTodayAsync(CancellationToken cancellationToken = default)
        {
            return await _clientRepository.GetClientsWithBirthdayTodayAsync(cancellationToken);
        }

        public async Task<List<ClientDTO>> GetClientsWithoutCardsAsync(CancellationToken cancellationToken = default)
        {
            return await _clientRepository.GetClientsWithoutCardsAsync(cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _clientRepository.IsEmailUniqueAsync(email, cancellationToken);
        }

        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _clientRepository.IsPhoneNumberUniqueAsync(phoneNumber, cancellationToken);
        }

        public async Task UpdateClientAsync(Guid id, UpdateClientDTO clientDto, CancellationToken cancellationToken = default)
        {
            await _clientRepository.UpdateAsync(id, clientDto, cancellationToken);
        }
    }
}
