using Quartz;

namespace CleanArchitecture.Jobs.Jobs;

public sealed class DemoJob : IJob
{
    private readonly ILogger<DemoJob> _logger;

    public DemoJob(ILogger<DemoJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("DemoJob executed");
        return Task.CompletedTask;
    }
}