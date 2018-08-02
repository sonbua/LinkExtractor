using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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

        private R2File(Stream stream, string name, string fileName, string contentType)
        {
            _stream = stream;
            Name = name;
            FileName = fileName;
            Length = stream.Length;
            ContentType = contentType;
        }

        public string ContentType { get; set; }

        public long Length { get; }

        public string Name { get; }

        public string FileName { get; }

        public Stream OpenReadStream() => _stream;

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            await _stream.CopyToAsync(target, DefaultBufferSize, cancellationToken);
        }
    }
}