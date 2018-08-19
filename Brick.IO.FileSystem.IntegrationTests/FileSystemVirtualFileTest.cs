using System;
using Xunit;

namespace Brick.IO.FileSystem.IntegrationTests
{
    public class FileSystemVirtualFileTest : IDisposable
    {
        private readonly FileSystemVirtualFiles _virtualFiles;

        public FileSystemVirtualFileTest()
        {
            _virtualFiles = new FileSystemVirtualFiles(TestConfiguration.RootDirectory);
        }

        [Theory]
        [InlineData("test.txt", "test.txt", "txt", "test.txt")]
        [InlineData("dir/test.txt", "test.txt", "txt", "dir/test.txt")]
        [InlineData("/dir/test.txt", "test.txt", "txt", "dir/test.txt")]
        [InlineData("\\dir\\test.txt", "test.txt", "txt", "dir/test.txt")]
        public void ExistingFile_ReturnsCorrectMetadata(
            string virtualPath,
            string expectedFileName,
            string expectedExtension,
            string expectedVirtualPath)
        {
            // arrange
            _virtualFiles.WriteFile(virtualPath, "some content");
            var virtualFile = _virtualFiles.GetFile(virtualPath);

            // act

            // assert
            Assert.Equal(expectedFileName, virtualFile.Name);
            Assert.Equal(expectedExtension, virtualFile.Extension);
            Assert.False(virtualFile.IsDirectory);
            Assert.Equal(expectedVirtualPath, virtualFile.VirtualPath);
            Assert.Equal(
                $"{TestConfiguration.RootDirectory}\\{expectedVirtualPath.Replace('/', '\\')}",
                virtualFile.RealPath
            );
        }

        public void Dispose()
        {
            TestConfiguration.CleanupRootDirectory();
        }
    }
}