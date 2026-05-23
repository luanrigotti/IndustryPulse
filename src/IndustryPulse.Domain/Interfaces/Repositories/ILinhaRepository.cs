using IndustryPulse.Domain.Entities;

namespace IndustryPulse.Domain.Interfaces.Repositories;

public interface ILinhaRepository : IBaseRepository<LinhaProducao>
{
    Task<IEnumerable<LinhaProducao>> BuscarAtivasAsync();
}