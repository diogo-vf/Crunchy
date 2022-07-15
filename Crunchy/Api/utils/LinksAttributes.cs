using Newtonsoft.Json;

namespace Crunchy.Api.utils;

public class LinksAttributes
{
    [JsonProperty("resource/channel")] public Link? ChannelLink;

    [JsonProperty("resource")] public Link? ResourceLink;

    [JsonProperty("episode/season")] public Link? SeasonLink;

    [JsonProperty("episode/series")] public Link? SeriesLink;

    [JsonProperty("streams")] public Link? StreamsLink;

    public class Link
    {
        [JsonProperty("href")] public string? Href;
    }
}