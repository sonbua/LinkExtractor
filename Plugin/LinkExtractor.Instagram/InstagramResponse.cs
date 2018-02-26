using Cqrs;

namespace LinkExtractor.Instagram
{
    public class InstagramResponse : IResponse<InstagramRequest>
    {
        public Medium[] Media { get; set; }

        public class Medium
        {
            public DisplayResource[] DisplayResources { get; set; }

            public bool IsVideo { get; set; }

            public string VideoUrl { get; set; }
        }

        public class DisplayResource
        {
            public string Source { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }
        }
    }
}