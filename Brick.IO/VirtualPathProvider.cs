using System;

namespace Brick.IO
{
    public abstract class VirtualPathProvider : IVirtualPathProvider
    {
        public abstract IVirtualDirectory RootDirectory { get; }

        public abstract string VirtualPathSeparator { get; }

        public abstract string RealPathSeparator { get; }

        public virtual string CombineVirtualPath(string basePath, string relativePath) =>
            string.Concat(basePath, VirtualPathSeparator, relativePath);

        public virtual bool FileExists(string virtualPath)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException(nameof(virtualPath));
            }

            var virtualFile = GetFile(SanitizePath(virtualPath));

            return !ReferenceEquals(virtualFile, NullVirtualFile.Instance);
        }

        public virtual bool DirectoryExists(string virtualPath)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException(nameof(virtualPath));
            }

            var virtualDirectory = GetDirectory(SanitizePath(virtualPath));

            return !ReferenceEquals(virtualDirectory, NullVirtualDirectory.Instance);
        }

        public virtual IVirtualFile GetFile(string virtualPath)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException(nameof(virtualPath));
            }

            return RootDirectory.GetFile(SanitizePath(virtualPath));
        }

        public string GetFileHash(string virtualPath) => GetFile(virtualPath).GetFileHash();

        public string GetFileHash(IVirtualFile virtualFile) => virtualFile.GetFileHash();

        public IVirtualDirectory GetDirectory(string virtualPath) =>
            IsRoot(virtualPath)
                ? RootDirectory
                : RootDirectory.GetDirectory(SanitizePath(virtualPath));

        private static bool IsRoot(string virtualPath) => virtualPath == string.Empty || virtualPath == "/";

        protected static string SanitizePath(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return SanitizePathImpl(filePath);
        }

        private static string SanitizePathImpl(string filePath)
        {
            if (filePath == string.Empty)
            {
                return filePath;
            }

            var sanitizedPath =
                filePath[0] == '/'
                    ? filePath.Substring(1)
                    : filePath;

            return sanitizedPath.Replace('\\', '/');
        }
    }
}