using CleanArchitecture.Application;
using CleanArchitecture.Jobs.Quartz;
using NLog;
using NLog.Extensions.Logging;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureServices((ctx, services) =>
    {
        //services.AddApplication();
        services.AddQuartzJobs(ctx.Configuration);
    });

    builder.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddNLog();
    });

    var host = builder.Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    logger.Error(ex, "Jobs host terminated unexpectedly");
    throw;
}
finally
{
    LogManager.Shutdown();
}