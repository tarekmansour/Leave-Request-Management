using Application.Commands.CreateLeaveRequest;
using Application.Dtos;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.Identifiers;

namespace Application;
public static class MappingExtensions
{
    public static LeaveRequest MapToLeaveRequest(this CreateLeaveRequestCommand command, int userId)
        => new LeaveRequest(
            submittedBy: new UserId(userId),
            leaveType: LeaveType.FromString(command.LeaveType),
            startDate: command.StartDate,
            endDate: command.EndDate,
            comment: command.Comment);

    public static UpdatedLeaveRequestDto MapToUpdatedLeaveRequestDto(this LeaveRequest command)
        => new(
            Id: command.Id.Value,
            SubmittedBy: command.SubmittedBy.Value,
            LeaveType: command.LeaveType.ToString(),
            StartDate: command.StartDate,
            EndDate: command.EndDate,
            Status: command.Status.ToString(),
            DecisionReason: command.DecisionReason);

    public static LeaveRequestsCollectionDto MapToLeaveRequestsCollectionDto(this IReadOnlyCollection<LeaveRequest>? leaveRequests)
    {
        if (leaveRequests == null || leaveRequests.Count == 0)
        {
            return LeaveRequestsCollectionDto.Default;
        }

        return new LeaveRequestsCollectionDto(
            Count: leaveRequests.Count,
            Items: (IReadOnlyCollection<LeaveRequestDto>?)leaveRequests.Select(x =>
            {
                return x.Status == LeaveRequestStatus.Rejected
                    ? new LeaveRequestDto(
                        Id: x.Id.Value,
                        LeaveType: x.LeaveType.Value,
                        StartDate: x.StartDate,
                        EndDate: x.EndDate,
                        Status: x.Status.Value,
                        DecidedBy: x.DecidedBy.Value,
                        DecisionReason: x.DecisionReason)
                    : new LeaveRequestDto(
                        Id: x.Id.Value,
                        LeaveType: x.LeaveType.Value,
                        StartDate: x.StartDate,
                        EndDate: x.EndDate,
                        Status: x.Status.Value);
            }).ToArray());
    }
}
