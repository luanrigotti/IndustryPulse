using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Interfaces.Repositories;
using IndustryPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IndustryPulse.Infrastructure.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) { }

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
        => await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email && u.Ativo);
}