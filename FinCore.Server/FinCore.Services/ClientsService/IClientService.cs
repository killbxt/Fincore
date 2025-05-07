using FinCore.Domain.Models.Clients.Clients.DTO;
using FinCore.Domain.Models.Clients;
using System.Linq.Expressions;

namespace FinCore.Services.ClientsService
{
    public interface IClientService
    {
        Task<bool> CreateClientAsync(AddClientDTO clientDto, CancellationToken cancellationToken = default); 
        Task<ClientDTO?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ClientDTO>> GetAllClientsAsync(CancellationToken cancellationToken = default);
        Task<List<ClientDTO>> FindClientsAsync(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken = default);
        Task UpdateClientAsync(Guid id, UpdateClientDTO clientDto, CancellationToken cancellationToken = default); 
        Task DeleteClientByIdAsync(Guid id, CancellationToken cancellationToken = default);
        

        Task<ClientDTO?> GetClientByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<ClientDTO?> GetClientByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<List<ClientDTO>> GetClientsWithBirthdayTodayAsync(CancellationToken cancellationToken = default);
        Task<List<ClientDTO>> GetClientsWithoutCardsAsync(CancellationToken cancellationToken = default);
        Task<string> GetClientPasswordForVerificationAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
