using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Id).HasConversion(
            userId => userId.Value,
            value => new UserId(value));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.FirstName)
                .HasColumnType("[varchar](100)")
                .IsRequired();

        builder.Property(x => x.LastName)
                .HasColumnType("[varchar](100)")
                .IsRequired();

        builder.Property(x => x.Email)
                .HasColumnType("[varchar](100)")
                .IsRequired();

        builder.Property(x => x.Roles)
                .HasColumnType("[varchar](max)")
                .IsRequired();

    }
}
