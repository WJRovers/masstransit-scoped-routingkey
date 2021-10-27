using Autofac;
using GreenPipes;
using MassTransit;
using MassTransit.PublishPipeSpecifications;
using MassTransit.SendPipeSpecifications;
using MT.DI.Test.Infrastructure.Options;

namespace MT.DI.Test.Infrastructure.MassTransit.Observer
{
    public class DecoratorObserver : ISendPipeSpecificationObserver, IPublishPipeSpecificationObserver
    {
        private readonly ILifetimeScope _scope;

        public DecoratorObserver(ILifetimeScope scope) => _scope = scope;

        public void MessageSpecificationCreated<T>(IMessageSendPipeSpecification<T> specification) where T : class
        {
            if (!_scope.IsRegistered<FooHeaderDecoratorFilter<T>>())
                throw new ConfigurationException(
                    "Missing generic FooHeaderDecoratorFilter<> dependency container registration.");

            if (_scope.TryResolve(out FooHeaderDecoratorFilter<T> filter))
                specification.UseFilter(filter);
        }

        public void MessageSpecificationCreated<T>(IMessagePublishPipeSpecification<T> specification) where T : class
        {
            if (!_scope.IsRegistered<FooHeaderDecoratorFilter<T>>())
                throw new ConfigurationException(
                    "Missing generic FooHeaderDecoratorFilter<> dependency container registration.");

            if (_scope.TryResolve(out FooHeaderDecoratorFilter<T> filter))
                specification.UseFilter(filter);
        }
    }
}