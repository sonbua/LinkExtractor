using System;

namespace ResponsibilityChain
{
    /// <summary>
    /// A handler that returns default value of type <typeparamref name="TResponse"/>. This is usually set as the last handler in the chain.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class ReturnDefaultHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        static ReturnDefaultHandler()
        {
        }

        private ReturnDefaultHandler()
        {
        }

        /// <summary>
        /// Singleton instance of this handler.
        /// </summary>
        public static IHandler<TRequest, TResponse> Instance { get; } =
            new ReturnDefaultHandler<TRequest, TResponse>();

        /// <summary>
        /// Returns default value of <typeparamref name="TResponse"/> on invocation.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            return default(TResponse);
        }
    }
}