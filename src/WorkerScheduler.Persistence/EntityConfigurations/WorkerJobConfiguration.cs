using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCrontab;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Persistence.EntityConfigurations;

public class WorkerJobConfiguration : IEntityTypeConfiguration<WorkerJobEntity>
{
    public void Configure(EntityTypeBuilder<WorkerJobEntity> builder)
    {
        builder
            .Property(job => job.CronSchedule)
            .HasConversion<string>(
                schedule => schedule.ToString(),
                schedule => CrontabSchedule.Parse(schedule)
            )
            .HasMaxLength(128)
            .HasDefaultValue(CrontabSchedule.Parse("0 0 * * *"))
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