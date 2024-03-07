using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Infrastructure.Common.EventBus.Settings;

namespace WorkerScheduler.Infrastructure.Common.EventBus.Brokers;

/// <summary>
/// Provides RabbitMQ connection
/// </summary>
public class RabbitMqConnectionProvider : IRabbitMqConnectionProvider
{
    /// <summary>
    /// Connection factory
    /// </summary>
    private readonly ConnectionFactory _connectionFactory;
    
    /// <summary>
    /// Connection
    /// </summary>
    private IConnection? _connection;

    public RabbitMqConnectionProvider(IOptions<RabbitMqConnectionSettings> rabbitMqConnectionSettings)
    {
        var rabbitMqConnectionSettings1 = rabbitMqConnectionSettings.Value;

        _connectionFactory = new ConnectionFactory
        {
            HostName = rabbitMqConnectionSettings1.HostName,
            Port =  rabbitMqConnectionSettings1.Port
        };
    }
    
    public async ValueTask<IChannel> CreateChannelAsync()
    {
        _connection ??= await _connectionFactory.CreateConnectionAsync();

        return await _connection.CreateChannelAsync();
    }
}