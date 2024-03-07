using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using WorkerScheduler.Application.Common.EventBus.Brokers;
using WorkerScheduler.Application.Common.Scheduler.Events;
using WorkerScheduler.Application.Common.Scheduler.Services;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Domain.Enums;
using WorkerScheduler.Persistence.Repositories.Interfaces;

namespace WorkerScheduler.Infrastructure.Common.Scheduler;

/// <summary>
/// Provides job scheduler functionality.
/// </summary>
public class JobScheduler(IWorkerJobRepository workerJobRepository, IEventBusBroker eventBusBroker) : IJobScheduler
{
    public async ValueTask<IImmutableList<WorkerJobEntity>> GetScheduledOrFailedJobsAsync(CancellationToken cancellationToken = default)
    {
        // Query all jobs that needs to be run
        var jobs = await workerJobRepository
            .Get(job => job.Status == WorkerJobStatus.Scheduled || job.Status == WorkerJobStatus.Failed)
            .ToListAsync(cancellationToken: cancellationToken);

        // Filter failed jobs and scheduled jobs
        var failedJobEvents = jobs.Where(job => job.Status == WorkerJobStatus.Failed);
        // var scheduledJobs = jobs.Where(job => job.Status == WorkerJobStatus.Scheduled);

        // Publish failed jobs to the queue

        // failedJobEvents
        return failedJobEvents.ToImmutableList();
    }

    public async ValueTask<int> UpdateJobsStatus(
        IImmutableList<WorkerJobEntity> jobs,
        WorkerJobStatus status,
        CancellationToken cancellationToken = default
    )
    {
        // Update job status
        return await workerJobRepository.UpdateBatchAsync(
            setPropertyCalls => setPropertyCalls.SetProperty(workerJob => workerJob.Status, valueSelector => status),
            workerJob => jobs.Any(job => job.Id == workerJob.Id),
            cancellationToken
        );
    }

    public async ValueTask PublishJobsAsync(IImmutableList<WorkerJobEntity> jobs, CancellationToken cancellationToken = default)
    {
        // Create process job events
        var publishJobTasks = jobs
            .Select(
                job => new ProcessJobEvent
                {
                    Job = job
                }
            )
            .Select(
                async processJobEvent => await eventBusBroker.PublishAsync(
                    processJobEvent,
                    "WorkerExchange",
                    "WorkerRoutingKey",
                    cancellationToken
                )
            ).ToImmutableList();
        
        await Task.WhenAll(publishJobTasks);
    }
}