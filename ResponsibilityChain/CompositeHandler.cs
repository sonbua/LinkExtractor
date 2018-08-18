using System;
using System.Collections.Generic;
using EnsureThat;

namespace ResponsibilityChain
{
    public class CompositeHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        protected readonly List<IHandler<TRequest, TResponse>> Handlers;

        public CompositeHandler()
        {
            Handlers = new List<IHandler<TRequest, TResponse>>();
        }

        public virtual TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            EnsureArg.HasItems(Handlers, nameof(Handlers));

            var handler = Handlers.CreatePipelineExecutionDelegate(next);

            return handler.Invoke(request);
        }

        protected void AddHandler(IHandler<TRequest, TResponse> handler)
        {
            EnsureArg.IsNotNull(handler, nameof(handler));

            Handlers.Add(handler);
        }

        protected void AddHandler<THandler>(IServiceProvider serviceProvider)
            where THandler : IHandler<TRequest, TResponse>
        {
            EnsureArg.IsNotNull(serviceProvider, nameof(serviceProvider));

            var handler = (THandler) serviceProvider.GetService(typeof(THandler));

            Handlers.Add(handler);
        }
    }
}