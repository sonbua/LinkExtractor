using System.IO;

namespace Brick.IO
{
    public interface IVirtualFile : IVirtualNode
    {
        IVirtualPathProvider VirtualPathProvider { get; }

        string Extension { get; }

        long Length { get; }

        string GetFileHash();
        
        Stream OpenRead();

        StreamReader OpenText();

        string ReadAllText();

        void Refresh();
    }
}