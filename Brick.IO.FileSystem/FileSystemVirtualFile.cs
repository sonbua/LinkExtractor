using System;
using System.IO;

namespace Brick.IO
{
    internal class FileSystemVirtualFile : VirtualFile
    {
        protected FileInfo BackingFileInfo;

        public FileSystemVirtualFile(
            IVirtualPathProvider owningProvider,
            IVirtualDirectory directory,
            FileInfo fileInfo)
            : base(owningProvider, directory)
        {
            BackingFileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
        }

        public override string Name => BackingFileInfo.Name;

        public override string RealPath => BackingFileInfo.FullName;

        public override DateTime LastModified => BackingFileInfo.LastWriteTimeUtc;

        public override long Length => BackingFileInfo.Length;

        public override Stream OpenRead()
        {
            var i = 0;
            var firstAttempt = DateTime.UtcNow;
            IOException originalException = null;

            while (DateTime.UtcNow - firstAttempt < VirtualPathUtils.MaxRetryOnTimeoutException)
            {
                try
                {
                    i++;
                    return BackingFileInfo.OpenRead();
                }
                // catch The process cannot access the file '...' because it is being used by another process.
                catch (IOException exception)
                {
                    if (originalException == null)
                    {
                        originalException = exception;
                    }

                    i.SleepBackOffMultiplier();
                }
            }

            throw new TimeoutException(
                $"Exceeded timeout of {VirtualPathUtils.MaxRetryOnTimeoutException}",
                originalException
            );
        }

        public override void Refresh()
        {
            BackingFileInfo.Refresh();
        }
    }
}