namespace Application.Abstractions.Messaging;
public interface IMessageSender
{
    //Task SendMessageAsync(LeaveRequestStatusChangedDto leaveRequestStatusChangedDto);

    void SendMessage<T>(T message);
}
