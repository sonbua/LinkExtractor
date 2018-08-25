using System;
using System.Collections.Generic;
using EnsureThat;

namespace ResponsibilityChain
{
    public class CompositeHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        private readonly List<IHandler<TRequest, TResponse>> _handlers;

        public CompositeHandler()
        {
            _handlers = new List<IHandler<TRequest, TResponse>>();
        }

        public virtual TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            EnsureArg.HasItems(_handlers, nameof(_handlers));

            var handler = _handlers.CreatePipelineExecutionDelegate(next);

            return handler.Invoke(request);
        }

        protected void AddHandler(IHandler<TRequest, TResponse> handler)
        {
            EnsureArg.IsNotNull(handler, nameof(handler));

            _handlers.Add(handler);
        }

        protected void AddHandler<THandler>(IServiceProvider serviceProvider)
            where THandler : IHandler<TRequest, TResponse>
        {
            EnsureArg.IsNotNull(serviceProvider, nameof(serviceProvider));

            var handler = (THandler) serviceProvider.GetService(typeof(THandler));

            _handlers.Add(handler);
        }
    }
}