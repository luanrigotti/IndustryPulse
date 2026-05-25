using IndustryPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndustryPulse.Infrastructure.Data.Mappings;

public class LinhaProducaoMapping : IEntityTypeConfiguration<LinhaProducao>
{
    public void Configure(EntityTypeBuilder<LinhaProducao> builder)
    {
        builder.ToTable("linhas_producao");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Descricao)
            .HasMaxLength(500);

        builder.Property(x => x.CapacidadeHora)
            .IsRequired()
            .HasPrecision(10, 2);
    }
}