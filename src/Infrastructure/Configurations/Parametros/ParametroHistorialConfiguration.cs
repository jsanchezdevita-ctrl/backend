using Domain.Parametros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Parametros;

internal sealed class ParametroHistorialConfiguration : IEntityTypeConfiguration<ParametroHistorial>
{
    public void Configure(EntityTypeBuilder<ParametroHistorial> builder)
    {
        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.Llave)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ph => ph.Valor)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(ph => ph.Descripcion)
            .HasMaxLength(500);

        builder.Property(ph => ph.FechaActualizacion)
            .IsRequired();

        builder.Property(ph => ph.ActualizadoPor)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ph => ph.Version)
            .IsRequired();

        builder.HasIndex(ph => new { ph.Llave, ph.Version })
            .IsUnique();

        builder.HasIndex(ph => ph.Llave);
        builder.HasIndex(ph => ph.FechaActualizacion);
    }
}