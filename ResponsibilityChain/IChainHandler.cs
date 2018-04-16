namespace ResponsibilityChain
{
    public interface IChainHandler<TRequest, TResponse>
    {
        bool CanHandle(TRequest request);

        TResponse Handle(TRequest request);
    }
}