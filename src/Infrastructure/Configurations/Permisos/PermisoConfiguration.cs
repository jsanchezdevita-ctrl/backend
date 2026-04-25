using Domain.Permisos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Permisos;

internal sealed class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.Nombre).IsUnique();

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(100);
    }
}
