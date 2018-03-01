using System.Threading.Tasks;

namespace R2.Aspect.Validation
{
    public interface IValidationRule
    {
    }

    public interface IValidationRule<TRequest> : IValidationRule
    {
        Task TestAsync(TRequest request);
    }
}