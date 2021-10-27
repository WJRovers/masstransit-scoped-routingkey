using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport.Contexts;
using Microsoft.Extensions.Logging;
using MT.DI.Test.Messages;
using MT.DI.Test.Provider;

namespace MT.DI.Test.Consumers
{
    public class PingMessageConsumer : IConsumer<PingRequest>
    {
        private readonly IContextProvider _contextProvider;
        private readonly ILogger<PingMessageConsumer> _logger;

        public PingMessageConsumer(
            IContextProvider contextProvider, 
            ILogger<PingMessageConsumer> logger
        )
        {
            _contextProvider = contextProvider;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PingRequest> context)
        {
            string headerValue = _contextProvider.GetHeader();

            if (context.ReceiveContext is RabbitMqReceiveContext rabbitMqReceiveContext)
                _logger.LogInformation("The routing key set is {RoutingKey}", rabbitMqReceiveContext.RoutingKey);
            
            await context.RespondAsync<PongResponse>(new
            {
                Message = $"The received header value was '{headerValue}'"
            });
        }
    }
}