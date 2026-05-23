using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Enums;

namespace IndustryPulse.Domain.Interfaces.Repositories;

public interface IOrdemRepository : IBaseRepository<OrdemProducao>
{
    Task<IEnumerable<OrdemProducao>> BuscarPorStatusAsync(StatusOrdem status);
    Task<IEnumerable<OrdemProducao>> BuscarPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<IEnumerable<OrdemProducao>> BuscarPorLinhaAsync(int linhaId);
    Task<int> ContarPorAnoAsync(int ano);
}