namespace Domain.Errors;
public static class LeaveRequestErrorMessages
{
    public static readonly string EndDateShouldBeGraterThanStartDate = "Invalid Dates. The End date should be greater than the start date.";
    public static readonly string StartDateShouldNotBeInPast = "Invalid start date. The start date cannot be in the past.";
    public static readonly string EmployeeIdShouldNotBeNull = "Invalid Employee Id. The Employee Id should not be null.";
    public static readonly string LeaveTypeIdShouldNotBeNull = "Invalid LeaveType Id. The LeaveType Id should not be null.";
    public static readonly string StartDateShouldNotBeNullOrEmpty = "Invalid start date. The start date should not be null or empty.";
    public static readonly string EndDateShouldNotBeNullOrEmpty = "Invalid end date. The end date should not be null or empty.";
    public static readonly string LeaveRequestIdShouldNotBeNull = "Invalid LeaveRequest Id. The LeaveRequestId should not be null.";
    public static readonly string LeaveRequestStatusShouldNotBeNullOrEmpty = "Invalid LeaveRequest new status. The new LeaveRequest status should not be null or empty.";
    public static readonly string NotFoundLEaveRequestToUpdate = "Not found LeaveRequest Id. The LeaveRequest to update is not found.";
}
