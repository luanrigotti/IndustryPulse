using IndustryPulse.Application.DTOs.Auth;

namespace IndustryPulse.Application.Services.Interfaces;

public interface IAuthService
{
    Task<TokenDTO> LoginAsync(LoginDTO dto);
}