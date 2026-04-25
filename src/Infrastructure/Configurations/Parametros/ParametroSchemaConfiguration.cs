using Domain.Parametros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Parametros;

internal sealed class ParametroSchemaConfiguration : IEntityTypeConfiguration<ParametroSchema>
{
    public void Configure(EntityTypeBuilder<ParametroSchema> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Llave)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ps => ps.Schema)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(ps => ps.FechaCreacion)
            .IsRequired();

        builder.Property(ps => ps.FechaActualizacion)
            .IsRequired();

    }
}