using Domain.Roles;
using Domain.Zonas;
using Domain.ZonasRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ZonasRoles;

internal sealed class ZonaRolConfiguration : IEntityTypeConfiguration<ZonaRol>
{
    public void Configure(EntityTypeBuilder<ZonaRol> builder)
    {
        builder.HasKey(zr => zr.Id);

        builder.HasIndex(zr => new { zr.ZonaId, zr.RolId }).IsUnique();

        builder.Property(zr => zr.CapacidadMaxima)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(zr => zr.EspacioUtilizado)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne<Zona>()
            .WithMany()
            .HasForeignKey(zr => zr.ZonaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Rol>()
            .WithMany()
            .HasForeignKey(zr => zr.RolId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}