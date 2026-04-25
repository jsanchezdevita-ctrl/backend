using Domain.EstadosRegistro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EstadosRegistro;

internal sealed class EstadoRegistroConfiguration : IEntityTypeConfiguration<EstadoRegistro>
{
    public void Configure(EntityTypeBuilder<EstadoRegistro> builder)
    {
        builder.HasKey(er => er.Id);

        builder.Property(er => er.Descripcion)
            .IsRequired()
            .HasMaxLength(100);

        // Índice único para la descripción
        builder.HasIndex(er => er.Descripcion)
            .IsUnique();
    }
}