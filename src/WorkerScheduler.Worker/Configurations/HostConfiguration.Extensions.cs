using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Serializers;
using WorkerScheduler.Infrastructure.Common.EventBus.Brokers;
using WorkerScheduler.Infrastructure.Common.EventBus.Settings;
using WorkerScheduler.Infrastructure.Common.Serializers;
using WorkerScheduler.Persistence.DataContexts;

namespace WorkerScheduler.Worker.Configurations;

public static partial class HostConfiguration
{
    private static readonly ICollection<Assembly> Assemblies;

    static HostConfiguration()
    {
        Assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        Assemblies.Add(Assembly.GetExecutingAssembly());
    }
    
    /// <summary>
    /// Configures serializers
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static IHostApplicationBuilder AddSerializers(this IHostApplicationBuilder builder)
    {
        // Register brokers
        builder.Services
            .AddSingleton<IJsonSerializationSettingsProvider, JsonSerializationSettingsProvider>();

        return builder;
    }
    
    /// <summary>
    /// Adds MediatR services to the application with custom service registrations.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static IHostApplicationBuilder AddMediator(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(Assemblies.ToArray()); });

        return builder;
    }
    
    private static IHostApplicationBuilder AddEventBus(this IHostApplicationBuilder builder)
    {
        // Register settings
        builder.Services
            .Configure<RabbitMqConnectionSettings>(builder.Configuration.GetSection(nameof(RabbitMqConnectionSettings)));
        
        // Register brokers
        builder.Services
            .AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>()
            .AddSingleton<IEventBusBroker, RabbitMqEventBusBroker>();

        // Register event subscribers
        
        return builder;
    }

    /// <summary>
    /// Adds persistence-related services to the web application builder.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
        // Register db context
        builder.Services.AddDbContext<WorkerDbContext>(
            options =>
            {
                options.EnableDetailedErrors();
                options.UseNpgsql(builder.Configuration.GetConnectionString("WorkerDatabaseConnection"));
                options.EnableSensitiveDataLogging();
            }
        );

        return builder;
    }

    /// <summary>
    /// Asynchronously migrates database schemas associated with the application.
    /// </summary>
    /// <param name="host">The WebApplication instance to configure.</param>
    /// <returns>A ValueTask representing the asynchronous operation, with the WebApplication instance.</returns>
    private static async ValueTask<IHost> MigrateDataBaseSchemasAsync(this IHost host)
    {
        var serviceScopeFactory = host.Services.GetRequiredKeyedService<IServiceScopeFactory>(null);

        await serviceScopeFactory.MigrateAsync<WorkerDbContext>();

        return host;
    }
}