using System.Threading.Tasks;

namespace Cqrs.Aspect.Postprocessing
{
    public interface IPostprocessor
    {
    }

    public interface IPostprocessor<TRequest, TResponse> : IPostprocessor
    {
        Task ProcessAsync(TRequest request, TResponse response);
    }
}