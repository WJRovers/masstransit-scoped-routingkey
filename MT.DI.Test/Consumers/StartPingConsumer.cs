using System.Threading.Tasks;
using MassTransit;
using MT.DI.Test.Messages;

namespace MT.DI.Test.Consumers
{
    public class StartPingConsumer : IConsumer<StartPingRequest>
    {
        public async Task Consume(ConsumeContext<StartPingRequest> context)
        {
            var fooValue = context.Headers.Get<string>("foo");
            await context.Publish<PingRequest>(new
                {
                    Message = context.Message.Message
                }, ctx =>
                    ctx.Headers.Set("foo", fooValue)
            );
        }
    }
}