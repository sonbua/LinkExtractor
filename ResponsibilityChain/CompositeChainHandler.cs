using System;
using System.Collections.Generic;
using System.Linq;

namespace ResponsibilityChain
{
    public abstract class CompositeChainHandler<TRequest, TResponse> : IChainHandler<TRequest, TResponse>
    {
        private const string _CANNOT_HANDLE = "{0} cannot handle this request. Request information: {1}";

        private readonly IList<IChainHandler<TRequest, TResponse>> _handlers;
        private readonly IServiceProvider _serviceProvider;

        protected CompositeChainHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _handlers = new List<IChainHandler<TRequest, TResponse>>();
        }

        public bool CanHandle(TRequest request) => _handlers.Any(handler => handler.CanHandle(request));

        public TResponse Handle(TRequest request)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(request))
                {
                    return handler.Handle(request);
                }
            }

            throw new NotSupportedException(
                string.Format(_CANNOT_HANDLE, nameof(CompositeChainHandler<TRequest, TResponse>), request)
            );
        }

        protected void AddHandler<THandler>()
            where THandler : IChainHandler<TRequest, TResponse>
        {
            var handler = (THandler) _serviceProvider.GetService(typeof(THandler));

            _handlers.Add(handler);
        }
    }
}