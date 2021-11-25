
using System.Text.Json.Serialization;

namespace CrunchyBetaDownloader.Api.ResponsesClasses
{
    public abstract class Response
    {                
        //refresh token is only available into mobile requests
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        public override string ToString()
        {
            return $@"
Token type: ""{TokenType}""
Access token:
""{AccessToken}""
Expire in: {ExpiresIn} sec
Refresh token:
""{RefreshToken}""
Country: {Country}
Scope:
""{Scope}""";
        }
    }
}