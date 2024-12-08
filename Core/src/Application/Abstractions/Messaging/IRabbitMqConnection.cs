using RabbitMQ.Client;

namespace Application.Abstractions.Messaging;
public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}
