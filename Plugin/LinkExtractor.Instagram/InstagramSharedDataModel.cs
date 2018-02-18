using System.Collections.Generic;

namespace LinkExtractor.Instagram
{
    public class InstagramSharedDataModel
    {
        public EntryData entry_data { get; set; }

        public class DisplayResource
        {
            public string src { get; set; }

            public int config_width { get; set; }

            public int config_height { get; set; }
        }

        public class Node
        {
            public string shortcode { get; set; }

            public IList<DisplayResource> display_resources { get; set; }

            public string video_url { get; set; }

            public bool is_video { get; set; }
        }

        public class Edge
        {
            public Node node { get; set; }
        }

        public class EdgeSidecarToChildren
        {
            public IList<Edge> edges { get; set; }
        }

        public class ShortcodeMedia : Node
        {
            public EdgeSidecarToChildren edge_sidecar_to_children { get; set; }
        }

        public class Graphql
        {
            public ShortcodeMedia shortcode_media { get; set; }
        }

        public class PostPage
        {
            public Graphql graphql { get; set; }
        }

        public class EntryData
        {
            public IList<PostPage> PostPage { get; set; }
        }
    }
}