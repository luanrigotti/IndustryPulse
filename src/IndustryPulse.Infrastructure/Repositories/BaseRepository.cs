using IndustryPulse.Domain.Interfaces.Repositories;
using IndustryPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IndustryPulse.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> BuscarTodosAsync()
        => await _dbSet.ToListAsync();

    public async Task<T?> BuscarPorIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<T> CriarAsync(T entidade)
    {
        await _dbSet.AddAsync(entidade);
        await _context.SaveChangesAsync();
        return entidade;
    }

    public async Task<T> AtualizarAsync(T entidade)
    {
        _dbSet.Update(entidade);
        await _context.SaveChangesAsync();
        return entidade;
    }

    public async Task DeletarAsync(T entidade)
    {
        _dbSet.Remove(entidade);
        await _context.SaveChangesAsync();
    }
}