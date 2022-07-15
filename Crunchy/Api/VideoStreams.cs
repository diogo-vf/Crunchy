using Crunchy.Api.ResponsesClasses;
using Newtonsoft.Json;

namespace Crunchy.Api;

public class VideoStreams : Response
{
    [JsonProperty("audio_locale")] public string? MediaId;

    [JsonProperty("QoS")] public Dictionary<string, string?>? QoS;

    [JsonProperty("streams")] public Streams? Streams;

    [JsonProperty("subtitles")] public Dictionary<string, Subtitle?>? Subtitles;
}

public class Streams
{
    [JsonProperty("adaptive_dash")] public Dictionary<string, Stream?>? AdaptiveDash;

    [JsonProperty("adaptive_hls")] public Dictionary<string, Stream?>? AdaptiveHls;

    [JsonProperty("download_hls")] public Dictionary<string, Stream?>? DownloadHls;

    public class Stream
    {
        [JsonProperty("hardsub_locale")] public string? HardsubLocale;

        [JsonProperty("url")] public string? Url;

        [JsonProperty("vcodec")] public string? Vcodec;
    }
}

public class Subtitle
{
    [JsonProperty("format")] public string? Format;

    [JsonProperty("locale")] public string? Locale;

    [JsonProperty("url")] public string? Url;
}