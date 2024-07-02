using Confluent.Kafka;
using MassTransit;
using MtKafkaIssueConsumer;

string topicName = "pings-new";
string consumerGroupId = "ping-service";
string kafkaHost = "localhost:9092";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddOptions<ServiceSettings>();
services.AddMassTransit(x =>
{
    x.UsingInMemory();

    x.AddRider(rider =>
    {
        rider.UsingKafka((context, k) =>
        {
            k.TopicEndpoint<Ping>(topicName, consumerGroupId, c =>
            {
                c.AutoOffsetReset = AutoOffsetReset.Latest;
                c.ConcurrentConsumerLimit = 20;
                c.ConfigureConsumer<PingConsumer>(context);
                c.CreateIfMissing();
            });
            
            k.Host(kafkaHost);
        });
        rider.AddConsumers(typeof(PingConsumer).Assembly);
    });
});

builder.Build().Run();