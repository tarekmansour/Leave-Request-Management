using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public sealed class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.Property(x => x.Id).HasConversion(
            leaveTypeId => leaveTypeId.Value,
            value => new LeaveTypeId(value));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
                .HasColumnType("[varchar](100)")
                .IsRequired();

        builder.Property(x => x.MaxDaysPerYear).IsRequired();

        builder.HasData(
            new LeaveType(id: new LeaveTypeId(1), name: "Holiday", maxDaysPerYear: 30),
            new LeaveType(id: new LeaveTypeId(2), name: "SickLeave", maxDaysPerYear: 10),
            new LeaveType(id: new LeaveTypeId(3), name: "Maternity", maxDaysPerYear: 80),
            new LeaveType(id: new LeaveTypeId(4), name: "Paternity", maxDaysPerYear: 25),
            new LeaveType(id: new LeaveTypeId(5), name: "MarriageOrPACS", maxDaysPerYear: 4));
    }
}
