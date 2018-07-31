using System;
using System.Collections.Generic;
using System.IO;

namespace Brick.IO
{
    public static class VirtualPathProviderExtensions
    {
        private static readonly string NotWritableErrorMessage = $"{0} does not implement {typeof(IVirtualFiles)}";

        public static void CopyFrom(
            this IVirtualPathProvider pathProvider,
            IEnumerable<IVirtualFile> sourceFiles,
            Func<IVirtualFile, string> toPathFunc)
        {
            foreach (var file in sourceFiles)
            {
                using (var stream = file.OpenRead())
                {
                    var destinationPath = toPathFunc(file);
                    
                    if (destinationPath == null)
                    {
                        continue;
                    }

                    pathProvider.WriteFile(destinationPath, stream);
                }
            }
        }
        public static void CopyFrom(
            this IVirtualPathProvider pathProvider,
            IEnumerable<IVirtualFile> sourceFiles)
        {
            foreach (var file in sourceFiles)
            {
                using (var stream = file.OpenRead())
                {
                    var destinationPath = file.VirtualPath;
                    
                    if (destinationPath == null)
                    {
                        continue;
                    }

                    pathProvider.WriteFile(destinationPath, stream);
                }
            }
        }

        public static void WriteFile(this IVirtualPathProvider pathProvider, string filePath, Stream stream)
        {
            if (!(pathProvider is IVirtualFiles writableFiles))
            {
                throw new InvalidOperationException(string.Format(NotWritableErrorMessage, pathProvider.GetType().Name));
            }

            writableFiles.WriteFile(filePath, stream);
        }
    }
}