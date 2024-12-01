using Domain.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable(nameof(Employee));

        builder.Property(x => x.Id).HasConversion(
            employeeId => employeeId.Value,
            value => new EmployeeId(value));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
                .HasColumnType("[varchar](100)")
                .IsRequired();

        builder.Property(x => x.LastName)
                .HasColumnType("[varchar](100)")
                .IsRequired();

        builder.Property(x => x.Email)
                .HasColumnType("[varchar](100)")
                .IsRequired();

        builder.OwnsOne(x => x.Position, employeePosition =>
        {
            employeePosition.Property(p => p.Value).HasColumnType("[varchar](100)");
        });

        builder.HasData(
            new Employee(id: new EmployeeId(1), firstName: "Michel", lastName: "Barry", email: "mBarry@gmail.com", position: EmployeePosition.Manager),
            new Employee(id: new EmployeeId(2), firstName: "Victor", lastName: "Majory", email: "vMajory@gmail.com", position: EmployeePosition.Employee),
            new Employee(id: new EmployeeId(3), firstName: "Christophe", lastName: "Garcia", email: "cGarcia@gmail.com", position: EmployeePosition.Employee));
    }
}
