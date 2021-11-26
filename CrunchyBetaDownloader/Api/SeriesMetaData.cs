using System;
using CrunchyBetaDownloader.Api.utils;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api
{
    public class SeriesMetaData
    {
        [JsonConverter(typeof(TimespanConverter))]
        [JsonProperty("duration_ms")]
        public TimeSpan? Duration;
        [JsonProperty("episode_number")]
        public int? EpisodeNumber;
        [JsonProperty("season_number")]
        public int? SeasonNumber;
        [JsonProperty("is_premium_only")]
        public bool IsPremiumOnly;
        [JsonProperty("season_id")]
        public string? SeasonId;
        [JsonProperty("season_slug_title")]
        public string? SeasonSlugTitle;
        [JsonProperty("season_title")]
        public string? SeasonTitle;
        [JsonProperty("series_id")]
        public string? SeriesId;
        [JsonProperty("series_slug_title")]
        public string? SeriesSlugTitle;
        [JsonProperty("series_title")]
        public string? SeriesTitle;
        
    }
}