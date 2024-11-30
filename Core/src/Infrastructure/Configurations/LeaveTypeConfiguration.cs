using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public sealed class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.ToTable(nameof(LeaveType));

        builder.Property(x => x.Id).HasConversion(
            leaveTypeId => leaveTypeId.Value,
            value => new LeaveTypeId(value));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
                .HasColumnType("[varchar](100)")
                .IsRequired();

        builder.Property(x => x.MaxDaysPerYear).IsRequired();

        builder.HasData(
            new LeaveType(name: "Holiday", maxDaysPerYear: 30),
            new LeaveType(name: "SickLeave", maxDaysPerYear: 10),
            new LeaveType(name: "Maternity", maxDaysPerYear: 80),
            new LeaveType(name: "Paternity", maxDaysPerYear: 25),
            new LeaveType(name: "MarriageOrPACS", maxDaysPerYear: 4),
            new LeaveType(name: "ChildBirth", maxDaysPerYear: 1));
    }
}
