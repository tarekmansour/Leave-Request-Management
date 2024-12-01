namespace Domain.Errors;
public static class LeaveRequestErrorMessages
{
    public static readonly string EndDateShouldBeGraterThanStartDate = "Invalid Dates. The End date should be greater than the start date.";
    public static readonly string StartDateShouldNotBeInPast = "Invalid start date. The start date cannot be in the past.";
    public static readonly string EmployeeIdShouldNotBeNull = "Invalid Employee Id. The Employee Id should not be null.";
    public static readonly string LeaveTypeIdShouldNotBeNull = "Invalid LeaveType Id. The LeaveType Id should not be null.";
    public static readonly string StartDateShouldNotBeNull = "Invalid start date. The start date should not be null.";
    public static readonly string EndDateShouldNotBeNull = "Invalid end date. The end date should not be null.";
}
