using FinCore.Domain.Models.Clients;
using FinCore.Domain.Models.Clients.Clients.DTO;
using System.Linq.Expressions;

namespace FinCore.Data.Repositories.ClientsRepository
{
    public interface IClientRepository
    {
        //CRUD - Базовые операции 

        Task<ClientDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ClientDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<ClientDTO>> FindAsync(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(AddClientDTO client , CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id,UpdateClientDTO client, CancellationToken cancellationToken = default);
        Task DeleteAsync(Client client, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // Специализированные методы

        Task<ClientDTO?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<ClientDTO?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default); 
        Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, CancellationToken cancellationToken = default);
        Task<List<ClientDTO>> GetClientsWithBirthdayTodayAsync(CancellationToken cancellationToken = default);
        Task<List<ClientDTO>> GetClientsWithoutCardsAsync(CancellationToken cancellationToken = default);
        Task<string> GetPasswordForVerificationAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
