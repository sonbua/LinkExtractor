using System;
using System.Collections.Generic;
using EnsureThat;

namespace ResponsibilityChain
{
    /// <summary>
    /// <para>Represents a composite handler, that comprises of multiple handlers in order to serve a more complicate input.</para>
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    public abstract class Handler<TIn, TOut> : IHandler<TIn, TOut>
    {
        private readonly List<IHandler<TIn, TOut>> _handlers;

        /// <summary>
        /// </summary>
        protected Handler()
        {
            _handlers = new List<IHandler<TIn, TOut>>();
        }

        /// <summary>
        /// Builds a chained delegate from the list of handlers.
        /// </summary>
        private Func<Func<TIn, TOut>, Func<TIn, TOut>> ChainedDelegate
        {
            get
            {
                Func<Func<TIn, TOut>, Func<TIn, TOut>> chainedDelegate = next => next;

                for (var index = _handlers.Count - 1; index >= 0; index--)
                {
                    var handler = _handlers[index];
                    var chainedDelegateCloned = chainedDelegate;

                    chainedDelegate = next => input => handler.Handle(input, chainedDelegateCloned(next));
                }

                return chainedDelegate;
            }
        }

        /// <summary>
        /// <para>Invokes handlers one by one until the input has been processed by a handler and returns output, ignoring the rest of the handlers.</para>
        /// <para>It is done by first creating a pipeline execution delegate from existing handlers then invoking that delegate against the input.</para>
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <param name="next">The next handler in the chain. If null is provided, <see cref="ThrowNotSupportedHandler{TIn,TOut}"/> will be set as the end of the chain.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if handler list is null.</exception>
        /// <exception cref="ArgumentException">Thrown if handler list is empty.</exception>
        public virtual TOut Handle(TIn input, Func<TIn, TOut> next)
        {
            EnsureArg.HasItems(_handlers, nameof(_handlers));

            if (next == null)
            {
                next = anInput => ThrowNotSupportedHandler<TIn, TOut>.Instance.Handle(anInput, null);
            }

            return ChainedDelegate.Invoke(next).Invoke(input);
        }

        /// <summary>
        /// Adds a handler instance to the last position in the chain.
        /// </summary>
        /// <param name="handler">The handler object.</param>
        protected void AddHandler(IHandler<TIn, TOut> handler)
        {
            EnsureArg.IsNotNull(handler, nameof(handler));

            _handlers.Add(handler);
        }

        /// <summary>
        /// Uses <paramref name="serviceProvider"/> to locate a handler instance of type <typeparamref name="THandler"/>, which implements <see cref="IHandler{TIn,TOut}"/>, and then adds it to the last position in the chain.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <typeparam name="THandler">The handler type, which implements <see cref="IHandler{TIn,TOut}"/></typeparam>
        protected void AddHandler<THandler>(IServiceProvider serviceProvider)
            where THandler : IHandler<TIn, TOut>
        {
            EnsureArg.IsNotNull(serviceProvider, nameof(serviceProvider));

            var handler = (THandler) serviceProvider.GetService(typeof(THandler));

            _handlers.Add(handler);
        }
    }
}