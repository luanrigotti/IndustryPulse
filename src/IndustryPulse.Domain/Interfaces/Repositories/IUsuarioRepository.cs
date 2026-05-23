using IndustryPulse.Domain.Entities;

namespace IndustryPulse.Domain.Interfaces.Repositories;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario?> BuscarPorEmailAsync(string email);
}