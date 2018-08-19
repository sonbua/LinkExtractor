using System.IO;

namespace Brick.IO.FileSystem.IntegrationTests
{
    internal static class TestConfiguration
    {
        public static string RootDirectory = @"C:\Temp\Brick.IO.FileSystem.Tests.Sandbox";

        static TestConfiguration()
        {
            if (!Directory.Exists(RootDirectory))
            {
                Directory.CreateDirectory(RootDirectory);
            }
        }

        public static void CleanupRootDirectory()
        {
            foreach (var directory in Directory.EnumerateDirectories(RootDirectory))
            {
                Directory.Delete(directory, recursive: true);
            }

            foreach (var file in Directory.EnumerateFiles(RootDirectory))
            {
                File.Delete(file);
            }
        }
    }
}