using System.Threading.Tasks;

namespace LinkExtractor.Core.Aspect.Preprocessing
{
    public interface IPreprocessor
    {
    }

    public interface IPreprocessor<TRequest> : IPreprocessor
    {
        Task ProcessAsync(TRequest request);
    }
}