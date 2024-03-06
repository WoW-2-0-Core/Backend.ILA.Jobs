using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WorkerScheduler.Persistence.DataContexts;

namespace ClassLibrary1WorkerScheduler.Worker.Configurations;

public static partial class HostConfiguration
{
    private static readonly ICollection<Assembly> Assemblies;

    static HostConfiguration()
    {
        Assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        Assemblies.Add(Assembly.GetExecutingAssembly());
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