using System.Threading.Tasks;

namespace R2.Aspect.Validation
{
    public interface IValidator
    {
    }

    public interface IValidator<TRequest> : IValidator
    {
        Task ValidateAsync(TRequest request);
    }
}