using Application.Commands.CreateLeaveRequest;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Application;
public static class MappingExtensions
{
    public static LeaveRequest MapToLeaveRequest(this CreateLeaveRequestCommand command)
    {
        return new LeaveRequest(
            submittedBy: new UserId(command.SubmittedBy),
            leaveType: LeaveType.FromString(command.LeaveType),
            startDate: command.StartDate,
            endDate: command.EndDate,
            comment: command.Comment);
    }
}
