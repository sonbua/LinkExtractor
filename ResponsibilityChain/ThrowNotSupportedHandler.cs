using System;

namespace ResponsibilityChain
{
    /// <summary>
    /// A handler that throws <see cref="NotSupportedException"/>. This is usually set as the last handler in the chain.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class ThrowNotSupportedHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        static ThrowNotSupportedHandler()
        {
        }

        private ThrowNotSupportedHandler()
        {
        }

        /// <summary>
        /// Singleton instance of this handler.
        /// </summary>
        public static IHandler<TRequest, TResponse> Instance { get; } =
            new ThrowNotSupportedHandler<TRequest, TResponse>();

        /// <summary>
        /// Throws <see cref="NotSupportedException"/> on invocation.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            throw new NotSupportedException(
                $"Cannot handle this request. Request information: {typeof(TRequest)}"
            );
        }
    }
}