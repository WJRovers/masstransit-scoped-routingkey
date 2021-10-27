using System;
using Autofac;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport.Topology;
using Microsoft.Extensions.Options;
using MT.DI.Test.Consumers;
using MT.DI.Test.Infrastructure.MassTransit.Observer;
using MT.DI.Test.Infrastructure.MassTransit.RoutingKeyFormatter;
using MT.DI.Test.Infrastructure.Options;
using MT.DI.Test.Messages;
using MT.DI.Test.Provider;
using Module = Autofac.Module;

namespace MT.DI.Test.Infrastructure.Autofac
{
    public class MassTransitModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // By default we have a HttpContextProvider
            builder.RegisterType<HttpContextProvider>().As<IContextProvider>();

            builder.RegisterGeneric(typeof(FooHeaderDecoratorFilter<>));
            builder.RegisterType<DecoratorObserver>().AsSelf();

            builder.RegisterType<PingMessageRoutingKeyFormatter>().As<IMessageRoutingKeyFormatter<PingRequest>>();

            builder.AddMassTransit(x =>
            {
                x.ConfigureScope = (containerBuilder, _) =>
                {
                    // Withing the scope of consuming message we use the ConsumeContextProvider
                    containerBuilder.RegisterType<ConsumeContextProvider>().As<IContextProvider>();
                };
                
                // Configure RabbitMq based of some config file
                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetService<IOptionsMonitor<EventBusOptions>>().CurrentValue;
                    cfg.Host(new Uri(options.ConnectionString), host =>
                    {
                        host.Username(options.UserName);
                        host.Password(options.Password);
                    });
                    cfg.PrefetchCount = options.PrefetchCount;
                    cfg.UseMessageRetry(retry => retry.Immediate(options.RetryCount));
                    
                    // Configure endpoints by convention
                    cfg.ConfigureEndpoints(context);

                    var obs = context.GetRequiredService<DecoratorObserver>();
                    cfg.ConfigurePublish((c) => c.ConnectPublishPipeSpecificationObserver(obs));
                    cfg.ConfigureSend((c) => c.ConnectSendPipeSpecificationObserver(obs));

                    cfg.Send<PingRequest>(configure =>
                    {
                        configure.UseRoutingKeyFormatter(
                            context.GetRequiredService<IMessageRoutingKeyFormatter<PingRequest>>()
                        );
                    });
                });
                
                x.AddRequestClient<PingRequest>();
                x.AddConsumer<StartPingConsumer>();
                x.AddConsumer<PingMessageConsumer>();
            });
        }
    }
}