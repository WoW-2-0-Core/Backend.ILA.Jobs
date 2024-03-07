namespace WorkerScheduler.Worker.Configurations;

public static partial class HostConfiguration
{
    /// <summary>
    /// Configures application builder
    /// </summary>
    /// <param name="builder">Application builder</param>
    /// <returns>Application builder</returns>
    public static ValueTask<IHostApplicationBuilder> ConfigureAsync(this IHostApplicationBuilder builder)
    {
        builder
            .AddSerializers()
            .AddMediator()
            .AddEventBus()
            .AddPersistence()
            .AddSchedulerInfrastructure()
            .AddWorkerInfrastructure();
        
        return new(builder);
    }

    /// <summary>
    /// Configures application
    /// </summary>
    /// <param name="host">Application host</param>
    /// <returns>Application host</returns>
    public static async  ValueTask<IHost> ConfigureAsync(this IHost host)
    {
        await host.MigrateDataBaseSchemasAsync();
        await host.SeedDataAsync();
        
        return host;
    }
}