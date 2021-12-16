using CrunchyBetaDownloader.Api.utils;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses;

public class SearchResponse : Response
{
    [JsonProperty("items")]
    public List<SearchItem?>? SearchItems;

    public class SearchItem
    {
        [JsonProperty("type")]
        public string? Type;
            
        [JsonProperty("items")]
        public List<Item?>? Items;
        [JsonConverter(typeof(JsonPathConverter))]
        public class Item
        {
            [JsonProperty("__links__")]
            public LinksAttributes? Links;
            [JsonProperty("channel_id")]
            public string? ChannelId;
            [JsonProperty("description")]
            public string? Description;
            [JsonProperty("external_id")]
            public string? ExternalId;
            [JsonProperty("id")]
            public string? Id;
            [JsonProperty("linked_resource_key")]
            public string? LinkedResourceKey;
            [JsonProperty("new")]
            public bool New;
            [JsonProperty("new_content")]
            public bool NewContent;
            [JsonProperty("slug_title")]
            public bool SlugTitle;
            [JsonProperty("title")]
            public bool Title;
            [JsonProperty("type")]
            public bool Type;
            [JsonProperty("series_metadata")]
            public SeriesMetaData? SeriesMetadata;
            [JsonProperty("playback")]
            public string? Playback;
        }
    }
}