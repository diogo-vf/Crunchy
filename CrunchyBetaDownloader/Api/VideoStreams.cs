using System.Collections.Generic;
using CrunchyBetaDownloader.Api.ResponsesClasses;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api
{
    public class VideoStreams : Response
    {
        [JsonProperty("audio_locale")] 
        public string? MediaId;

        [JsonProperty("subtitles")]
        public Dictionary<string, Subtitle?>? Subtitles;
        [JsonProperty("streams")] 
        public Streams? Streams;
        [JsonProperty("QoS")] 
        public Dictionary<string, string?>? QoS;
    }
    public class Streams
    {
        [JsonProperty("adaptive_dash")]
        public Dictionary<string, Stream?>? AdaptiveDash;
        [JsonProperty("adaptive_hls")]
        public Dictionary<string, Stream?>? AdaptiveHls;
        [JsonProperty("download_hls")]
        public Dictionary<string, Stream?>? DownloadHls;

        public class Stream
        {
            [JsonProperty("hardsub_locale")] 
            public string? HardsubLocale;
            [JsonProperty("url")]
            public string? Url;
            [JsonProperty("vcodec")]
            public string? Vcodec;
        }
    }
    public class Subtitle
    {
            [JsonProperty("locale")] 
            public string? Locale;
            [JsonProperty("url")]
            public string? Url;
            [JsonProperty("format")]
            public string? Format;
    }
}