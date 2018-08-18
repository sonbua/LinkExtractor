using System;

namespace ResponsibilityChain
{
    public interface IHandler
    {
    }

    public interface IHandler<TRequest, TResponse> : IHandler
    {
        TResponse Handle(TRequest request, Func<TRequest, TResponse> next);
    }
}