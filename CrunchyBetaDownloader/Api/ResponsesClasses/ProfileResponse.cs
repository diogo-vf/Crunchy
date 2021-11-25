using System.Text.Json.Serialization;

namespace CrunchyBetaDownloader.Api.ResponsesClasses
{
    public class ProfileResponse : Response
    {
        [JsonPropertyName("preferred_content_subtitle_language")]
        public string? PreferredContentSubtitleLanguage { get; set; }
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        public override string ToString()
        {
            return $@"email: ""{Email}""
Username: ""{Username}""
{base.ToString()}
Preferred content subtitle language: ""{PreferredContentSubtitleLanguage}""";
        }
    }
}