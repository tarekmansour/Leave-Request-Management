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

        builder.HasData(
            new User(id: new UserId(1), firstName: "Michel", lastName: "Barry", email: "mBarry@gmail.com"),
            new User(id: new UserId(2), firstName: "Victor", lastName: "Majory", email: "vMajory@gmail.com"),
            new User(id: new UserId(3), firstName: "Christophe", lastName: "Garcia", email: "cGarcia@gmail.com"));
    }
}
