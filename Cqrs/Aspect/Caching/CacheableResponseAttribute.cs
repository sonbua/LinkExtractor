using System;

namespace Cqrs.Aspect.Caching
{
    public class CacheableResponseAttribute : Attribute
    {
        /// <summary>
        /// Duration in seconds.
        /// </summary>
        public int Duration { get; set; } = 3600;
    }
}