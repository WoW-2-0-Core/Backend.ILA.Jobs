using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quartz;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Persistence.EntityConfigurations;

public class WorkerJobConfiguration : IEntityTypeConfiguration<WorkerJobEntity>
{
    public void Configure(EntityTypeBuilder<WorkerJobEntity> builder)
    {
        builder
            .Property(job => job.CronSchedule)
            .HasConversion<string>(
                schedule => schedule.CronExpressionString,
                schedule => new CronExpression(schedule)
            )
            .HasMaxLength(128)
            .HasDefaultValue(new CronExpression("0/10 * * * * ?"))
            .IsRequired();
        
        builder
            .Property(job => job.JobType)
            .HasMaxLength(256)
            .IsRequired();
        
        builder
            .HasIndex(job => job.JobType)
            .IsUnique();

        builder
            .Property(job => job.Status)
            .HasConversion<string>()
            .HasMaxLength(64);
    }
}