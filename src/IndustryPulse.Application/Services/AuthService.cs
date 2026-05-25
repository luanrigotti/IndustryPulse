using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IndustryPulse.Application.DTOs.Auth;
using IndustryPulse.Application.Services.Interfaces;
using IndustryPulse.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IndustryPulse.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUsuarioRepository repository,
        IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<TokenDTO> LoginAsync(LoginDTO dto)
    {
        var usuario = await _repository.BuscarPorEmailAsync(dto.Email)
            ?? throw new KeyNotFoundException("Usuário não encontrado");

        var senhaHash = GerarHash(dto.Senha);
        if (usuario.SenhaHash != senhaHash)
            throw new InvalidOperationException("Senha incorreta");

        var token = GerarToken(usuario.Id, usuario.Email, usuario.Perfil);

        return new TokenDTO(token, string.Empty, usuario.Nome, usuario.Perfil);
    }

    private string GerarToken(int id, string email, string perfil)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, perfil)
        };

        var expiracao = int.Parse(_configuration["Jwt:ExpiracaoHoras"]!);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiracao),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GerarHash(string senha)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(senha);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}