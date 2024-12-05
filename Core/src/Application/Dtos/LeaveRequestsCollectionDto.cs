using Domain.Entities;

namespace Application.Dtos;

public record class LeaveRequestsCollectionDto(
    int Count = 0,
    IReadOnlyCollection<LeaveRequest>? Items = null)
{
    public static readonly LeaveRequestsCollectionDto Default = new LeaveRequestsCollectionDto();
    public IReadOnlyCollection<LeaveRequest> Items { get; init; } = Items ?? [];
}
