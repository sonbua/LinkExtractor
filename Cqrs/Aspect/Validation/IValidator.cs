using System.Threading.Tasks;

namespace Cqrs.Aspect.Validation
{
    public interface IValidator
    {
    }

    public interface IValidator<TRequest> : IValidator
    {
        Task ValidateAsync(TRequest request);
    }
}