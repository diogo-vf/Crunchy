using Crunchy.Api.utils;
using Newtonsoft.Json;

namespace Crunchy.Api;

public class SeriesMetaData
{
    [JsonConverter(typeof(TimespanFromMsConverter))] [JsonProperty("duration_ms")]
    public TimeSpan? Duration;

    [JsonProperty("episode")] public double? EpisodeNumber;

    [JsonProperty("is_premium_only")] public bool IsPremiumOnly;

    [JsonProperty("season_id")] public string? SeasonId;

    [JsonProperty("season_number")] public int? SeasonNumber;

    [JsonProperty("season_slug_title")] public string? SeasonSlugTitle;

    [JsonProperty("season_title")] public string? SeasonTitle;

    [JsonProperty("series_id")] public string? SeriesId;

    [JsonProperty("series_slug_title")] public string? SeriesSlugTitle;

    [JsonProperty("series_title")] public string? SeriesTitle;

    [JsonProperty("episode_count")] public double? TotalEpisodesExpected;

    [JsonProperty("season_count")] public double? TotalEpisodesOut;
}