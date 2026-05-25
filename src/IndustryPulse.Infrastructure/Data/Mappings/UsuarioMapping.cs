using IndustryPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndustryPulse.Infrastructure.Data.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.SenhaHash)
            .IsRequired();

        builder.Property(x => x.Perfil)
            .IsRequired()
            .HasMaxLength(20);
    }
}