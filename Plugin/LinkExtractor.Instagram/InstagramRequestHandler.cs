using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LinkExtractor.Core;
using Newtonsoft.Json;

namespace LinkExtractor.Instagram
{
    public class InstagramRequestHandler : IRequestHandler<InstagramRequest, InstagramResponse>
    {
        private const string _WINDOW_SHARED_DATA_VARIABLE_KEY = "window._sharedData = ";

        public virtual async Task<InstagramResponse> HandleAsync(InstagramRequest request)
        {
            var htmlWeb = new HtmlWeb();

            var htmlDocument = await htmlWeb.LoadFromWebAsync(request.Url);

            var sharedDataScript =
                htmlDocument
                    .DocumentNode
                    .Descendants("script")
                    .Single(x => x.InnerText.StartsWith(_WINDOW_SHARED_DATA_VARIABLE_KEY))
                    .InnerText;

            var jsonLength = sharedDataScript.Length - _WINDOW_SHARED_DATA_VARIABLE_KEY.Length - 1;

            var json = sharedDataScript.Substring(_WINDOW_SHARED_DATA_VARIABLE_KEY.Length, jsonLength);

            var model = JsonConvert.DeserializeObject<InstagramSharedDataModel>(json);

            var originalDisplayResources = model
                .entry_data
                .PostPage
                .First()
                .graphql
                .shortcode_media
                .display_resources;

            var displayResources =
                originalDisplayResources
                    .Select(
                        x => new InstagramResponse.DisplayResource
                        {
                            Source = x.src,
                            Width = x.config_width,
                            Height = x.config_height
                        })
                    .ToArray();

            return new InstagramResponse
            {
                Media = new[]
                {
                    new InstagramResponse.Medium
                    {
                        DisplayResources = displayResources
                    }
                }
            };
        }
    }
}