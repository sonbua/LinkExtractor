using System.Threading.Tasks;

namespace Cqrs.Aspect.Preprocessing
{
    public interface IPreprocessor
    {
    }

    public interface IPreprocessor<TRequest> : IPreprocessor
    {
        Task ProcessAsync(TRequest request);
    }
}