using System;

namespace ResponsibilityChain
{
    /// <summary>
    /// A handler that returns default value of type <typeparamref name="TResponse"/>. This is usually set as the last handler in the chain.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class ReturnDefaultValueHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        static ReturnDefaultValueHandler()
        {
        }

        private ReturnDefaultValueHandler()
        {
        }

        /// <summary>
        /// Singleton instance of this handler.
        /// </summary>
        public static IHandler<TRequest, TResponse> Instance { get; } =
            new ReturnDefaultValueHandler<TRequest, TResponse>();

        /// <summary>
        /// Returns default value of <typeparamref name="TResponse"/> on invocation.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public TResponse Handle(TRequest request, Func<TRequest, TResponse> next) => default(TResponse);
    }
}