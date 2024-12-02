using Domain.ValueObjects.Identifiers;

namespace Application.Dtos;
public record UpdatedLeaveRequestDto(
    LeaveRequestId Id,
    EmployeeId SubmittedBy,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    string? DecisionReason);
