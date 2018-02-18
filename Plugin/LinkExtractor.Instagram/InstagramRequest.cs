using System.ComponentModel.DataAnnotations;

namespace LinkExtractor.Instagram
{
    public class InstagramRequest
    {
        [RegularExpression(@"^http(s)?://(www\.)?instagram\.com/.+$")]
        public string Url { get; set; }
    }
}