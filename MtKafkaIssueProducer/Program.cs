using MassTransit;
using MtKafkaIssueProducer;

string topicName = "pings-new";
string kafkaHost = "localhost:9092";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddOptions<ServiceSettings>();
services.AddMassTransit(x =>
{
    x.UsingInMemory();

    x.AddRider(rider =>
    {
        rider.UsingKafka((_, k) =>
        {
            k.Host(kafkaHost);
        });

        rider.AddProducer<int, Ping>(topicName, c => c.Message.Key);
    });
});

services.AddScoped<PingProducerService>();
services.AddHostedService<PingProducerBackgroundService>();

builder.Build().Run();