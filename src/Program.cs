using Confluent.Kafka;
using MassTransit;
using MasstransitKafkaTester;

string topicName = "pings-new";
string consumerGroupId = "ping-service";
string kafkaHost = "localhost:9092";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


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
            });
            
            k.Host(kafkaHost);
        });

        rider.AddProducer<int, Ping>(topicName, c => c.Message.Key);
        rider.AddConsumers(typeof(PingConsumer).Assembly);
    });
});

services.AddScoped<PingProducerService>();
services.AddHostedService<PingProducerBackgroundService>();

builder.Build().Run();