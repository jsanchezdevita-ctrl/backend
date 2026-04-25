using Domain.Dispositivos;
using Domain.PuntosControl;
using Domain.QrTokens;
using Domain.RegistrosIngresosEgresos;
using Domain.Roles;
using Domain.Usuarios;
using Domain.Zonas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.QrTokens;

internal sealed class QrTokenConfiguration : IEntityTypeConfiguration<QrToken>
{
    public void Configure(EntityTypeBuilder<QrToken> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Token)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(q => q.Token)
            .IsUnique();

        builder.Property(q => q.UsuarioId)
            .IsRequired();

        builder.Property(q => q.RolId)
            .IsRequired();

        builder.Property(q => q.FechaCreacion)
            .IsRequired();

        builder.Property(q => q.FechaExpiracion)
            .IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(q => q.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Rol>()
            .WithMany()
            .HasForeignKey(q => q.RolId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación opcional con Dispositivo
        builder.HasOne<Dispositivo>()
            .WithMany()
            .HasForeignKey(q => q.DispositivoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación opcional con PuntoControl
        builder.HasOne<PuntoControl>()
            .WithMany()
            .HasForeignKey(q => q.PuntoControlId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación opcional con ZonaRol
        builder.HasOne<Zona>()
            .WithMany()
            .HasForeignKey(q => q.ZonaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación opcional con RegistroIngresoEgreso
        builder.HasOne<RegistroIngresoEgreso>()
            .WithMany()
            .HasForeignKey(q => q.RegistroIngresoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}