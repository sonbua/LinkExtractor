using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;

namespace R2
{
    public class R2File : IFile
    {
        private const int DefaultBufferSize = 81920;
        private readonly Stream _stream;

        public R2File(Stream stream, string name, string fileName)
            : this(stream, name, fileName, string.Empty)
        {
        }

        public R2File(Stream stream, string name, string fileName, string contentType)
        {
            _stream = stream;
            Name = name;
            FileName = fileName;
            Length = stream.Length;
            ContentType = contentType;
        }

        public string ContentType { get; }

        public long Length { get; }

        public string Name { get; }

        public string FileName { get; }

        public Stream OpenReadStream()
        {
            _stream.Position = 0;

            return _stream;
        }

        public void CopyTo(Stream target)
        {
            EnsureArg.IsNotNull(target, nameof(target));

            OpenReadStream().CopyTo(target, DefaultBufferSize);
        }

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureArg.IsNotNull(target, nameof(target));

            await OpenReadStream().CopyToAsync(target, DefaultBufferSize, cancellationToken);
        }
    }
}