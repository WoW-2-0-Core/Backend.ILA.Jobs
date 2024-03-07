using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerScheduler.Application.Common.Schedulers.Services;
using WorkerScheduler.Domain.Enums;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Infrastructure.Common.Schedulers.Services;

public class SchedulerBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<SchedulerBackgroundService> logger)
    : BackgroundService
{
    private IWorkerJobRepository? _workerJobRepository;
    private IJobSchedulerService? _jobSchedulerService;

    private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    private Guid? _nextJobId = default;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = serviceScopeFactory.CreateScope();
        _workerJobRepository = scope.ServiceProvider.GetRequiredService<IWorkerJobRepository>();
        _jobSchedulerService = scope.ServiceProvider.GetRequiredService<IJobSchedulerService>();
        
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            await PublishNextJobAsync(_nextJobId, stoppingToken);
    }

    private async ValueTask PublishNextJobAsync(Guid? jobId = default, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        if (_jobSchedulerService is null || _workerJobRepository is null)
            throw new InvalidOperationException("Service not initialized");

        if (jobId.HasValue)
        {
            var job = await _workerJobRepository.GetByIdAsync(jobId.Value, cancellationToken: cancellationToken) ??
                      throw new InvalidOperationException($"Job with ID {jobId} not found");

            // Update job to queued state
            await _jobSchedulerService.UpdateJobsStatus(WorkerJobStatus.Queued, cancellationToken, job);

            // Publish job
            await _jobSchedulerService.PublishJobsAsync(cancellationToken, job);

            // Reset next job Id
            _nextJobId = null;
        }

        // Get next scheduled job
        var nextSchedule = await _jobSchedulerService.GetNextScheduledJob(cancellationToken);

        // Set next scheduled time
        if (nextSchedule.HasValue)
        {
            _timer.Period = nextSchedule.Value.nextJobScheduledTime - DateTimeOffset.UtcNow <= TimeSpan.FromSeconds(1) 
                ? TimeSpan.FromSeconds(1) 
                : nextSchedule.Value.nextJobScheduledTime - DateTimeOffset.UtcNow;
            _nextJobId = nextSchedule.Value.jobId;
        }
        else
            // TODO : get from settings
            _timer.Period = TimeSpan.FromSeconds(5);
    }
}