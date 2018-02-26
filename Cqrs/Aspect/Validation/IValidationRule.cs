using System.Threading.Tasks;

namespace Cqrs.Aspect.Validation
{
    public interface IValidationRule
    {
    }

    public interface IValidationRule<TRequest> : IValidationRule
    {
        Task TestAsync(TRequest request);
    }
}