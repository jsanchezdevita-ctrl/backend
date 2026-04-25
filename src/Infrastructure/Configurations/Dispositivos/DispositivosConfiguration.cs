using Domain.Dispositivos;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Dispositivos;

internal sealed class DispositivoConfiguration : IEntityTypeConfiguration<Dispositivo>
{
    public void Configure(EntityTypeBuilder<Dispositivo> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.DispositivoId)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.DireccionIp)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasIndex(d => d.DispositivoId).IsUnique();
        builder.HasIndex(d => d.Nombre).IsUnique();
        builder.HasIndex(d => d.DireccionIp).IsUnique();

        builder.Property(d => d.UltimaConexion)
            .IsRequired(false);

        builder.HasOne<PuntoControl>()
            .WithMany()
            .HasForeignKey(d => d.PuntoControlId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}