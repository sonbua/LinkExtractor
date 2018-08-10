using System;
using System.Collections.Generic;
using System.IO;
using EnsureThat;

namespace Brick.IO
{
    public abstract class VirtualFile : IVirtualFile
    {
        protected VirtualFile(IVirtualPathProvider owningProvider, IVirtualDirectory directory)
        {
            EnsureArg.IsNotNull(owningProvider, nameof(owningProvider));
            EnsureArg.IsNotNull(directory, nameof(directory));

            VirtualPathProvider = owningProvider;
            Directory = directory;
        }

        public static List<string> ScanSkipPaths { get; } = new List<string>();

        public IVirtualDirectory Directory { get; }

        public abstract string Name { get; }

        public virtual string VirtualPath => GetVirtualPathToRoot();

        public virtual string RealPath => GetRealPathToRoot();

        public virtual bool IsDirectory => false;

        public abstract DateTime LastModified { get; }

        public IVirtualPathProvider VirtualPathProvider { get; }

        public string Extension => Name.LastRightPart('.');

        public abstract long Length { get; }

        public virtual string GetFileHash()
        {
            using (var stream = OpenRead())
            {
                return stream.ToMd5Hash();
            }
        }

        public abstract Stream OpenRead();

        public virtual StreamReader OpenText() => new StreamReader(OpenRead());

        public virtual string ReadAllText()
        {
            using (var reader = OpenText())
            {
                var text = reader.ReadToEnd();
                return text;
            }
        }

        public virtual void Refresh()
        {
        }

        public override bool Equals(object obj)
        {
            if (!(obj is VirtualFile other))
            {
                return false;
            }

            return other.VirtualPath == VirtualPath;
        }

        public override int GetHashCode() => VirtualPath.GetHashCode();

        public override string ToString() => $"{RealPath} -> {VirtualPath}";

        protected virtual string GetVirtualPathToRoot()
        {
            return GetPathToRoot(VirtualPathProvider.VirtualPathSeparator, p => p.VirtualPath);
        }

        protected virtual string GetRealPathToRoot()
        {
            return GetPathToRoot(VirtualPathProvider.RealPathSeparator, p => p.RealPath);
        }

        protected virtual string GetPathToRoot(string separator, Func<IVirtualDirectory, string> pathSelector)
        {
            var parentPath = Directory != null ? pathSelector(Directory) : string.Empty;
            if (parentPath == separator)
            {
                parentPath = string.Empty;
            }

            return parentPath == null
                ? Name
                : string.Concat(parentPath, separator, Name);
        }
    }
}