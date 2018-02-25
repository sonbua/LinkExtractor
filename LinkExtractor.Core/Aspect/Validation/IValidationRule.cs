using System.Threading.Tasks;

namespace LinkExtractor.Core.Aspect.Validation
{
    public interface IValidationRule
    {
    }

    public interface IValidationRule<TRequest> : IValidationRule
    {
        Task TestAsync(TRequest request);
    }
}