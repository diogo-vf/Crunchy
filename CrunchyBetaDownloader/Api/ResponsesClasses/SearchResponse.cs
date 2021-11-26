using System.Collections.Generic;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses
{
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
            public class Item
            {
                [JsonProperty("__links__.resource.href")] 
                public string? ResourceLink;
                [JsonProperty("__links__.resource/channel.href")] 
                public string? ChannelLink;
                [JsonProperty("__links__.episode/season.href")] 
                public string? SeasonLink;
                [JsonProperty("__links__.episode/series.href")] 
                public string? SeriesLink;
                [JsonProperty("__links__.streams.href")] 
                public string? StreamsLink;
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
                public SeriesMetaData? Metadata;
                [JsonProperty("playback")]
                public string? Playback;
            }
        }
    }
}