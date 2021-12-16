using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses;

public class ProfileResponse : Response
{
    [JsonProperty("preferred_content_subtitle_language")]
    public string? PreferredContentSubtitleLanguage { get; set; }
    [JsonProperty("username")]
    public string? Username { get; set; }
    [JsonProperty("email")]
    public string? Email { get; set; }

    public override string ToString()
    {
        return $@"{base.ToString()}
email: ""{Email}""
Username: ""{Username}""
Preferred content subtitle language: ""{PreferredContentSubtitleLanguage}""";
    }
}