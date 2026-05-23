using IndustryPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndustryPulse.Infrastructure.Data.Mappings;

public class OrdemProducaoMapping : IEntityTypeConfiguration<OrdemProducao>
{
    public void Configure(EntityTypeBuilder<OrdemProducao> builder)
    {
        builder.ToTable("ordens_producao");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Numero)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(x => x.QuantidadePlanejada)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(x => x.QuantidadeProduzida)
            .HasPrecision(10, 2);

        builder.Property(x => x.Observacao)
            .HasMaxLength(500);

        builder.HasOne(x => x.Produto)
            .WithMany(x => x.Ordens)
            .HasForeignKey(x => x.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LinhaProducao)
            .WithMany(x => x.Ordens)
            .HasForeignKey(x => x.LinhaProducaoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Paradas)
            .WithOne(x => x.OrdemProducao)
            .HasForeignKey(x => x.OrdemProducaoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}