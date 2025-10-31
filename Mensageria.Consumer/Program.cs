using Mensageria.Consumer.BackgroundServices;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<MessageConsumerWorker>();

var host = builder.Build();
host.Run();