using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Roles;

internal sealed class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.NombreRol).IsUnique();

        builder.Property(r => r.NombreRol)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Descripcion)
            .HasMaxLength(250);
    }
}
