using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace MtCon;

public class SimplePublishService : BackgroundService
{
    private readonly IBus _bus;

    public SimplePublishService(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:order-acknowledged-test"));

        await endpoint.Send(new SubmitOrder
        {
            // OrderId = NewId.NextGuid()
            OrderId = InVar.Id
        }, stoppingToken);
        // await _scheduler.SchedulePublish<SubmitOrder>(DateTime.Now.AddSeconds(30),
        //     new SubmitOrder { OrderId = InVar.Id });
        // await Task.CompletedTask;
    }
}
