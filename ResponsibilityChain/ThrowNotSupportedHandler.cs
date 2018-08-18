using System;

namespace ResponsibilityChain
{
    public sealed class ThrowNotSupportedHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        static ThrowNotSupportedHandler()
        {
        }

        private ThrowNotSupportedHandler()
        {
        }

        public static IHandler<TRequest, TResponse> Instance { get; } =
            new ThrowNotSupportedHandler<TRequest, TResponse>();

        public TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            throw new NotSupportedException(
                $"Cannot handle this request. Request information: {typeof(TRequest)}"
            );
        }
    }
}