using System.Threading.Tasks;
using MassTransit;
using MT.DI.Test.Messages;
using MT.DI.Test.Provider;

namespace MT.DI.Test.Consumers
{
    public class PingMessageConsumer : IConsumer<PingRequest>
    {
        private readonly IContextProvider _contextProvider;

        public PingMessageConsumer(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public async Task Consume(ConsumeContext<PingRequest> context)
        {
            string headerValue = _contextProvider.GetHeader();
            await context.RespondAsync<PongResponse>(new
            {
                Message = $"The received header value was '{headerValue}'"
            });
        }
    }
}