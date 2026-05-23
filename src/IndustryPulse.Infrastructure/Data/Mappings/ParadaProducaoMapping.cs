using IndustryPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndustryPulse.Infrastructure.Data.Mappings;

public class ParadaProducaoMapping : IEntityTypeConfiguration<ParadaProducao>
{
    public void Configure(EntityTypeBuilder<ParadaProducao> builder)
    {
        builder.ToTable("paradas_producao");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Motivo)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(x => x.Descricao)
            .HasMaxLength(500);
    }
}