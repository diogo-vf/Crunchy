using CrunchyBetaDownloader.Api.utils;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api;

public class Episode
{
    [JsonProperty("channel_id")] 
    public string? ChannelId;
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
    [JsonProperty("sequence_number")] 
    public int? SequenceNumber;
    [JsonProperty("production_episode_id")]
    public string? ProductionEpisodeId;
    [JsonProperty("title")] 
    public string? Title;
    [JsonProperty("slug_title")] 
    public string? SlugTitle;
    [JsonProperty("description")] 
    public string? Description;
    [JsonProperty("next_episode_id")] 
    public string? NextEpisodeId;
    [JsonProperty("next_episode_title")] 
    public string? NextEpisodeTitle;
    [JsonProperty("hd_flag")] 
    public bool HdFlag;
    [JsonProperty("is_mature")] 
    public bool IsMature;
    [JsonProperty("mature_blocked")] 
    public bool MatureBlocked;
    [JsonProperty("episode_air_date")] 
    public string? EpisodeAirDate;
    [JsonProperty("is_subbed")] 
    public bool IsSubbed;
    [JsonProperty("is_dubbed")] 
    public bool IsDubbed;
    [JsonProperty("is_clip")] 
    public bool IsClip;
    [JsonProperty("seo_title")]
    public string? SeoTitle;
    [JsonProperty("seo_description")] 
    public string? SeoDescription;
    [JsonProperty("available_offline")] 
    public bool AvailableOffline;
    [JsonProperty("media_type")] 
    public string? MediaType;
    [JsonProperty("slug")] 
    public string? Slug;
    [JsonConverter(typeof(TimespanFromMsConverter))] 
    [JsonProperty("duration_ms")]
    public TimeSpan? DurationMs;
    [JsonProperty("is_premium_only")] 
    public bool IsPremiumOnly;
    [JsonProperty("listing_id")] 
    public string? ListingId;
    [JsonProperty("subtitle_locales")] 
    public IList<string>? SubtitleLocales;
    [JsonProperty("playback")] 
    public string? Playback;
    [JsonProperty("availability_notes")] 
    public string? AvailabilityNotes;
        
    /// <summary>
    /// name with special char removed
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    public string SubsFileName { get; set; } = string.Empty;

}