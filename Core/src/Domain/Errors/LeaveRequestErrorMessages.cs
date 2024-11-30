namespace Domain.Errors;
public static class LeaveRequestErrorMessages
{
    public static readonly string EndDateShouldBeGraterThanStartDate = "Invalid Dates. The End date should be greater than the start date.";
    public static readonly string StartDateShouldNotBeInPast = "Invalid start date. The start date cannot be in the past.";
}
