using System;
using System.Collections.Generic;
using System.Linq;

namespace ResponsibilityChain
{
    public abstract class CompositeChainHandler<TRequest, TResponse> : IChainHandler<TRequest, TResponse>
    {
        private const string _CANNOT_HANDLE = "{0} cannot handle this request. Request information: {1}";

        protected readonly ICollection<IChainHandler<TRequest, TResponse>> Handlers;

        protected CompositeChainHandler()
        {
            Handlers = new List<IChainHandler<TRequest, TResponse>>();
        }

        public virtual bool CanHandle(TRequest request) => Handlers.Any(handler => handler.CanHandle(request));

        public virtual TResponse Handle(TRequest request)
        {
            foreach (var handler in Handlers)
            {
                if (handler.CanHandle(request))
                {
                    return handler.Handle(request);
                }
            }

            throw new NotSupportedException(
                string.Format(_CANNOT_HANDLE, this.GetType(), request)
            );
        }

        protected void AddHandler(IChainHandler<TRequest, TResponse> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Handlers.Add(handler);
        }

        protected void AddHandler<THandler>(IServiceProvider serviceProvider)
            where THandler : IChainHandler<TRequest, TResponse>
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var handler = (THandler) serviceProvider.GetService(typeof(THandler));

            Handlers.Add(handler);
        }
    }
}