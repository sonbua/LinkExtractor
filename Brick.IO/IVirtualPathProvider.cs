namespace Brick.IO
{
    public interface IVirtualPathProvider
    {
        IVirtualDirectory RootDirectory { get; }

        string VirtualPathSeparator { get; }

        string RealPathSeparator { get; }

        string CombineVirtualPath(string basePath, string relativePath);

        bool FileExists(string virtualPath);

        bool DirectoryExists(string virtualPath);

        IVirtualFile GetFile(string virtualPath);

        string GetFileHash(string virtualPath);

        string GetFileHash(IVirtualFile virtualFile);

        IVirtualDirectory GetDirectory(string virtualPath);
    }
}