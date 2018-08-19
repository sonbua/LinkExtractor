using System;
using Xunit;

namespace Brick.IO.FileSystem.IntegrationTests
{
    public class FileSystemVirtualFilesTest : IDisposable
    {
        private readonly FileSystemVirtualFiles _virtualFiles;

        public FileSystemVirtualFilesTest()
        {
            _virtualFiles = new FileSystemVirtualFiles(TestConfiguration.RootDirectory);
        }

        [Theory]
        [InlineData("non-existing-file.txt")]
        [InlineData("/non-existing-file.txt")]
        [InlineData("non-existing-directory/non-existing-file.txt")]
        [InlineData("/non-existing-directory/non-existing-file.txt")]
        public void GetFile_NonExistingFile_ReturnsNullVirtualFileInstance(string virtualPath)
        {
            // arrange

            // act
            var virtualFile = _virtualFiles.GetFile(virtualPath);

            // assert
            Assert.Equal(NullVirtualFile.Instance, virtualFile);
        }

        [Theory]
        [InlineData("file.txt", "file.txt")]
        [InlineData("/file.txt", "file.txt")]
        [InlineData("dir/file.txt", "dir/file.txt")]
        [InlineData("/dir/file.txt", "dir/file.txt")]
        public void GetFile_ExistingFile_ReturnsVirtualFileWithCorrectMetadata(
            string virtualPath,
            string virtualPathToRoot)
        {
            // arrange
            _virtualFiles.WriteFile(virtualPath, "some contents");

            // act
            var virtualFile = _virtualFiles.GetFile(virtualPath);

            // assert
            Assert.Equal(virtualPathToRoot, virtualFile.VirtualPath);
        }

        public void Dispose()
        {
            TestConfiguration.CleanupRootDirectory();
        }
    }
}