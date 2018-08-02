using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace R2
{
    public interface IFile
    {
        string ContentType { get; }

        long Length { get; }

        string Name { get; }

        string FileName { get; }

        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken));
    }
}