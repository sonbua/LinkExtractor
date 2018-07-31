using System;
using System.Collections;
using System.Collections.Generic;

namespace Brick.IO
{
    public sealed class NullVirtualDirectory : IVirtualDirectory
    {
        private NullVirtualDirectory()
        {
        }

        public static IVirtualDirectory Instance => Nested._instance;

        public IVirtualDirectory Directory => throw new NotSupportedException();

        public string Name => throw new NotSupportedException();

        public string VirtualPath => throw new NotSupportedException();

        public string RealPath => throw new NotSupportedException();

        public bool IsDirectory => throw new NotSupportedException();

        public DateTime LastModified => throw new NotSupportedException();

        public IEnumerator<IVirtualNode> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsRoot => throw new NotSupportedException();

        public IVirtualDirectory ParentDirectory => throw new NotSupportedException();

        public IEnumerable<IVirtualFile> Files => throw new NotSupportedException();

        public IEnumerable<IVirtualDirectory> Directories => throw new NotSupportedException();

        public IVirtualFile GetFile(string virtualPath) => throw new NotSupportedException();

        public IVirtualFile GetFile(Stack<string> virtualPath) => throw new NotSupportedException();

        public IVirtualDirectory GetDirectory(string virtualPath) => throw new NotSupportedException();

        public IVirtualDirectory GetDirectory(Stack<string> virtualPath) => throw new NotSupportedException();

        private class Nested
        {
            internal static readonly NullVirtualDirectory _instance = new NullVirtualDirectory();

            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            static Nested()
            {
            }
        }
    }
}