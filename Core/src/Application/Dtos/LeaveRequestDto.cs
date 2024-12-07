namespace Application.Dtos;

public record LeaveRequestDto(
    int Id,
    string LeaveType,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    string? Comment = null,
    int? DecidedBy = null,
    string? DecisionReason = null);