using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses;

public class SeasonsResponse : Response
{
    [JsonProperty("items")] 
    public List<SeasonItem?>? SeasonItems;

    public class SeasonItem
    {
        [JsonProperty("id")]
        public string? Id;
        [JsonProperty("channel_id")]
        public string? ChannelId;
        [JsonProperty("title")]
        public string? Title;
        [JsonProperty("slug_title")]
        public string? SlugTitle;
        [JsonProperty("series_id")]
        public string? SeriesId;
        [JsonProperty("season_number")]
        public int? SeasonNumber;
        [JsonProperty("is_complete")]
        public bool IsComplete;
        [JsonProperty("description")]
        public string? Description;
        [JsonProperty("is_mature")]
        public bool IsMature;
        [JsonProperty("mature_blocked")]
        public string? MatureBlocked;
        [JsonProperty("is_subbed")]
        public bool IsSubbed;
        [JsonProperty("is_dubbed")]
        public bool IsDubbed;
        [JsonProperty("is_simulcast")]
        public string? IsSimulcast;
        [JsonProperty("seo_title")]
        public string? SeoTitle;
        [JsonProperty("seo_description")]
        public string? SeoDescription;
        [JsonProperty("availability_notes")]
        public string? AvailabilityNotes;
    }
}