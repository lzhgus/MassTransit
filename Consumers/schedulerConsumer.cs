using Microsoft.Extensions.Logging;

namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class schedulerConsumer :
        IConsumer<SubmitOrder>
    {
        private ILogger<schedulerConsumer> _logger;

        public schedulerConsumer(ILogger<schedulerConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SubmitOrder> context)
        {
            _logger.LogInformation("received message after 1 mins");
            return Task.CompletedTask;
        }
    }
}
