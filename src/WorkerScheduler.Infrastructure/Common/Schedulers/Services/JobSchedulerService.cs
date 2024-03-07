using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Schedulers.Events;
using WorkerScheduler.Application.Common.Schedulers.Services;
using WorkerScheduler.Domain.Common.Events;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Domain.Enums;
using WorkerScheduler.Infrastructure.Common.EventBus.Settings;
using WorkerScheduler.Infrastructure.Common.Schedulers.EventSubscribers;
using WorkerScheduler.Infrastructure.Common.Schedulers.Settings;
using WorkerScheduler.Infrastructure.Common.Workers.Services;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Infrastructure.Common.Schedulers.Services;

/// <summary>
/// Provides job scheduler functionality.
/// </summary>
public class JobSchedulerService(
    IOptions<SchedulerEventBusSettings> schedulerEventBusSettings,
    // IOptions<EventBusSubscriberSettings<WorkerEventSubscriber>> workerEventBusSettings,
    IWorkerJobRepository workerJobRepository, 
    IEventBusBroker eventBusBroker
    ) : IJobSchedulerService
{
    private readonly SchedulerEventBusSettings _schedulerEventBusSettings = schedulerEventBusSettings.Value;
    
    public async ValueTask<(Guid jobId, DateTimeOffset nextJobScheduledTime)?> GetNextScheduledJob(CancellationToken cancellationToken = default)
    {
        // Get the next scheduled job
        var allJobs = await workerJobRepository.Get(job => job.Status == WorkerJobStatus.Scheduled)
            // .Include(job => job.ExecutionHistories
            //     .OrderByDescending(history => history.ExecutionTime)
            //     .FirstOrDefault()
            // )
            .ToListAsync(cancellationToken: cancellationToken);

        var scheduledJobs = allJobs
            .Select(
                job =>
                {
                    var schedule = job.CronSchedule.GetNextValidTimeAfter(DateTime.UtcNow) ??
                                   throw new InvalidOperationException("Invalid cron schedule");

                    return (Job: job, Schedule: schedule);
                }
            )
            .OrderBy(jobAndSchedule => jobAndSchedule.Schedule)
            .ToList();

        // .OrderBy(job => job.CronSchedule)
        // .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        // Calculate the time until the next scheduled job
        // var timeUntilNextScheduledJob = nextScheduledJob.ScheduledTime - DateTime.UtcNow;

        return scheduledJobs.Any() ? (scheduledJobs.First().Job.Id, scheduledJobs.First().Schedule) : null;
    }

    public async ValueTask<IImmutableList<WorkerJobEntity>> GetScheduledOrFailedJobsAsync(CancellationToken cancellationToken = default)
    {
        // Query all jobs that needs to be run
        var jobs = await workerJobRepository.Get(job => job.Status == WorkerJobStatus.Scheduled || job.Status == WorkerJobStatus.Failed)
            .ToListAsync(cancellationToken: cancellationToken);

        // Filter failed jobs and scheduled jobs
        var failedJobEvents = jobs.Where(job => job.Status == WorkerJobStatus.Failed);
        // var scheduledJobs = jobs.Where(job => job.Status == WorkerJobStatus.Scheduled);

        // Publish failed jobs to the queue

        // failedJobEvents
        return failedJobEvents.ToImmutableList();
    }

    public async ValueTask UpdateJobsStatus(
        WorkerJobStatus status,
        CancellationToken cancellationToken = default,
        params WorkerJobEntity[] jobs
    )
    {
        // Update job status
        var updatedJobsCount = await workerJobRepository.UpdateBatchAsync(
            setPropertyCalls => setPropertyCalls.SetProperty(workerJob => workerJob.Status, valueSelector => status),
            workerJob => jobs.Select(job => job.Id).Contains(workerJob.Id),
            cancellationToken
        );
        
        if(updatedJobsCount < jobs.Length)
            throw new InvalidOperationException("Failed to update job status");

        foreach (var job in jobs)
            job.Status = status;
    }

    public async ValueTask PublishJobsAsync(CancellationToken cancellationToken = default, params WorkerJobEntity[] jobs)
    {
        // Create process job events
        var publishJobTasks = jobs.Select(
                job => new ProcessJobEvent
                {
                    Job = job
                }
            )
            .Select(
                async processJobEvent => await eventBusBroker.PublishAsync(
                    processJobEvent, 
                    new EventOptions
                    {
                        Exchange = _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.ExchangeName,
                        RoutingKey = _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.RoutingKey,
                        ReplyTo = _schedulerEventBusSettings.SchedulerOutgoingBusDeclaration.QueueName,
                        CorrelationId = processJobEvent.Id.ToString()
                    },
                    cancellationToken)
            )
            .ToImmutableList();

        await Task.WhenAll(publishJobTasks);
    }
}