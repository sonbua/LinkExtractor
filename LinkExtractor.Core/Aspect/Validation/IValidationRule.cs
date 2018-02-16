using System.Threading.Tasks;

namespace LinkExtractor.Core.Aspect.Validation
{
    public interface IValidationRule<TRequest>
    {
        Task TestAsync(TRequest request);
    }
}