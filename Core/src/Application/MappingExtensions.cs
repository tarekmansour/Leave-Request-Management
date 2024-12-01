using Application.Commands.CreateLeaveRequest;
using Domain.Entities;

namespace Application;
public static class MappingExtensions
{
    public static LeaveRequest MapToLeaveRequest(this CreateLeaveRequestCommand command)
    {
        return new LeaveRequest(
            employeeId: command.EmployeeId,
            leaveTypeId: command.LeaveTypeId,
            startDate: command.StartDate,
            endDate: command.EndDate,
            comment: command.Comment);
    }
}
