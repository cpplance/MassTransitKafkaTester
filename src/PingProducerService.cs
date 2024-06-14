using Confluent.Kafka;
using MassTransit;
using Microsoft.Extensions.Options;

namespace MasstransitKafkaTester;

public class PingProducerService
{
    private readonly ITopicProducer<int, Ping> _pingTopicProducer;
    private readonly ILogger<PingProducerService> _logger;
    private readonly ServiceSettings _serviceSettings;
    
    public PingProducerService(ITopicProducer<int, Ping> pingTopicProducer, ILogger<PingProducerService> logger, IOptions<ServiceSettings> options)
    {
        _pingTopicProducer = pingTopicProducer;
        _logger = logger;
        _serviceSettings = options.Value;
    }

    public async Task RunAsync(int seqId, ServiceSettings settings, CancellationToken cancellationToken)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        Ping ping = new()
        {
            LaunchId = settings.LaunchId,
            SeqId = seqId,
            CreatedAt = utcNow
        };

        for (int i = 0; i < _serviceSettings.SendCount; i++)
        {
            try
            {
                await _pingTopicProducer.Produce(ping.Key, ping, cancellationToken);
                _logger.LogInformation("> Sent: launchId={LaunchId} seqId={SeqId} createdAt={CreatedAt}", ping.LaunchId, ping.SeqId, ping.CreatedAt);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "> Sent: FAILED launchId={LaunchId} seqId={SeqId} createdAt={CreatedAt}", ping.LaunchId, ping.SeqId, ping.CreatedAt);
            }
        }
        
        await Task.Delay(_serviceSettings.SendDelay, cancellationToken);
    }
}