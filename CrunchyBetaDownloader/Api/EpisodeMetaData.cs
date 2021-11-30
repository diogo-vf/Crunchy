using System;
using System.Collections.Generic;
using CrunchyBetaDownloader.Api.utils;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api
{
    public class EpisodeMetaData
    {
        [JsonProperty("series_id")]
        public string? SeriesId;
        [JsonProperty("series_title")]
        public string? SeriesTitle;
        [JsonProperty("series_slug_title")]
        public string? SeriesSlugTitle;
        [JsonProperty("season_id")]
        public string? SeasonId;
        [JsonProperty("season_title")]
        public string? SeasonTitle;
        [JsonProperty("season_slug_title")]
        public string? SeasonSlugTitle;
        [JsonProperty("season_number")]
        public int? SeasonNumber;
        [JsonProperty("episode")]
        public double? EpisodeNumber;
        [JsonConverter(typeof(TimespanFromMsConverter))]
        [JsonProperty("duration_ms")]
        public TimeSpan? Duration;
        [JsonProperty("is_premium_only")]
        public bool IsPremiumOnly;
        [JsonProperty("subtitle_locales")] 
        public IList<string>? Subtitles;


    }
}