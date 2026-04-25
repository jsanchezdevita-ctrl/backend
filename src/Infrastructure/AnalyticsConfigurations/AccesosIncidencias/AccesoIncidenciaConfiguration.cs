using Domain.Analytics.AccesosIncidencias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AnalyticsConfigurations.AccesosIncidencias;

internal sealed class AccesoIncidenciaConfiguration : IEntityTypeConfiguration<AccesoIncidencia>
{
    public void Configure(EntityTypeBuilder<AccesoIncidencia> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.PuntoControlId)
            .IsRequired();

        builder.Property(d => d.NombrePuntoControl)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Incidencia)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Cantidad)
            .IsRequired();

        builder.Property(d => d.Fecha)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.HasIndex(d => new { d.PuntoControlId, d.Incidencia, d.Fecha })
            .IsUnique();
    }
}