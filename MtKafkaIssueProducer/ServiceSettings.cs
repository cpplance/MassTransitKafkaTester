namespace MtKafkaIssueProducer;

public class ServiceSettings
{
    public string LaunchId { get; set; } = Guid.NewGuid().ToString().GetHashCode().ToString();
    public TimeSpan SendDelay { get; set; } = TimeSpan.FromSeconds(1);
    public int SendCount { get; set; } = 32;
}