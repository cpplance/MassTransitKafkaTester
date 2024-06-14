using Microsoft.Extensions.Options;

namespace MasstransitKafkaTester;

public class PingProducerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PingProducerBackgroundService> _logger;
    private readonly ServiceSettings _serviceSettings;
    private int SeqId { get; set; }

    public PingProducerBackgroundService(IServiceProvider serviceProvider, ILogger<PingProducerBackgroundService> logger, IOptions<ServiceSettings> serviceOptions)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _serviceSettings = serviceOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Starting new cycle");
            
            using var serviceScope = _serviceProvider.CreateScope();
            PingProducerService service = serviceScope.ServiceProvider.GetRequiredService<PingProducerService>();
            await service.RunAsync(SeqId++, _serviceSettings, cancellationToken);
      
            await Task.Delay(_serviceSettings.SendDelay, cancellationToken);
        }
    }
}