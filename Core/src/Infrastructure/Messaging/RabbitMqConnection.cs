using Application.Abstractions.Messaging;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;
public class RabbitMqConnection : IRabbitMqConnection, IDisposable
{
    private IConnection? _connection;
    public IConnection Connection => _connection!;

    private readonly string _hostname;

    public RabbitMqConnection(string hostname)
    {
        _hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
        InitializeConnection(_hostname);
    }

    private void InitializeConnection(string hostname)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostname
        };

        _connection = factory.CreateConnection();
    }

    public void Dispose() => _connection?.Dispose();
}
