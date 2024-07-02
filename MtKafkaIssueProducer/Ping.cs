namespace MtKafkaIssueProducer;

public class Ping
{
    public required string LaunchId { get; set; }
    public required int SeqId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int Key => Random.Shared.Next(int.MinValue, int.MaxValue);
}