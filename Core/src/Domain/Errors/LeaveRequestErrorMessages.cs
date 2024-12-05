namespace Domain.Errors;
public static class LeaveRequestErrorMessages
{
    public static readonly string EndDateShouldBeAfterStartDate = "End date should be after the start date.";
    public static readonly string StartDateShouldNotBeInPast = "Invalid start date. The start date cannot be in the past.";
    public static readonly string UserIdShouldNotBeNull = "Invalid User Id. The User Id should not be null.";
    public static readonly string LeaveTypeShouldNotBeNullOrEmpty = "Invalid LeaveType. The LeaveType should not be null or empty.";
    public static readonly string StartDateShouldNotBeNullOrEmpty = "Invalid start date. The start date should not be null or empty.";
    public static readonly string EndDateShouldNotBeNullOrEmpty = "Invalid end date. The end date should not be null or empty.";
    public static readonly string LeaveRequestIdShouldNotBeNull = "Invalid LeaveRequest Id. The LeaveRequestId should not be null.";
    public static readonly string LeaveRequestStatusShouldNotBeNullOrEmpty = "Invalid LeaveRequest new status. The new LeaveRequest status should not be null or empty.";
    public static readonly string NotFoundLeaveRequestToUpdate = "Not found LeaveRequest Id. The LeaveRequest to update is not found.";
    public static readonly string LeaveTypeNotSupported = "The leave type is not supported.";
    public static readonly string LeaveRequestStatusNotSupported = "The leave request status is not supported.";
    public static readonly string OnlyPendingLeaveRequestCanBeApproved = "Invalid operation. Only 'Pending' leave requests can be approved.";
    public static readonly string ValidUserShouldApproveLeaveRequest = "A valid user should approve the leave request.";
    public static readonly string InvalidNewLeaveType = "The new leave type is the same as the current leave type.";
    public static readonly string StartDateShouldBeBeforeEndDate = "The start date should be before the end date.";
    public static readonly string UnsupportedStatusUpdate = "Unsupported status update.";
    public static readonly string UserNotFound = "User Id not valid. The user is not found.";
    public static readonly string ForRejectedRequestsReasonShouldBeProvided = "For rejected requests a decision reason should be provided.";
}
