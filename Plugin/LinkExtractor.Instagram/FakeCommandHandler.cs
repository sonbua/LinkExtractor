using System.Threading.Tasks;
using Cqrs;

namespace LinkExtractor.Instagram
{
    public class FakeCommandHandler : BaseCommandHandler<FakeCommand>
    {
        public override Task<Nothing<FakeCommand>> HandleAsync(FakeCommand request)
        {
            // do something fake here

            return Task.FromResult(Nothing<FakeCommand>.Instance);
        }
    }
}