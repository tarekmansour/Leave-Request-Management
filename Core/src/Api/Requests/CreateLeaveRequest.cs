using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record CreateLeaveRequest(
    [Required] int SubmittedBy,
    [Required] LeaveType LeaveType,
    [Required] DateTime StartDate,
    [Required] DateTime EndDate,
    string? Comment);

public enum LeaveType
{
    Off,
    SickLeave,
    Maternity,
    Paternity,
    MarriageOrPACS
}