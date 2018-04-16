namespace ResponsibilityChain
{
    public class NullChainHandler<TRequest, TResponse> : IChainHandler<TRequest, TResponse>
    {
        public bool CanHandle(TRequest request) => true;

        public TResponse Handle(TRequest request) => default(TResponse);
    }
}