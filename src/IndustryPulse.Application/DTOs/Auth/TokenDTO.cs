namespace IndustryPulse.Application.DTOs.Auth;

public record TokenDTO(
    string Token,
    string RefreshToken,
    string NomeUsuario,
    string Perfil
);