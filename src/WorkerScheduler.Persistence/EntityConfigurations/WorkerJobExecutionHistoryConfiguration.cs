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
            .WithMany()
            .HasForeignKey(history => history.JobId);
    }
}