using IndustryPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndustryPulse.Infrastructure.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("produtos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.Codigo)
            .IsUnique();

        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.UnidadeMedida)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.TempoProducaoMinutos)
            .HasPrecision(10, 2);
    }
}