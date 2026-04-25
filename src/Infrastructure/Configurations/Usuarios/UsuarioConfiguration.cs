using Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Usuarios;

internal sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.NumeroDocumento).IsUnique();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.HorarioInicio)
            .HasColumnType("time");

        builder.Property(u => u.HorarioFin)
            .HasColumnType("time");

        builder.Property(u => u.FcmToken)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(u => u.FcmTokenUpdatedAt)
            .IsRequired(false);
    }
}
