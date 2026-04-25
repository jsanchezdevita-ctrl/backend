using Domain.PuntosControl;
using Domain.Zonas;
using Domain.ZonasPuntosControl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ZonasPuntosControl;

internal sealed class ZonaPuntoControlConfiguration : IEntityTypeConfiguration<ZonaPuntoControl>
{
    public void Configure(EntityTypeBuilder<ZonaPuntoControl> builder)
    {
        builder.HasKey(zpc => zpc.Id);

        builder.HasIndex(zpc => new { zpc.ZonaId, zpc.PuntoControlId }).IsUnique();

        builder
            .HasOne(x => x.Zona)
            .WithMany(x => x.PuntosDeControlAsociados)
            .HasForeignKey(zpc => zpc.ZonaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.PuntoControl)
            .WithMany(x => x.ZonasAsociadas)
            .HasForeignKey(zpc => zpc.PuntoControlId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}