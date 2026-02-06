using core.application.DTOs.Users;
using core.application.Wrappers;

namespace core.application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponseDto>> AuthenticateAsync(AuthenticationRequestDto request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequestDto request, string origin);
    }
}
