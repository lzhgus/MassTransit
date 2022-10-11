using System;
using Microsoft.Extensions.Logging;

namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class OrderAcknowledgedConsumer :
        IConsumer<SubmitOrder>
    {
        private readonly ILogger<OrderAcknowledgedConsumer> _logger;

        public OrderAcknowledgedConsumer(ILogger<OrderAcknowledgedConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            _logger.LogInformation("Received message: {text}", context.Message.OrderId.ToString());
            Uri notification = new Uri("queue:scheduler");

            await context.ScheduleSend<SubmitOrder>(notification, DateTime.Now.AddMinutes(1), context.Message);
        }
    }
}
