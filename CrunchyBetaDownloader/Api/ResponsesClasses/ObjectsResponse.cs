using System.Collections.Generic;
using CrunchyBetaDownloader.Api.utils;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses
{
    
    public class ObjectsResponse : Response
    {
        [JsonProperty("items")]
        public List<Item?>? Items;
        public class Item
        {
            [JsonProperty("__links__")]
            public LinksAttributes? Links;
            [JsonProperty("id")]
            public string? Id;
            [JsonProperty("external_id")]
            public string? ExternalId;
            [JsonProperty("channel_id")]
            public string? ChannelId;
            [JsonProperty("title")]
            public string? Title;
            [JsonProperty("slug_title")]
            public string? SlugTitle;
            [JsonProperty("description")]
            public string? Description;
            [JsonProperty("type")]
            public string? Type;
            [JsonProperty("episode_metadata")] 
            public Episode? Episode;
            [JsonProperty("series_metadata")] 
            public SeriesMetaData? SeriesMetaData;
            [JsonProperty("playback")]
            public string? Playback;
            [JsonProperty("linked_resource_key")]
            public string? LinkedResourceKey;
        }
    }
}