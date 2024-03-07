using WorkerScheduler.Worker;
using WorkerScheduler.Worker.Configurations;

var builder = Host.CreateApplicationBuilder(args);

await builder.ConfigureAsync();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

await host.ConfigureAsync();
host.Run();