using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Application.Common.Workers.Services;

/// <summary>
/// Defines task executor interface
/// </summary>
public interface ITaskExecutor
{
    /// <summary>
    /// Executes given job
    /// </summary>
    /// <param name="job">Job to execute</param>
    ValueTask<bool> ExecuteAsync(WorkerJobEntity job);
}