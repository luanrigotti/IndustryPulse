using IndustryPulse.Application.Services;
using IndustryPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndustryPulse.Infrastructure.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Usuarios.AnyAsync())
            return;

        var usuarios = new List<Usuario>
        {
            new()
            {
                Nome = "Administrador",
                Email = "admin@industrypulse.com",
                SenhaHash = AuthService.GerarHash("Admin@123"),
                Perfil = "Gestor",
                Ativo = true
            }
        };

        var linhas = new List<LinhaProducao>
        {
            new() { Nome = "Linha A", Descricao = "Linha de produção A", CapacidadeHora = 100 },
            new() { Nome = "Linha B", Descricao = "Linha de produção B", CapacidadeHora = 80 },
            new() { Nome = "Linha C", Descricao = "Linha de produção C", CapacidadeHora = 120 }
        };

        var produtos = new List<Produto>
        {
            new() { Codigo = "PROD-001", Descricao = "Produto A", UnidadeMedida = "UN", TempoProducaoMinutos = 5 },
            new() { Codigo = "PROD-002", Descricao = "Produto B", UnidadeMedida = "UN", TempoProducaoMinutos = 8 },
            new() { Codigo = "PROD-003", Descricao = "Produto C", UnidadeMedida = "KG", TempoProducaoMinutos = 3 }
        };

        await context.Usuarios.AddRangeAsync(usuarios);
        await context.LinhasProducao.AddRangeAsync(linhas);
        await context.Produtos.AddRangeAsync(produtos);
        await context.SaveChangesAsync();
    }
}