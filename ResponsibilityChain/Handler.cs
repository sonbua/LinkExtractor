using System;
using System.Collections.Generic;
using EnsureThat;

namespace ResponsibilityChain
{
    /// <summary>
    /// <para>Represents a composite handler, that comprises of multiple handlers in order to serve a more complicate request.</para>
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public abstract class Handler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        private readonly List<IHandler<TRequest, TResponse>> _handlers;

        /// <summary>
        /// </summary>
        protected Handler()
        {
            _handlers = new List<IHandler<TRequest, TResponse>>();
        }

        /// <summary>
        /// Builds a chained delegate from the list of handlers.
        /// </summary>
        private Func<Func<TRequest, TResponse>, Func<TRequest, TResponse>> ChainedDelegate
        {
            get
            {
                Func<Func<TRequest, TResponse>, Func<TRequest, TResponse>> chainedDelegate = next => next;

                for (var index = _handlers.Count - 1; index >= 0; index--)
                {
                    var handler = _handlers[index];
                    var chainedDelegateCloned = chainedDelegate;

                    chainedDelegate = next => request => handler.Handle(request, chainedDelegateCloned(next));
                }

                return chainedDelegate;
            }
        }

        /// <summary>
        /// <para>Invokes handlers one by one until the request has been processed by a handler and returns response, ignoring the rest of the handlers.</para>
        /// <para>It is done by first creating a pipeline execution delegate from existing handlers then invoking that delegate against the request.</para>
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="next">The next handler in the chain. If null is provided, <see cref="ThrowNotSupportedHandler{TRequest,TResponse}"/> will be set as the end of the chain.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if handler list is null.</exception>
        /// <exception cref="ArgumentException">Thrown if handler list is empty.</exception>
        public virtual TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            EnsureArg.HasItems(_handlers, nameof(_handlers));

            if (next == null)
            {
                next = aRequest => ThrowNotSupportedHandler<TRequest, TResponse>.Instance.Handle(aRequest, null);
            }

            return ChainedDelegate.Invoke(next).Invoke(request);
        }

        /// <summary>
        /// Adds a handler instance to the last position in the chain.
        /// </summary>
        /// <param name="handler">The handler object.</param>
        protected void AddHandler(IHandler<TRequest, TResponse> handler)
        {
            EnsureArg.IsNotNull(handler, nameof(handler));

            _handlers.Add(handler);
        }

        /// <summary>
        /// Uses <paramref name="serviceProvider"/> to locate a handler instance of type <typeparamref name="THandler"/>, which implements <see cref="IHandler{TRequest,TResponse}"/>, and then adds it to the last position in the chain.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <typeparam name="THandler">The handler type, which implements <see cref="IHandler{TRequest,TResponse}"/></typeparam>
        protected void AddHandler<THandler>(IServiceProvider serviceProvider)
            where THandler : IHandler<TRequest, TResponse>
        {
            EnsureArg.IsNotNull(serviceProvider, nameof(serviceProvider));

            var handler = (THandler) serviceProvider.GetService(typeof(THandler));

            _handlers.Add(handler);
        }
    }
}