using System;

namespace LinkExtractor.Core.Aspect.Caching
{
    public class CacheableResponseAttribute : Attribute
    {
        public int Duration { get; set; } = 3600;
    }
}