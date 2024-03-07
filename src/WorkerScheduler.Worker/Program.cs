using WorkerScheduler.Worker.Configurations;

var builder = Host.CreateApplicationBuilder(args);
await builder.ConfigureAsync();

var host = builder.Build();
await host.ConfigureAsync();

host.Run();