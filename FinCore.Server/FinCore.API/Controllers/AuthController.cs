using FinCore.Domain.Models.Clients.Clients.DTO;
using FinCore.Services.AuthsService;
using Microsoft.AspNetCore.Mvc;

namespace FinCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginClientDTO loginDto, CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginAsync(loginDto, cancellationToken);

            if (result)
            {
                var token = await _authService.GenerateJwtToken(loginDto.PhoneNumber);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized("Неверные логин или пароль."); // Возвращаем 401 Unauthorized
            }
        }
    }
}