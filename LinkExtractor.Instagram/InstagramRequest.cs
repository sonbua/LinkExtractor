using System.ComponentModel.DataAnnotations;
using R2;
using R2.Aspect.Caching;

namespace LinkExtractor.Instagram
{
    [CacheableResponse]
    public class InstagramRequest : IQuery<InstagramResponse>
    {
        [Required]
        [RegularExpression(@"^http(s)?://(www\.)?instagram\.com/.+$")]
        public string Url { get; set; }
    }
}