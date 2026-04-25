using Domain.Roles;
using Domain.Usuarios;
using Domain.UsuariosRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.UsuariosRoles;

internal sealed class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
{
    public void Configure(EntityTypeBuilder<UsuarioRol> builder)
    {
        builder.HasKey(ur => ur.Id);

        builder.HasIndex(ur => ur.UsuarioId);

        builder.HasIndex(ur => new { ur.UsuarioId, ur.RolId })
            .IsUnique();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(ur => ur.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Rol>()
            .WithMany()
            .HasForeignKey(ur => ur.RolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ur => ur.FechaAsignacion)
            .IsRequired();
    }
}
