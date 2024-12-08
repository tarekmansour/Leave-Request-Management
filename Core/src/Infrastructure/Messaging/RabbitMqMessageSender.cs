using System.Text;
using System.Text.Json;
using Application.Abstractions.Messaging;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;
public class RabbitMqMessageSender : IMessageSender
{
    private readonly IRabbitMqConnection _connection;
    private readonly string _queueName;

    public RabbitMqMessageSender(
        IRabbitMqConnection connection,
        IConfiguration rabbitMqConfig)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _queueName = rabbitMqConfig["RabbitMQ:QueueName"] ?? throw new ArgumentNullException(nameof(rabbitMqConfig));
    }

    public void SendMessage<T>(T message)
    {
        using var channel = _connection.Connection.CreateModel();

        channel.QueueDeclare(queue: _queueName, exclusive: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: _queueName, body: body);
    }
}
