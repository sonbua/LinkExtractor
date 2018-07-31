using System.Collections.Generic;

namespace Brick.IO
{
    public interface IVirtualDirectory : IVirtualNode, IEnumerable<IVirtualNode>
    {
        bool IsRoot { get; }

        IVirtualDirectory ParentDirectory { get; }

        IEnumerable<IVirtualFile> Files { get; }

        IEnumerable<IVirtualDirectory> Directories { get; }

        IVirtualFile GetFile(string virtualPath);

        IVirtualFile GetFile(Stack<string> virtualPath);

        IVirtualDirectory GetDirectory(string virtualPath);

        IVirtualDirectory GetDirectory(Stack<string> virtualPath);
    }
}