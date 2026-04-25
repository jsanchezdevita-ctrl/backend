using Domain.Analytics.AccesosPorHora;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AnalyticsConfigurations.AccesosPorHora;

internal sealed class AccesoPorHoraConfiguration : IEntityTypeConfiguration<AccesoPorHora>
{
    public void Configure(EntityTypeBuilder<AccesoPorHora> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Cantidad)
            .IsRequired();

        builder.Property(d => d.Hora)
            .IsRequired();

        builder.Property(d => d.Fecha)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        // Índice único para evitar duplicados (Fecha + Hora)
        builder.HasIndex(d => new { d.Fecha, d.Hora })
            .IsUnique();

        // Validación para hora entre 0-23
        //builder.HasCheckConstraint("CK_AccesosPorHora_Hora_Range", "\"Hora\" >= 0 AND \"Hora\" <= 23");
    }
}