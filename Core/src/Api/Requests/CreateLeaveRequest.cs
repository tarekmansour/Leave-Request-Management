using System.ComponentModel.DataAnnotations;
using Domain.ValueObjects.Identifiers;

namespace Api.Requests;

public record CreateLeaveRequest
{
    [Required]
    public required EmployeeId EmployeeId { get; init; }
    [Required]
    public required LeaveTypeId LeaveTypeId { get; init; }
    [Required]
    public required DateTime StartDate { get; init; }
    [Required]
    public required DateTime EndDate { get; init; }
    public string? Comment { get; init; }
};