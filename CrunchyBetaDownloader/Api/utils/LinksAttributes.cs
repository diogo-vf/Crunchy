using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.utils
{
    public class LinksAttributes
    {
        [JsonProperty("episode/season")] 
        public Link? SeasonLink;
        [JsonProperty("episode/series")] 
        public Link? SeriesLink;
        [JsonProperty("resource")] 
        public Link? ResourceLink;
        [JsonProperty("resource/channel")] 
        public Link? ChannelLink;
        [JsonProperty("streams")] 
        public Link? StreamsLink;

        public class Link
        {
            [JsonProperty("href")] 
            public string? Href;
        }
    }
}