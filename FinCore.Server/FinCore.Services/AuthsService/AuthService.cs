using FinCore.Data.Repositories.ClientsRepository;
using FinCore.Domain.Models.Clients.Clients.DTO;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinCore.Services.ClientsService;

namespace FinCore.Services.AuthsService
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;
        private readonly IConfiguration _configuration;

        public AuthService(IClientService clientService, IConfiguration configuration)
        {
            _clientService = clientService;
            _configuration = configuration;
        }

        public async Task<bool> LoginAsync(LoginClientDTO loginDto, CancellationToken cancellationToken = default)
        {
            var foundedclient = await _clientService.GetClientByPhoneNumberAsync(loginDto.PhoneNumber);
            if(foundedclient!= null)
            {
                if (loginDto.Password == await _clientService.GetClientPasswordForVerificationAsync(foundedclient.Id))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<string> GenerateJwtToken(string mobilephone)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim(ClaimTypes.MobilePhone, mobilephone),
        };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ClientDTO> RegisterAsync(RegisterClientDTO registerDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
