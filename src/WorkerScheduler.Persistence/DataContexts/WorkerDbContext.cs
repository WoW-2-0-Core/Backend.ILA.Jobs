using Microsoft.EntityFrameworkCore;
using WorkerScheduler.Domain.Entities;

namespace WorkerScheduler.Persistence.DataContexts;

public class WorkerDbContext(DbContextOptions<WorkerDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<WorkerJobEntity> Jobs => Set<WorkerJobEntity>();
    
    public DbSet<WorkerJobExecutionHistoryEntity> JobExecutionHistories => Set<WorkerJobExecutionHistoryEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkerDbContext).Assembly);
    }
}