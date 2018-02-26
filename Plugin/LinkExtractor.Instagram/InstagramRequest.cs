﻿using System.ComponentModel.DataAnnotations;
using Cqrs.Aspect.Caching;

namespace LinkExtractor.Instagram
{
    [CacheableResponse]
    public class InstagramRequest
    {
        [Required]
        [RegularExpression(@"^http(s)?://(www\.)?instagram\.com/.+$")]
        public string Url { get; set; }
    }
}