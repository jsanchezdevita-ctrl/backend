using Domain.Analytics.AccesosDiaSemana;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AnalyticsConfigurations.AccesosDiaSemana;

internal sealed class AccesoDiaSemanaConfiguration : IEntityTypeConfiguration<AccesoDiaSemana>
{
    public void Configure(EntityTypeBuilder<AccesoDiaSemana> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Cantidad)
            .IsRequired();

        builder.Property(d => d.DiaSemanaCompleto)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(d => d.DiaSemanaCorto)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(d => d.Fecha)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.HasIndex(d => new { d.Fecha, d.DiaSemanaCompleto })
            .IsUnique();
    }
}