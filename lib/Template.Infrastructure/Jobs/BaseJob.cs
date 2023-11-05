using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Template.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public abstract class BaseJob<T> : IJob, IConfigureOptions<QuartzOptions> 
    where T : BaseJob<T>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    protected BaseJob(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        using var scopeCronJob = _logger.BeginScope("CronJob");

        var jobName = GetType().FullName ?? "Unknown Job";
        using var scopeJobName = _logger.BeginScope(jobName);

        var jobId = Guid.NewGuid().ToString();
        using var scopeJobId = _logger.BeginScope(jobId);
        try
        {
            _logger.LogInformation("Starting Job {JobId}", jobId);
            var command = GetCommand();

            var response = await _mediator.Send(command, context.CancellationToken);

            _logger.LogInformation("Completed {JobId}: {@Response}", jobId, response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed Job {JobId}", jobId);
        }
    }

    public void Configure(QuartzOptions options)
    {
        var jobName = typeof(T).FullName ?? "Unknown Job";
        var jobKey = JobKey.Create(jobName);
        
        var cronSchedule = GetCronSchedule();
        if (string.IsNullOrWhiteSpace(cronSchedule))
        {
            // schedule it for the distant future so it never runs
            cronSchedule = $"0 0 23 1 1 ? {DateTime.UtcNow.Year + 100}";
        }
        
        options
            .AddJob<T>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithIdentity($"{jobName}-trigger")
                .WithCronSchedule(cronSchedule));
    }

    protected abstract IBaseRequest GetCommand();

    protected abstract string GetCronSchedule();
}
