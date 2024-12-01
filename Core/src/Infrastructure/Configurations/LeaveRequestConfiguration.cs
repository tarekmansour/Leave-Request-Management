using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public sealed class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable(nameof(LeaveRequest));

        builder.Property(x => x.Id).HasConversion(
            leaveRequestId => leaveRequestId.Value,
            value => new LeaveRequestId(value));

        builder.HasKey(x => x.Id);

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .HasConstraintName("FK_LeaveRequest_Employee")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<LeaveType>()
            .WithMany()
            .HasForeignKey(x => x.LeaveTypeId)
            .HasConstraintName("FK_LeaveRequest_LeaveType")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(x => x.StartDate).IsRequired();

        builder.Property(x => x.EndDate).IsRequired();

        builder.OwnsOne(x => x.Status, leaveRequestStatusBuilder =>
        {
            leaveRequestStatusBuilder.Property(s => s.Value)
            .HasColumnName("Status")
            .HasColumnType("[varchar](50)");
        });

        builder.Property(x => x.Comment).HasColumnType("[nvarchar](max)");

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.ApprovedBy)
            .HasConstraintName("FK_LeaveRequest_Employee_Approval")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
