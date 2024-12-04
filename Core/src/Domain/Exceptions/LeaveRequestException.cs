namespace Domain.Exceptions;
public class LeaveRequestException : Exception
{
    public LeaveRequestException()
    {

    }

    public LeaveRequestException(string message) : base(message)
    {

    }
}
