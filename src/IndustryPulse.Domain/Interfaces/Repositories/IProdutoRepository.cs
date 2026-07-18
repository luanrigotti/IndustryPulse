using IndustryPulse.Domain.Entities;

namespace IndustryPulse.Domain.Interfaces.Repositories;

public interface IProdutoRepository : IBaseRepository<Produto>
{
    Task<Produto?> BuscarPorCodigoAsync(string codigo);
    Task<IEnumerable<Produto>> BuscarAtivosAsync();
    Task<int> ContarAsync();
}