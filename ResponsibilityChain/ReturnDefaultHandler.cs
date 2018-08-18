using System;

namespace ResponsibilityChain
{
    public sealed class ReturnDefaultHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        static ReturnDefaultHandler()
        {
        }

        private ReturnDefaultHandler()
        {
        }

        public static IHandler<TRequest, TResponse> Instance { get; } =
            new ReturnDefaultHandler<TRequest, TResponse>();

        public TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            return default(TResponse);
        }
    }
}