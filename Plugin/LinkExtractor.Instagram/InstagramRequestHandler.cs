using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cqrs;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace LinkExtractor.Instagram
{
    public class InstagramRequestHandler : BaseRequestHandler<InstagramRequest, InstagramResponse>
    {
        private const string _WINDOW_SHARED_DATA_VARIABLE_KEY = "window._sharedData = ";

        public override async Task<InstagramResponse> HandleAsync(InstagramRequest request)
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

            var media = ExtractAllMedia(model.entry_data.PostPage.First().graphql.shortcode_media);

            return new InstagramResponse
            {
                Media = media.ToArray()
            };
        }

        private IEnumerable<InstagramResponse.Medium> ExtractAllMedia(
            InstagramSharedDataModel.ShortcodeMedia primaryMediumModel)
        {
            var primaryMedium = ExtractMedium(primaryMediumModel);

            yield return primaryMedium;

            var childMedia = ExtractChildMedia(primaryMediumModel.edge_sidecar_to_children);

            foreach (var childMedium in childMedia)
            {
                yield return childMedium;
            }
        }

        private InstagramResponse.Medium ExtractMedium(InstagramSharedDataModel.Node node)
        {
            var originalDisplayResources = node.display_resources;

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

            return new InstagramResponse.Medium
            {
                DisplayResources = displayResources,
                IsVideo = node.is_video,
                VideoUrl = node.video_url
            };
        }

        private IEnumerable<InstagramResponse.Medium> ExtractChildMedia(
            InstagramSharedDataModel.EdgeSidecarToChildren children)
        {
            if (children == null)
            {
                yield break;
            }

            foreach (var edge in children.edges)
            {
                yield return ExtractMedium(edge.node);
            }
        }
    }
}