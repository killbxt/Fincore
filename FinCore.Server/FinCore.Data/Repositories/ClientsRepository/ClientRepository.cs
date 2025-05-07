using FinCore.Data.Context;
using FinCore.Domain.Models.Clients;
using FinCore.Domain.Models.Clients.Clients.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FinCore.Data.Repositories.ClientsRepository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ILogger<ClientRepository> _logger;
        private readonly FinCoreDbContext _finCoreDbContext;

        public ClientRepository(ILogger<ClientRepository> logger, FinCoreDbContext finCoreDbContext)
        {
            _logger = logger;
            _finCoreDbContext = finCoreDbContext;
        }

        public async Task<bool> AddAsync(AddClientDTO clientdto, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var client = (Client)clientdto;
                await _finCoreDbContext.AddAsync(client);
                await _finCoreDbContext.SaveChangesAsync();
                _logger.LogInformation("Клиент добавлен");
                return true;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Добавление клиента отменено."); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в ClientRepository: {ex.Message}");
            }
            return false;
        }

        public async Task DeleteAsync(Client client, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _finCoreDbContext.Remove(client);
                await _finCoreDbContext.SaveChangesAsync();
                _logger.LogInformation("Клиент удален");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Удаление клиента отменено.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в ClientRepository: {ex.Message}");
            }
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _finCoreDbContext.Clients.FindAsync(id);
                _finCoreDbContext.Clients.Remove(client);
                await _finCoreDbContext.SaveChangesAsync();
                _logger.LogInformation("Клиент удален");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Удаление клиента отменено.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в ClientRepository: {ex.Message}");
            }
        }

        public async Task<List<ClientDTO>> FindAsync(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                    var clients = await _finCoreDbContext.Clients
                    .Where(predicate)
                    .AsNoTracking() 
                    .ToListAsync(cancellationToken);
                     return clients.Select(c => (ClientDTO)c).ToList();

            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Поиск клиентов отменен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске клиентов.");
            }
            return new List<ClientDTO>();
        }

        public async Task<List<ClientDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var clients = await _finCoreDbContext.Clients
                .AsNoTracking()
                .ToListAsync(cancellationToken);
                return clients.Select(c => (ClientDTO)c).ToList();

            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Поиск клиентов отменен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске клиентов.");
            }
            return null;
        }

        public async Task<ClientDTO?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _finCoreDbContext.Clients
                .AsNoTracking()
                .Where(c => c.Email == email)
                .FirstOrDefaultAsync(cancellationToken);
                return (ClientDTO?)client;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Поиск клиентов отменен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске клиента.");
            }
            return null;
        }

        public async Task<ClientDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _finCoreDbContext.Clients
                .AsNoTracking()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
                return (ClientDTO?)client;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Поиск клиентов отменен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске клиента");
            }
            return null;
        }

        public async Task<ClientDTO?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _finCoreDbContext.Clients
                .Where(c => c.PhoneNumber == phoneNumber)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
                return (ClientDTO?)client;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Поиск клиентов отменен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске клиента");
            }
            return null;
        }

        public async Task<List<ClientDTO>> GetClientsWithBirthdayTodayAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var clients = await _finCoreDbContext.Clients
                .Where(c => c.DateOfBirth == DateOnly.FromDateTime(DateTime.Now))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
                return clients.Select(c => (ClientDTO)c).ToList();

            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Поиск клиентов отменен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске клиентов.");
            }
            return null;
        }

        public async Task<List<ClientDTO>> GetClientsWithoutCardsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var clients = await _finCoreDbContext.Clients
                .Where(c => c.Cards.Count == 0)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
                return clients.Select(c => (ClientDTO)c).ToList();

            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Поиск клиентов отменен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске клиентов.");
            }
            return null;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _finCoreDbContext.Clients
                    .AnyAsync(c => c.Email == email, cancellationToken); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке уникальности email.");
                return false; 
            }
        }

        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _finCoreDbContext.Clients
                    .AnyAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке уникальности номера.");
                return false;
            }
        }

        public async Task UpdateAsync(Guid Id, UpdateClientDTO clientDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _finCoreDbContext.Clients.FindAsync(Id, cancellationToken);
               
                client.Email = clientDto.Email ?? client.Email;  
                client.Name = clientDto.Name ?? client.Name;
                client.Surname = clientDto.Surname ?? client.Surname;
                client.Patronymic = clientDto.Patronymic ?? client.Patronymic;
                client.PhoneNumber = clientDto.PhoneNumber ?? client.PhoneNumber;
                client.DateOfBirth = clientDto.DateOfBirth ?? client.DateOfBirth;

                _finCoreDbContext.Update(client);

                await _finCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating client.");
            }
        }

        public async Task<string> GetPasswordForVerificationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _finCoreDbContext.Clients
                .AsNoTracking()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
                return client.Password;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Поиск клиентов отменен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске клиента");
            }
            return null;
        }
    }
}
