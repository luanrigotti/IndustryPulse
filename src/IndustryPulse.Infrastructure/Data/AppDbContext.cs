using IndustryPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndustryPulse.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<OrdemProducao> OrdensProducao { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<LinhaProducao> LinhasProducao { get; set; }
    public DbSet<ParadaProducao> ParadasProducao { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}