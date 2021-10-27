using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MT.DI.Test.Messages;

namespace MT.DI.Test.Consumers
{
    public class StartPingConsumer : IConsumer<StartPingRequest>
    {
        private readonly ILogger<StartPingRequest> _logger;

        public StartPingConsumer(ILogger<StartPingRequest> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StartPingRequest> context)
        {
            var fooValue = context.Headers.Get<string>("foo");
            
            _logger.LogInformation("Starting a ping request while the header current has a value of {FoodHeaderValue}", fooValue);
            await context.Publish<PingRequest>(new {Message = context.Message.Message});
        }
    }
}