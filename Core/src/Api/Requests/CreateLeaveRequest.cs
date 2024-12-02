using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record CreateLeaveRequest(
    [Required] int SubmittedBy,
    [Required] int LeaveTypeId,
    [Required] DateTime StartDate,
    [Required] DateTime EndDate,
    string? Comment);