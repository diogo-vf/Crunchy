using Crunchy.Api.utils;
using Newtonsoft.Json;

namespace Crunchy.Api;

public class Episode
{
    [JsonProperty("availability_notes")] public string? AvailabilityNotes;

    [JsonProperty("available_offline")] public bool AvailableOffline;

    [JsonProperty("channel_id")] public string? ChannelId;

    [JsonProperty("description")] public string? Description;

    [JsonConverter(typeof(TimespanFromMsConverter))] [JsonProperty("duration_ms")]
    public TimeSpan? DurationMs;

    [JsonProperty("episode_air_date")] public string? EpisodeAirDate;

    [JsonProperty("episode")] public double? EpisodeNumber;

    [JsonProperty("hd_flag")] public bool HdFlag;

    [JsonProperty("is_clip")] public bool IsClip;

    [JsonProperty("is_dubbed")] public bool IsDubbed;

    [JsonProperty("is_mature")] public bool IsMature;

    [JsonProperty("is_premium_only")] public bool IsPremiumOnly;

    [JsonProperty("is_subbed")] public bool IsSubbed;

    [JsonProperty("listing_id")] public string? ListingId;

    [JsonProperty("mature_blocked")] public bool MatureBlocked;

    [JsonProperty("media_type")] public string? MediaType;

    [JsonProperty("next_episode_id")] public string? NextEpisodeId;

    [JsonProperty("next_episode_title")] public string? NextEpisodeTitle;

    [JsonProperty("playback")] public string? Playback;

    [JsonProperty("production_episode_id")]
    public string? ProductionEpisodeId;

    [JsonProperty("season_id")] public string? SeasonId;

    [JsonProperty("season_number")] public int? SeasonNumber;

    [JsonProperty("season_slug_title")] public string? SeasonSlugTitle;

    [JsonProperty("season_title")] public string? SeasonTitle;

    [JsonProperty("seo_description")] public string? SeoDescription;

    [JsonProperty("seo_title")] public string? SeoTitle;

    [JsonProperty("sequence_number")] public int? SequenceNumber;

    [JsonProperty("series_id")] public string? SeriesId;

    [JsonProperty("series_slug_title")] public string? SeriesSlugTitle;

    [JsonProperty("series_title")] public string? SeriesTitle;

    [JsonProperty("slug")] public string? Slug;

    [JsonProperty("slug_title")] public string? SlugTitle;

    [JsonProperty("subtitle_locales")] public IList<string>? SubtitleLocales;

    [JsonProperty("title")] public string? Title;

    /// <summary>
    ///     name with special char removed
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    public string SubsFileName { get; set; } = string.Empty;
}