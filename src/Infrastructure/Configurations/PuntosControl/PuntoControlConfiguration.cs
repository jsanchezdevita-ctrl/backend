using Domain.Enums;
using Domain.PuntosControl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.PuntosControl;

internal sealed class PuntoControlConfiguration : IEntityTypeConfiguration<PuntoControl>
{
    public void Configure(EntityTypeBuilder<PuntoControl> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Ubicacion)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Tipo)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (PuntoControlType)Enum.Parse(typeof(PuntoControlType), v))
            .HasMaxLength(20);

        builder.Property(p => p.Estado)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => (PuntoControlState)Enum.Parse(typeof(PuntoControlState), v));

        builder.Property(p => p.Descripcion)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasIndex(p => p.Nombre)
            .IsUnique();

        builder
            .HasMany(p => p.ZonasAsociadas)
            .WithOne(z => z.PuntoControl)
            .HasForeignKey(z => z.PuntoControlId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}