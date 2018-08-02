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

        /// <summary>
        /// Opens the request stream for reading the uploaded file.
        /// </summary>
        Stream OpenReadStream();

        /// <summary>
        /// Copies the contents of the uploaded file to the <paramref name="target" /> stream.
        /// </summary>
        /// <param name="target">The stream to copy the file contents to.</param>
        void CopyTo(Stream target);

        /// <summary>
        /// Asynchronously copies the contents of the uploaded file to the <paramref name="target" /> stream.
        /// </summary>
        /// <param name="target">The stream to copy the file contents to.</param>
        /// <param name="cancellationToken"></param>
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken));
    }
}