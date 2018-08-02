using System;
using System.Collections.Generic;
using System.IO;

namespace Brick.IO
{
    public interface IVirtualFiles : IVirtualPathProvider
    {
        void WriteFile(string filePath, string textContents);

        void WriteFile(string filePath, Stream stream);

        void WriteFiles(IEnumerable<IVirtualFile> files);

        void WriteFiles(IEnumerable<IVirtualFile> files, Func<IVirtualFile, string> destinationPathSelector);

        void AppendFile(string filePath, string textContents);

        void AppendFile(string filePath, Stream stream);

        void DeleteFile(string filePath);

        void DeleteFiles(IEnumerable<string> filePaths);

        void DeleteFolder(string directoryPath);
    }
}