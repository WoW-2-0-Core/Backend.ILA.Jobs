using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Persistence.EntityConfigurations;

public class WorkerJobExecutionHistoryConfiguration : IEntityTypeConfiguration<WorkerJobExecutionHistoryEntity>
{
    public void Configure(EntityTypeBuilder<WorkerJobExecutionHistoryEntity> builder)
    {
        builder
            .HasOne(history => history.Job)
            .WithMany(job => job.ExecutionHistories)
            .HasForeignKey(history => history.JobId);
        
        builder
            .HasOne<WorkerJobExecutionHistoryEntity>()
            .WithMany(history => history.RetryHistories)
            .HasForeignKey(history => history.RetryForHistoryId);
    }
}