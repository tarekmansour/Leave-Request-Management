using Application.Commands.CreateLeaveRequest;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Application;
public static class MappingExtensions
{
    public static LeaveRequest MapToLeaveRequest(this CreateLeaveRequestCommand command, int userId)
    {
        return new LeaveRequest(
            submittedBy: new UserId(userId),
            leaveType: LeaveType.FromString(command.LeaveType),
            startDate: command.StartDate,
            endDate: command.EndDate,
            comment: command.Comment);
    }
}
