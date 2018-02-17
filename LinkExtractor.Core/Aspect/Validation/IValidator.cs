using System.Threading.Tasks;

namespace LinkExtractor.Core.Aspect.Validation
{
    public interface IValidator
    {
    }

    public interface IValidator<TRequest> : IValidator
    {
        Task ValidateAsync(TRequest request);
    }
}