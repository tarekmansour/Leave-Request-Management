using Domain.Entities;
using Domain.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public sealed class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                leaveRequestId => leaveRequestId.Value,
                value => new LeaveRequestId(value))
            .ValueGeneratedOnAdd();

        // Ensure that the SQL Server column is configured to use IDENTITY
        builder.Property(x => x.Id)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.SubmittedBy)
            .HasConstraintName("FK_LeaveRequest_User")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.OwnsOne(x => x.LeaveType, leaveTypeBuilder =>
        {
            leaveTypeBuilder.Property(s => s.Value)
            .HasColumnName("Type")
            .HasColumnType("[varchar](50)");
        });

        builder.Property(x => x.StartDate).IsRequired();

        builder.Property(x => x.EndDate).IsRequired();

        builder.OwnsOne(x => x.Status, leaveRequestStatusBuilder =>
        {
            leaveRequestStatusBuilder.Property(s => s.Value)
            .HasColumnName("Status")
            .HasColumnType("[varchar](50)");
        });

        builder.Property(x => x.Comment).HasColumnType("[varchar](255)");

        builder.Property(x => x.DecisionReason).HasColumnType("[varchar](255)");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.DecidedBy)
            .IsRequired(false)
            .HasConstraintName("FK_LeaveRequest_User_Approval")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
