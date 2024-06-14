using MassTransit;
using Microsoft.Extensions.Options;

namespace MasstransitKafkaTester;

public class PingConsumer: IConsumer<Ping>
{
    private readonly ILogger<PingConsumer> _logger;
    private readonly ServiceSettings _settings;

    public PingConsumer(ILogger<PingConsumer> logger, IOptions<ServiceSettings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task Consume(ConsumeContext<Ping> context)
    {
        var ping = context.Message;

        if (ping.LaunchId != _settings.LaunchId)
            return;
        
        _logger.LogInformation(" < Recv: launchId={LaunchId} seqId={SeqId} createdAt={CreatedAt}", ping.LaunchId, ping.SeqId, ping.CreatedAt);
    }
}