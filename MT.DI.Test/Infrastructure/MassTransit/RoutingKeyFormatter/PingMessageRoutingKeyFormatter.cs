using MassTransit.RabbitMqTransport;
using MassTransit.RabbitMqTransport.Topology;
using MT.DI.Test.Messages;
using MT.DI.Test.Provider;

namespace MT.DI.Test.Infrastructure.MassTransit.RoutingKeyFormatter
{
    public class PingMessageRoutingKeyFormatter : IMessageRoutingKeyFormatter<PingRequest>
    {
        // We want this to be scoped per context
        private readonly IContextProvider _provider;

        public PingMessageRoutingKeyFormatter(IContextProvider provider)
        {
            _provider = provider;
        }

        public string FormatRoutingKey(RabbitMqSendContext<PingRequest> context)
        {
            return _provider.GetHeader();
        }
    }
}