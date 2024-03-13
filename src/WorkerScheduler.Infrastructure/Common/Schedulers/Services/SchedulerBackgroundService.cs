using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerScheduler.Application.Common.EventBus.EventSubscribers;
using WorkerScheduler.Application.Common.Schedulers.Events;
using WorkerScheduler.Application.Common.Schedulers.Services;
using WorkerScheduler.Application.Common.WorkerJobExecutionHistories.Services;
using WorkerScheduler.Application.Common.WorkerJobs.Services;
using WorkerScheduler.Domain.Constants;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Domain.Enums;
using WorkerScheduler.Domain.Extensions;
using WorkerScheduler.Infrastructure.Common.Schedulers.EventSubscribers;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Infrastructure.Common.Schedulers.Services;

public class SchedulerBackgroundService(
    [FromKeyedServices(EventBusConstants.SchedulerEventSubscriber)] IEnumerable<IEventSubscriber> eventSubscribers,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<SchedulerBackgroundService> logger
) : BackgroundService
{
    private IWorkerJobService? _workerJobService;
    private IWorkerJobExecutionHistoryService? _executionHistoryService;
    private IJobSchedulerService? _jobSchedulerService;

    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));
    private Guid? _nextJobId;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var eventSubscriber in eventSubscribers.OfType<SchedulerEventSubscriber>())
            eventSubscriber.HandleProcessAsync += HandleJobResultAsync;

        await Task.WhenAll(eventSubscribers.Select(eventSubscriber => eventSubscriber.StartAsync(stoppingToken).AsTask()));

        var scope = serviceScopeFactory.CreateScope();
        _workerJobService = scope.ServiceProvider.GetRequiredService<IWorkerJobService>();
        _jobSchedulerService = scope.ServiceProvider.GetRequiredService<IJobSchedulerService>();
        _executionHistoryService = scope.ServiceProvider.GetRequiredService<IWorkerJobExecutionHistoryService>();

        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            await PublishNextJobAsync(_nextJobId, stoppingToken);
    }

    private async ValueTask PublishNextJobAsync(Guid? jobId = default, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        if (_jobSchedulerService is null || _workerJobService is null)
            throw new InvalidOperationException("Service not initialized");

        if (jobId.HasValue)
        {
            var job = await _workerJobService.GetByIdAsync(jobId.Value, cancellationToken: cancellationToken) ??
                      throw new InvalidOperationException($"Job with ID {jobId} not found");

            // Update job to queued state
            await _jobSchedulerService.UpdateJobsStatusAsync(WorkerJobStatus.Queued, cancellationToken, job);

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
    
    private async ValueTask<(bool Result, bool Requeue)> HandleJobResultAsync(RecordJobHistoryEvent @event, CancellationToken cancellationToken)
    {
        if (_jobSchedulerService is null)
            throw new InvalidOperationException("Services are not initialized");
        
        var recordHistoryTask = () => _jobSchedulerService.CreateJobHistoryAsync(
            new WorkerJobExecutionHistoryEntity
            {
                JobId = @event.JobId,
                IsSuccessful = @event.IsSuccessful,
                ExecutionTime = DateTimeOffset.UtcNow,
                RetryForHistoryId = @event.ParentHistoryId,
                RetryCount = @event.RetryCount
            },
            cancellationToken
        );

        // Write to database with try-catch
        await recordHistoryTask.GetValueAsync();

        return (true, false);
    }
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(eventSubscribers.Select(eventSubscriber => eventSubscriber.StopAsync(cancellationToken).AsTask()));
    }
}