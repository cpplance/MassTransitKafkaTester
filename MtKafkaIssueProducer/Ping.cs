namespace MtKafkaIssueProducer;

public class Ping
{
    public required string LaunchId { get; set; }
    public required int SeqId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int Key => CreatedAt.GetHashCode();
}