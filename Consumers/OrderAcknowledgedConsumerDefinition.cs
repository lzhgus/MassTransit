namespace Company.Consumers
{
    using MassTransit;

    public class OrderAcknowledgedConsumerDefinition :
        ConsumerDefinition<OrderAcknowledgedConsumer>
    {
        public OrderAcknowledgedConsumerDefinition()
        {
            EndpointName = "order-acknowledged-test";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<OrderAcknowledgedConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}
