using System;
using System.IO;

namespace Brick.IO
{
    public sealed class NullVirtualFile : IVirtualFile
    {
        private NullVirtualFile()
        {
        }

        public static IVirtualFile Instance => Nested._instance;

        public IVirtualDirectory Directory => throw new NotSupportedException();

        public string Name => throw new NotSupportedException();

        public string VirtualPath => throw new NotSupportedException();

        public string RealPath => throw new NotSupportedException();

        public bool IsDirectory => throw new NotSupportedException();

        public DateTime LastModified => throw new NotSupportedException();

        public IVirtualPathProvider VirtualPathProvider => throw new NotSupportedException();

        public string Extension => throw new NotSupportedException();

        public long Length => throw new NotSupportedException();

        public string GetFileHash() => throw new NotSupportedException();

        public Stream OpenRead() => throw new NotSupportedException();

        public StreamReader OpenText() => throw new NotSupportedException();

        public string ReadAllText() => throw new NotSupportedException();

        public void Refresh() => throw new NotSupportedException();

        private class Nested
        {
            internal static readonly NullVirtualFile _instance = new NullVirtualFile();

            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            static Nested()
            {
            }
        }
    }
}