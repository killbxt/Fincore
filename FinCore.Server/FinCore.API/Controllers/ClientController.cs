using FinCore.Domain.Models.Clients.Clients.DTO;
using FinCore.Domain.Models.Clients;
using FinCore.Services.ClientsService;  
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace FinCore.API.Controllers
{
    [ApiController]
    [Route("Fincore.API/[controller]")]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientAsync([FromBody] AddClientDTO clientDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _clientService.CreateClientAsync(clientDto, cancellationToken);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return ValidationProblem();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании клиента");
                return BadRequest("Произошла ошибка при создании клиента.");
            }
        }

            [HttpGet("{id}")]
        public async Task<IActionResult> GetClientByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _clientService.GetClientByIdAsync(id, cancellationToken);
                if (client == null)
                {
                    return NotFound(); 
                }
                return Ok(client); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении клиента по ID {id}", id);
                return BadRequest("Произошла ошибка при получении клиента."); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var clients = await _clientService.GetAllClientsAsync(cancellationToken);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                
               _logger.LogError(ex, "Ошибка при получении всех клиентов");
                return BadRequest("Произошла ошибка при получении списка клиентов."); 
            }
        }

        [HttpPost("find")]
        public async Task<IActionResult> FindClientsAsync([FromBody] Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                if (predicate == null)
                {
                    return BadRequest("Predicate cannot be null"); 
                }
                var clients = await _clientService.FindClientsAsync(predicate, cancellationToken);
                return Ok(clients); 
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при поиске клиентов."); 
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientAsync(Guid id, [FromBody] UpdateClientDTO clientDto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (clientDto == null)
                {
                    return BadRequest("Client DTO cannot be null"); 
                }
                await _clientService.UpdateClientAsync(id, clientDto, cancellationToken);
                return Ok(); 
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при обновлении клиента."); 
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _clientService.DeleteClientByIdAsync(id, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при удалении клиента."); 
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetClientByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _clientService.GetClientByEmailAsync(email, cancellationToken);
                if (client == null)
                {
                    return NotFound(); 
                }
                return Ok(client); 
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении клиента по email."); 
            }
        }

        [HttpGet("phone/{phoneNumber}")]
        public async Task<IActionResult> GetClientByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _clientService.GetClientByPhoneNumberAsync(phoneNumber, cancellationToken);
                if (client == null)
                {
                    return NotFound(); 
                }
                return Ok(client); 
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении клиента по номеру телефона.");
            }
        }

        [HttpGet("is-email-unique/{email}")]
        public async Task<IActionResult> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                var isUnique = await _clientService.IsEmailUniqueAsync(email, cancellationToken);
                return Ok(isUnique);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при проверке уникальности email.");
            }
        }

        [HttpGet("is-phone-unique/{phoneNumber}")]
        public async Task<IActionResult> IsPhoneNumberUniqueAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                var isUnique = await _clientService.IsPhoneNumberUniqueAsync(phoneNumber, cancellationToken);
                return Ok(isUnique); 
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при проверке уникальности номера телефона."); 
            }
        }

        [HttpGet("birthday-today")]
        public async Task<IActionResult> GetClientsWithBirthdayTodayAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var clients = await _clientService.GetClientsWithBirthdayTodayAsync(cancellationToken);
                return Ok(clients);
            }
            catch (Exception ex)
            {   
                return BadRequest("Произошла ошибка при получении клиентов с днем рождения сегодня.");
            }
        }

        [HttpGet("without-cards")]
        public async Task<IActionResult> GetClientsWithoutCardsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var clients = await _clientService.GetClientsWithoutCardsAsync(cancellationToken);
                return Ok(clients); 
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении клиентов без карт.");
            }
        }

        [HttpGet("{id}/password")]
        public async Task<IActionResult> GetClientPasswordForVerificationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var password = await _clientService.GetClientPasswordForVerificationAsync(id, cancellationToken);
                return Ok(password); 
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении пароля.");
            }
        }
    }

}
