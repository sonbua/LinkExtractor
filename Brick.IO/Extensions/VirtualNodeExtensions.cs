using System;
using System.Linq;

namespace Brick.IO
{
    public static class VirtualNodeExtensions
    {
        public static bool ShouldSkipPath(this IVirtualNode node)
        {
            return VirtualFile.ScanSkipPaths
                .Select(skipPath => skipPath.TrimStart('/'))
                .Any(skipPath => node.VirtualPath.StartsWith(skipPath, StringComparison.OrdinalIgnoreCase));
        }
    }
}