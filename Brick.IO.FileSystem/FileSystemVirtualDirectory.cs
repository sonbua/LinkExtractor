using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Brick.IO
{
    public class FileSystemVirtualDirectory : VirtualDirectory
    {
        protected DirectoryInfo BackingDirectoryInfo;

        public FileSystemVirtualDirectory(
            IVirtualPathProvider owningProvider,
            IVirtualDirectory parentDirectory,
            DirectoryInfo directoryInfo)
            : base(owningProvider, parentDirectory)
        {
            BackingDirectoryInfo = directoryInfo ?? throw new ArgumentNullException(nameof(directoryInfo));
        }

        public override string Name => BackingDirectoryInfo.Name;

        public override string RealPath => BackingDirectoryInfo.FullName;
        
        public override DateTime LastModified => BackingDirectoryInfo.LastWriteTimeUtc;

        public override IEnumerable<IVirtualFile> Files =>
            this.Where(n => !n.IsDirectory).Cast<IVirtualFile>();

        public override IEnumerable<IVirtualDirectory> Directories =>
            this.Where(n => n.IsDirectory).Cast<IVirtualDirectory>();

        public override IEnumerator<IVirtualNode> GetEnumerator()
        {
            var directoryNodes =
                GetDirectories()
                    .Select(directoryInfo => new FileSystemVirtualDirectory(VirtualPathProvider, this, directoryInfo))
                    .Where(x => !x.ShouldSkipPath());

            var fileNodes =
                GetFiles().Select(fileInfo => new FileSystemVirtualFile(VirtualPathProvider, this, fileInfo));

            return directoryNodes.Cast<IVirtualNode>()
                .Union(fileNodes.Cast<IVirtualNode>())
                .GetEnumerator();
        }

        protected override IVirtualFile GetFileFromBackingDirectoryOrDefault(string fileName)
        {
            var fileInfo = EnumerateFiles(fileName).FirstOrDefault();

            return fileInfo != null
                ? new FileSystemVirtualFile(VirtualPathProvider, this, fileInfo)
                : null;
        }

        protected override IVirtualDirectory GetDirectoryFromBackingDirectoryOrDefault(string directoryName)
        {
            var directoryInfo = EnumerateDirectories(directoryName).FirstOrDefault();

            return directoryInfo != null
                ? new FileSystemVirtualDirectory(VirtualPathProvider, this, directoryInfo)
                : NullVirtualDirectory.Instance;
        }

        private DirectoryInfo[] GetDirectories() => BackingDirectoryInfo.GetDirectories();

        private FileInfo[] GetFiles() => BackingDirectoryInfo.GetFiles();

        private IEnumerable<FileInfo> EnumerateFiles(string pattern) =>
            BackingDirectoryInfo.GetFiles(pattern, SearchOption.TopDirectoryOnly);

        private IEnumerable<DirectoryInfo> EnumerateDirectories(string directoryName) =>
            BackingDirectoryInfo.GetDirectories(directoryName, SearchOption.TopDirectoryOnly);
    }
}