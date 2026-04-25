using Domain.Dispositivos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Dispositivos;

internal sealed class DispositivoConfiguracionConfiguration : IEntityTypeConfiguration<DispositivoConfiguracion>
{
    public void Configure(EntityTypeBuilder<DispositivoConfiguracion> builder)
    {
        builder.HasKey(dc => dc.Id);

        builder.Property(dc => dc.DispositivoId)
            .IsRequired();

        builder.Property(dc => dc.FrecuenciaSincronizacionSegundos)
            .IsRequired();

        builder.Property(dc => dc.PotenciaTransmision)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(dc => dc.CanalComunicacion)
            .IsRequired();

        builder.HasOne<Dispositivo>()
            .WithOne()
            .HasForeignKey<DispositivoConfiguracion>(dc => dc.DispositivoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(dc => dc.DispositivoId).IsUnique();
    }
}