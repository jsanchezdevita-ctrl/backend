using Domain.Parametros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Parametros;

internal sealed class ParametrosConfiguration : IEntityTypeConfiguration<Parametro>
{
    public void Configure(EntityTypeBuilder<Parametro> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Llave)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Valor)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(p => p.Descripcion)
            .HasMaxLength(500);

        builder.Property(p => p.FechaActualizacion)
            .IsRequired();

        builder.Property(p => p.ActualizadoPor)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Version)
            .IsRequired();

        builder.HasIndex(p => p.Llave)
            .IsUnique();
    }
}