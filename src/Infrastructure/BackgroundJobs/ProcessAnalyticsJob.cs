using Application.Abstractions.Messaging;
using Application.Analytics;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.BackgroundJobs;

public class ProcessAnalyticsJob : IJob
{
    private readonly ICommandHandler<ProcessAnalyticsCommand> _handler;
    private readonly ILogger<ProcessAnalyticsJob> _logger;

    public ProcessAnalyticsJob(
        ICommandHandler<ProcessAnalyticsCommand> handler,
        ILogger<ProcessAnalyticsJob> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Procesando analytics (ayer + hoy)...");

        await _handler.Handle(
            new ProcessAnalyticsCommand(),
            context.CancellationToken);

        _logger.LogInformation("Analytics procesado para ayer y hoy");
    }
}