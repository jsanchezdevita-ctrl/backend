using Domain.Zonas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Zonas;

internal sealed class ZonaConfiguration : IEntityTypeConfiguration<Zona>
{
    public void Configure(EntityTypeBuilder<Zona> builder)
    {
        builder.HasKey(z => z.Id);

        builder.HasIndex(z => z.Nombre).IsUnique();

        builder.Property(z => z.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(z => z.Descripcion)
            .HasMaxLength(500);

        builder
            .HasMany(z => z.PuntosDeControlAsociados)
            .WithOne(zpc => zpc.Zona)
            .HasForeignKey(zpc => zpc.ZonaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}