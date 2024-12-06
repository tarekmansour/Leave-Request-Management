namespace Application.Dtos;

public record class LeaveRequestsCollectionDto(
    int Count = 0,
    IReadOnlyCollection<LeaveRequestDto>? Items = null)
{
    public static readonly LeaveRequestsCollectionDto Default = new();
    public IReadOnlyCollection<LeaveRequestDto> Items { get; init; } = Items ?? [];
}

public record LeaveRequestDto(
    int Id,
    string LeaveType,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    string? Comment = null,
    int? DecidedBy = null,
    string? DecisionReason = null);