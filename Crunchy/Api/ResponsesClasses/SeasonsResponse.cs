using Newtonsoft.Json;

namespace Crunchy.Api.ResponsesClasses;

public class SeasonsResponse : Response
{
    [JsonProperty("items")] public List<SeasonItem?>? SeasonItems;

    public class SeasonItem
    {
        [JsonProperty("availability_notes")] public string? AvailabilityNotes;

        [JsonProperty("channel_id")] public string? ChannelId;

        [JsonProperty("description")] public string? Description;

        [JsonProperty("id")] public string? Id;

        [JsonProperty("is_complete")] public bool IsComplete;

        [JsonProperty("is_dubbed")] public bool IsDubbed;

        [JsonProperty("is_mature")] public bool IsMature;

        [JsonProperty("is_simulcast")] public string? IsSimulcast;

        [JsonProperty("is_subbed")] public bool IsSubbed;

        [JsonProperty("mature_blocked")] public string? MatureBlocked;

        [JsonProperty("season_number")] public int? SeasonNumber;

        [JsonProperty("seo_description")] public string? SeoDescription;

        [JsonProperty("seo_title")] public string? SeoTitle;

        [JsonProperty("series_id")] public string? SeriesId;

        [JsonProperty("slug_title")] public string? SlugTitle;

        [JsonProperty("title")] public string? Title;
    }
}