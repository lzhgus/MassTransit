using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Company.Consumers;
using Contracts;
using Hangfire;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.SqlServer;
using Hangfire.MemoryStorage;
using MassTransit.HangfireIntegration;
using Microsoft.Extensions.Configuration;
using Scheduler.Service;

namespace MtCon;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddCommandLine(args);
            })
            .ConfigureServices((_, services) =>
            {
                services.AddHangfire(config =>
                {
                    config.UseMemoryStorage();
                });

                services.AddSingleton<IHangfireComponentResolver, ServiceProviderHangfireComponentResolver>();
                services.AddHangfireServer();
                ConfigureMassTransit(services);
                services.AddHostedService<SimplePublishService>();
            });
    }

    static void ConfigureMassTransit(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            // x.SetKebabCaseEndpointNameFormatter();
            x.AddMessageScheduler(new Uri("queue:hangfire"));
            x.AddConsumer<OrderAcknowledgedConsumer, OrderAcknowledgedConsumerDefinition>();
            x.AddConsumer<schedulerConsumer, schedulerConsumerDefinition>();
                // .Endpoint(e =>
                // {
                //     e.Name = "order-acknowledged-test";
                // });
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseHangfireScheduler(context.GetRequiredService<IHangfireComponentResolver>(),"hangfire");
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
