using Domain.RolesUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.RolesUI;

internal sealed class RolUIConfiguration : IEntityTypeConfiguration<RolUI>
{
    public void Configure(EntityTypeBuilder<RolUI> builder)
    {
        builder.HasKey(ru => ru.Id);

        builder.HasIndex(ru => ru.RolId)
            .IsUnique();

        builder.Property(ru => ru.TextColor)
            .IsRequired()
            .HasMaxLength(7)
            .HasDefaultValue("#000000");

        builder.Property(ru => ru.BackgroundColor)
            .IsRequired()
            .HasMaxLength(7)
            .HasDefaultValue("#FFFFFF");
    }
}