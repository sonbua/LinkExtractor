using System;
using System.Threading.Tasks;

namespace ResponsibilityChain
{
    /// <summary>
    /// A handler that returns a completed task with default value of type <typeparamref name="TResponse"/>. This is usually set as the last handler in the chain.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class ReturnCompletedTaskFromDefaultValueHandler<TRequest, TResponse> : IHandler<TRequest, Task<TResponse>>
    {
        static ReturnCompletedTaskFromDefaultValueHandler()
        {
        }

        private ReturnCompletedTaskFromDefaultValueHandler()
        {
        }

        /// <summary>
        /// Singleton instance of this handler.
        /// </summary>
        public static IHandler<TRequest, Task<TResponse>> Instance { get; } =
            new ReturnCompletedTaskFromDefaultValueHandler<TRequest, TResponse>();

        /// <summary>
        /// Returns a completed task with default value of <typeparamref name="TResponse"/> on invocation.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task<TResponse> Handle(TRequest request, Func<TRequest, Task<TResponse>> next) =>
            Task.FromResult(default(TResponse));
    }
}