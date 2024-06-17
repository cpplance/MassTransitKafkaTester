using MassTransit;
using Microsoft.Extensions.Options;

namespace MtKafkaIssueConsumer;

public class PingConsumer: IConsumer<Ping>
{
    private readonly ILogger<PingConsumer> _logger;

    public PingConsumer(ILogger<PingConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<Ping> context)
    {
        var ping = context.Message;
        _logger.LogInformation(" < Recv: launchId={LaunchId} seqId={SeqId} createdAt={CreatedAt}", ping.LaunchId, ping.SeqId, ping.CreatedAt);
    }
}