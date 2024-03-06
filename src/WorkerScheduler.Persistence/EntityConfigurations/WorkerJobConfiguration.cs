using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Persistence.EntityConfigurations;

public class WorkerJobConfiguration : IEntityTypeConfiguration<WorkerJobEntity>
{
    public void Configure(EntityTypeBuilder<WorkerJobEntity> builder)
    {
        builder
            .Property(job => job.CronSchedule)
            .HasMaxLength(128)
            .HasDefaultValue("0 0 * * *")
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