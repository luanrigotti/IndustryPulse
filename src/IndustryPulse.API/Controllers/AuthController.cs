using IndustryPulse.Application.DTOs.Auth;
using IndustryPulse.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IndustryPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var token = await _service.LoginAsync(dto);
        return Ok(token);
    }
}