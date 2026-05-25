using IndustryPulse.Domain.Entities;
using IndustryPulse.Domain.Interfaces.Repositories;
using IndustryPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IndustryPulse.Infrastructure.Repositories;

public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context) { }

    public async Task<Produto?> BuscarPorCodigoAsync(string codigo)
        => await _context.Produtos
            .FirstOrDefaultAsync(p => p.Codigo == codigo);

    public async Task<IEnumerable<Produto>> BuscarAtivosAsync()
        => await _context.Produtos
            .Where(p => p.Ativo)
            .ToListAsync();
}