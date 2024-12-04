namespace Application.Dtos;
public record UpdatedLeaveRequestDto(
    int Id,
    int SubmittedBy,
    string LeaveType,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    string? DecisionReason);
