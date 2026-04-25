using Domain.EstadosRegistro;
using Domain.PuntosControl;
using Domain.RegistrosIngresosEgresos;
using Domain.Roles;
using Domain.Zonas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.RegistrosIngresosEgresos;

internal sealed class RegistroIngresoEgresoConfiguration : IEntityTypeConfiguration<RegistroIngresoEgreso>
{
    public void Configure(EntityTypeBuilder<RegistroIngresoEgreso> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Fecha)
            .IsRequired();

        builder.Property(rie => rie.UsuarioId)
            .IsRequired();

        builder.HasOne<PuntoControl>()
            .WithMany()
            .HasForeignKey(r => r.PuntoEntradaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PuntoControl>()
            .WithMany()
            .HasForeignKey(r => r.PuntoSalidaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<EstadoRegistro>()
            .WithMany()
            .HasForeignKey(r => r.EstadoRegistroId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Zona>()
            .WithMany()
            .HasForeignKey(r => r.ZonaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Rol>()
            .WithMany()
            .HasForeignKey(r => r.RolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(u => u.Observacion)
            .IsRequired(false);
    }
}
