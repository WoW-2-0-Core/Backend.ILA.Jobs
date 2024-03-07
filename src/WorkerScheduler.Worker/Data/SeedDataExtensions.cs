using Microsoft.EntityFrameworkCore;
using Quartz;
using WorkerScheduler.Domain.Entities;
using WorkerScheduler.Persistence.DataContexts;

namespace WorkerScheduler.Worker.Data;

/// <summary>
/// Extension methods for seeding data into the database.
/// </summary>
public static class SeedDataExtensions
{
    /// <summary>
    /// Initializes the seed data asynchronously.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async ValueTask InitializeSeedAsync(this IServiceProvider serviceProvider)
    {
        var appDbContext = serviceProvider.GetRequiredService<WorkerDbContext>();

        if (!await appDbContext.Jobs.AnyAsync())
            await appDbContext.SeedJobsAsync();
    }
    
    private static async ValueTask SeedJobsAsync(this WorkerDbContext appDbContext)
    {
        var clients = new List<WorkerJobEntity>
        {
            new()
            {
                JobType = "Check PreOrders",
                CronSchedule = new CronExpression("0/10 * * * * ?")
            },
            new()
            {
                JobType = "Check Deliveries",
                CronSchedule = new CronExpression("0/20 * * * * ?")
            }
        };

        await appDbContext.Jobs.AddRangeAsync(clients);
        await appDbContext.SaveChangesAsync();
    }
}