using Domain.Analytics.AccesosTipoUsuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AnalyticsConfigurations.AccesosTipoUsuario;

internal sealed class AccesosTipoUsuarioConfiguration : IEntityTypeConfiguration<AccesoTipoUsuario>
{
    public void Configure(EntityTypeBuilder<AccesoTipoUsuario> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.TipoUsuario)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(d => d.Cantidad)
            .IsRequired();

        builder.Property(d => d.Fecha)
            .IsRequired()
            .HasColumnType("timestamp without time zone"); ;
    }
}