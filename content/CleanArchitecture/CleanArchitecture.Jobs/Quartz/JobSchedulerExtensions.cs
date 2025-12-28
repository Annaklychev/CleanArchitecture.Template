using CleanArchitecture.Jobs.Jobs;
using Quartz;
using CronExpression = CleanArchitecture.Jobs.Quartz;

namespace CleanArchitecture.Jobs.Quartz;

public static class JobSchedulerExtensions
{
    public static IServiceCollection AddQuartzJobs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<QuartzOptions>(
            configuration.GetSection("Quartz"));

        services.AddQuartz(q =>
        {
            q.AddJob<DemoJob>(opts =>
                opts.WithIdentity(nameof(DemoJob)));

            q.AddTrigger(opts => opts
                .ForJob(nameof(DemoJob))
                .WithIdentity($"{nameof(DemoJob)}-trigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(10)
                    .RepeatForever()));
            //.WithCronSchedule(CronExpressions.MinuteInterval(10)));
        });

        services.AddQuartzHostedService(q =>
        {
            q.WaitForJobsToComplete = true;
        });

        return services;
    }
}