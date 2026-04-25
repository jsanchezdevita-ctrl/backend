using Domain.Permisos;
using Domain.Roles;
using Domain.RolesPermisos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.RolesPermisos;

internal sealed class RolPermisoConfiguration : IEntityTypeConfiguration<RolPermiso>
{
    public void Configure(EntityTypeBuilder<RolPermiso> builder)
    {
        builder.HasKey(rp => rp.Id);

        builder.HasIndex(rp => new { rp.RolId, rp.PermisoId }).IsUnique();

        builder.Property(rp => rp.RolId)
            .IsRequired();

        builder.Property(rp => rp.PermisoId)
            .IsRequired();

        builder.HasOne<Permiso>()
            .WithMany()
            .HasForeignKey(ur => ur.PermisoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Rol>()
            .WithMany()
            .HasForeignKey(ur => ur.RolId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
