using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Schedulers.Events;
using WorkerScheduler.Application.Common.Schedulers.Services;
using WorkerScheduler.Application.Common.WorkerJobExecutionHistories.Services;
using WorkerScheduler.Application.Common.WorkerJobs.Services;
using WorkerScheduler.Domain.Common.Events;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Domain.Enums;
using WorkerScheduler.Infrastructure.Common.Schedulers.Settings;

namespace WorkerScheduler.Infrastructure.Common.Schedulers.Services;

/// <summary>
/// Provides job scheduler functionality.
/// </summary>
public class JobSchedulerService(
    IOptions<SchedulerEventBusSettings> schedulerEventBusSettings,
    IWorkerJobService workerJobRepository, 
    IWorkerJobExecutionHistoryService workerJobExecutionHistoryService,
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

    public async ValueTask UpdateJobsStatusAsync(
        WorkerJobStatus status,
        CancellationToken cancellationToken = default,
        params WorkerJobEntity[] jobs
    )
    {
        // Update job status
        var updatedJobsCount = await UpdateJobsStatusAsync(status, cancellationToken, jobs.Select(job => job.Id).ToArray());
        
        if(updatedJobsCount < jobs.Length)
            throw new InvalidOperationException("Failed to update job status");

        foreach (var job in jobs)
            job.Status = status;
    }
    
    private async ValueTask<int> UpdateJobsStatusAsync(
        WorkerJobStatus status,
        CancellationToken cancellationToken = default,
        params Guid[] jobsId
    )
    {
        // Update job status
        var updatedJobsCount = await workerJobRepository.UpdateBatchAsync(
            setPropertyCalls => setPropertyCalls.SetProperty(workerJob => workerJob.Status, valueSelector => status),
            workerJob => jobsId.Contains(workerJob.Id),
            cancellationToken
        );

        return updatedJobsCount;
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
                        CorrelationId = processJobEvent.Id.ToString()
                    },
                    cancellationToken)
            )
            .ToImmutableList();

        await Task.WhenAll(publishJobTasks);
    }

    public async ValueTask CreateJobHistoryAsync(WorkerJobExecutionHistoryEntity history, CancellationToken cancellationToken = default)
    {
        await UpdateJobsStatusAsync(history.IsSuccessful ? WorkerJobStatus.Scheduled : WorkerJobStatus.Failed, cancellationToken, history.JobId);
        await workerJobExecutionHistoryService.CreateAsync(history, cancellationToken);
    }
}