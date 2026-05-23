namespace IndustryPulse.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> BuscarTodosAsync();
    Task<T?> BuscarPorIdAsync(int id);
    Task<T> CriarAsync(T entidade);
    Task<T> AtualizarAsync(T entidade);
    Task DeletarAsync(T entidade);
}