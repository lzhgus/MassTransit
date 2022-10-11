namespace Company.Consumers
{
    using MassTransit;

    public class schedulerConsumerDefinition :
        ConsumerDefinition<schedulerConsumer>
    {
        public schedulerConsumerDefinition()
        {
            EndpointName = "scheduler";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<schedulerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}
