using FinCore.Domain.Models.Clients.Clients.DTO;

namespace FinCore.Services.AuthsService
{
    public interface IAuthService
    {
        Task<ClientDTO> RegisterAsync(RegisterClientDTO registerDto, CancellationToken cancellationToken = default);
        Task<bool> LoginAsync(LoginClientDTO loginDto, CancellationToken cancellationToken = default);
        Task LogoutAsync(CancellationToken cancellationToken = default);
        Task<string> GenerateJwtToken(string mobilephone);
        Task<string> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
