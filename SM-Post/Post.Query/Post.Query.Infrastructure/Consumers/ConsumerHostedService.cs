using CQRS.Core.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Post.Query.Infrastructure.Consumers
{
    public class ConsumerHostedService : IHostedService
    {
        private readonly ILogger<ConsumerHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider,IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event Consumer service is running.");
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var eventConsumer = _serviceProvider.GetRequiredService<IEventConsumer>();
                var topic = _configuration["KAFKA_TOPIC"];

                Task.Run(() => {
                    eventConsumer.Consume(topic);
                },cancellationToken);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event Consumer service is stopped.");
            return Task.CompletedTask;
        }
    }
}