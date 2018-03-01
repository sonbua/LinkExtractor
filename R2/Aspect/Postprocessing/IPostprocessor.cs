using System.Threading.Tasks;

namespace R2.Aspect.Postprocessing
{
    public interface IPostprocessor
    {
    }

    public interface IPostprocessor<TRequest, TResponse> : IPostprocessor
    {
        Task ProcessAsync(TRequest request, TResponse response);
    }
}