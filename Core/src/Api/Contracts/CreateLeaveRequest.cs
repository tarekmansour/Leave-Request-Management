using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public record CreateLeaveRequest(
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