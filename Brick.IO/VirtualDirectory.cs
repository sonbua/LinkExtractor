using System;
using System.Collections;
using System.Collections.Generic;
using EnsureThat;

namespace Brick.IO
{
    public abstract class VirtualDirectory : IVirtualDirectory
    {
        protected IVirtualPathProvider VirtualPathProvider;

        protected VirtualDirectory(IVirtualPathProvider owningProvider)
            : this(owningProvider, NullVirtualDirectory.Instance)
        {
        }

        protected VirtualDirectory(IVirtualPathProvider owningProvider, IVirtualDirectory parentDirectory)
        {
            EnsureArg.IsNotNull(owningProvider, nameof(owningProvider));
            EnsureArg.IsNotNull(parentDirectory, nameof(parentDirectory));

            VirtualPathProvider = owningProvider;
            ParentDirectory = parentDirectory;
        }

        public IVirtualDirectory Directory => this;

        public abstract string Name { get; }

        public virtual string VirtualPath => GetVirtualPathToRoot();

        public virtual string RealPath => GetRealPathToRoot();

        public virtual bool IsDirectory => true;

        public abstract DateTime LastModified { get; }

        public virtual bool IsRoot => ReferenceEquals(ParentDirectory, NullVirtualDirectory.Instance);

        public IVirtualDirectory ParentDirectory { get; }

        public abstract IEnumerable<IVirtualFile> Files { get; }

        public abstract IEnumerable<IVirtualDirectory> Directories { get; }

        public virtual IVirtualFile GetFile(string virtualPath)
        {
            var tokens = virtualPath.TokenizeVirtualPath(VirtualPathProvider.VirtualPathSeparator);

            return GetFile(tokens);
        }

        public IVirtualFile GetFile(Stack<string> virtualPath)
        {
            if (virtualPath.Count == 0)
            {
                return NullVirtualFile.Instance;
            }

            var pathToken = virtualPath.Pop();

            if (virtualPath.Count == 0)
            {
                return GetFileFromBackingDirectoryOrDefault(pathToken);
            }

            var virtualDirectory = GetDirectoryFromBackingDirectoryOrDefault(pathToken);

            return ReferenceEquals(virtualDirectory, NullVirtualDirectory.Instance)
                ? NullVirtualFile.Instance
                : virtualDirectory.GetFile(virtualPath);
        }

        public virtual IVirtualDirectory GetDirectory(string virtualPath)
        {
            var tokens = virtualPath.TokenizeVirtualPath(VirtualPathProvider.VirtualPathSeparator);

            return GetDirectory(tokens);
        }

        public IVirtualDirectory GetDirectory(Stack<string> virtualPath)
        {
            if (virtualPath.Count == 0)
            {
                return NullVirtualDirectory.Instance;
            }

            var pathToken = virtualPath.Pop();

            var virtualDirectory = GetDirectoryFromBackingDirectoryOrDefault(pathToken);

            if (ReferenceEquals(virtualDirectory, NullVirtualDirectory.Instance))
            {
                return NullVirtualDirectory.Instance;
            }

            return virtualPath.Count == 0
                ? virtualDirectory
                : virtualDirectory.GetDirectory(virtualPath);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public abstract IEnumerator<IVirtualNode> GetEnumerator();

        public override bool Equals(object obj)
        {
            if (!(obj is VirtualDirectory other))
            {
                return false;
            }

            return other.VirtualPath == VirtualPath;
        }

        public override int GetHashCode() => VirtualPath.GetHashCode();

        public override string ToString() => $"{RealPath} -> {VirtualPath}";

        protected virtual string GetVirtualPathToRoot() =>
            GetPathToRoot(VirtualPathProvider.VirtualPathSeparator, p => p.VirtualPath);

        protected virtual string GetRealPathToRoot() =>
            GetPathToRoot(VirtualPathProvider.RealPathSeparator, p => p.RealPath);

        protected virtual string GetPathToRoot(string separator, Func<IVirtualDirectory, string> pathSelector)
        {
            if (IsRoot)
            {
                return string.Empty;
            }

            var parentPath = pathSelector(ParentDirectory);

            return parentPath == string.Empty
                ? Name
                : string.Concat(parentPath, separator, Name);
        }

        protected abstract IVirtualFile GetFileFromBackingDirectoryOrDefault(string fileName);

        protected abstract IVirtualDirectory GetDirectoryFromBackingDirectoryOrDefault(string directoryName);
    }
}