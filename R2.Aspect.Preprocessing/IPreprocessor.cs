using System.Threading.Tasks;

namespace R2.Aspect.Preprocessing
{
    public interface IPreprocessor
    {
    }

    public interface IPreprocessor<TRequest> : IPreprocessor
    {
        Task ProcessAsync(TRequest request);
    }
}