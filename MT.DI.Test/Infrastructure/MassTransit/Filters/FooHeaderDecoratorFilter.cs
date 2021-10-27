using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MT.DI.Test.Provider;

namespace MT.DI.Test.Infrastructure.Options
{
    public class FooHeaderDecoratorFilter<TMessage> :
        IFilter<SendContext<TMessage>>,
        IFilter<PublishContext<TMessage>>
        where TMessage : class
    {
        private readonly IContextProvider _contextProvider;

        public FooHeaderDecoratorFilter(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public async Task Send(SendContext<TMessage> context, IPipe<SendContext<TMessage>> next)
        {
            await InternalSend(context, async () => await next.Send(context));
        }

        public async Task Send(PublishContext<TMessage> context, IPipe<PublishContext<TMessage>> next)
        {
            await InternalSend(context, async () => await next.Send(context));
        }

        private async Task InternalSend(SendContext context, Func<Task> nextAction)
        {
            var fooValue = _contextProvider.GetHeader();
            context.Headers.Set("foo", fooValue);

            await nextAction();
        }

        public void Probe(ProbeContext context) => context.CreateFilterScope("foo-decorator");
    }
}