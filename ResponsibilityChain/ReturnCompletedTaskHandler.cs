using System;
using System.Threading.Tasks;

namespace ResponsibilityChain
{
    /// <summary>
    /// A handler that returns a completed task. This is usually set as the last handler in the chain.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    public sealed class ReturnCompletedTaskHandler<TRequest> : IHandler<TRequest, Task>
    {
        static ReturnCompletedTaskHandler()
        {
        }

        private ReturnCompletedTaskHandler()
        {
        }

        /// <summary>
        /// Singleton instance of this handler.
        /// </summary>
        public static IHandler<TRequest, Task> Instance { get; } =
            new ReturnCompletedTaskHandler<TRequest>();

        /// <summary>
        /// Returns a completed task.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task Handle(TRequest request, Func<TRequest, Task> next) => Task.FromResult(0);
    }
}