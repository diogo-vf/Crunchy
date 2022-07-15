using Crunchy.Api.utils;
using Newtonsoft.Json;

namespace Crunchy.Api.ResponsesClasses;

public class ObjectsResponse : Response
{
    [JsonProperty("items")] public List<Item?>? Items;

    public class Item
    {
        [JsonProperty("channel_id")] public string? ChannelId;

        [JsonProperty("description")] public string? Description;

        [JsonProperty("episode_metadata")] public Episode? Episode;

        [JsonProperty("external_id")] public string? ExternalId;

        [JsonProperty("id")] public string? Id;

        [JsonProperty("linked_resource_key")] public string? LinkedResourceKey;

        [JsonProperty("__links__")] public LinksAttributes? Links;

        [JsonProperty("playback")] public string? Playback;

        [JsonProperty("series_metadata")] public SeriesMetaData? SeriesMetaData;

        [JsonProperty("slug_title")] public string? SlugTitle;

        [JsonProperty("title")] public string? Title;

        [JsonProperty("type")] public string? Type;
    }
}