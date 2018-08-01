using System;
using System.Collections.Generic;
using System.IO;

namespace Brick.IO
{
    public class FileSystemVirtualFiles : VirtualPathProvider, IVirtualFiles
    {
        protected DirectoryInfo RootDirectoryInfo;

        public FileSystemVirtualFiles(string rootDirectoryPath)
            : this(new DirectoryInfo(rootDirectoryPath))
        {
        }

        public FileSystemVirtualFiles(DirectoryInfo rootDirectoryInfo)
        {
            if (rootDirectoryInfo == null)
            {
                throw new ArgumentNullException(nameof(rootDirectoryInfo));
            }

            if (!rootDirectoryInfo.Exists)
            {
                throw new Exception($"Root directory '{RootDirectoryInfo.FullName}' for virtual path does not exist.");
            }

            RootDirectoryInfo = rootDirectoryInfo;
            RootDirectory = new FileSystemVirtualDirectory(this, NullVirtualDirectory.Instance, RootDirectoryInfo);
        }

        public override IVirtualDirectory RootDirectory { get; }

        public override string VirtualPathSeparator => "/";

        public override string RealPathSeparator => Convert.ToString(Path.DirectorySeparatorChar);

        public override bool DirectoryExists(string virtualPath)
        {
            var isDirectory = Directory.Exists(RootDirectory.RealPath.CombineWith(SanitizePath(virtualPath)));
            return isDirectory;
        }

        public override bool FileExists(string virtualPath) =>
            File.Exists(RootDirectory.RealPath.CombineWith(SanitizePath(virtualPath)));

        public void WriteFile(string filePath, string textContents)
        {
            var realFilePath = RootDirectory.RealPath.CombineWith(filePath);

            EnsureDirectory(Path.GetDirectoryName(realFilePath));

            File.WriteAllText(realFilePath, textContents);
        }

        public void WriteFile(string filePath, Stream stream)
        {
            var realFilePath = RootDirectory.RealPath.CombineWith(filePath);

            EnsureDirectory(Path.GetDirectoryName(realFilePath));

            File.WriteAllBytes(realFilePath, stream.ReadFully());
        }

        public void WriteFiles(IEnumerable<IVirtualFile> files, Func<IVirtualFile, string> toPath = null)
        {
            this.CopyFrom(files, toPath);
        }

        public void AppendFile(string filePath, string textContents)
        {
            var realFilePath = RootDirectory.RealPath.CombineWith(filePath);

            EnsureDirectory(Path.GetDirectoryName(realFilePath));

            File.AppendAllText(realFilePath, textContents);
        }

        public void AppendFile(string filePath, Stream stream)
        {
            var realFilePath = RootDirectory.RealPath.CombineWith(filePath);

            EnsureDirectory(Path.GetDirectoryName(realFilePath));

            using (var fileStream = new FileStream(realFilePath, FileMode.Append))
            {
                stream.WriteTo(fileStream);
            }
        }

        public void DeleteFile(string filePath)
        {
            var realFilePath = RootDirectory.RealPath.CombineWith(filePath);

            try
            {
                File.Delete(realFilePath);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public void DeleteFiles(IEnumerable<string> filePaths)
        {
            filePaths.ForEach(DeleteFile);
        }

        public void DeleteFolder(string directoryPath)
        {
            var realPath = RootDirectory.RealPath.CombineWith(directoryPath);

            if (Directory.Exists(realPath))
            {
                Directory.Delete(realPath, true);
            }
        }

        private static void EnsureDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}