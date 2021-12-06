using System;
using CrunchyBetaDownloader.Api.utils;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses
{
    public abstract class Response
    {                
        //refresh token is only available into mobile requests
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; set; }
        [JsonProperty("token_type")]
        public string? TokenType { get; set; }
        [JsonProperty("scope")]
        public string? Scope { get; set; }
        [JsonProperty("country")]
        public string? Country { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public override string ToString()
        {
            return $@"
Access token:
""{AccessToken}""
Token type: ""{TokenType}""
Expire in: {ExpiresIn} sec
Expire at: {ExpiresAt?.ToString()}
Refresh token: ""{RefreshToken}""
Country: ""{Country}""
Scope: ""{Scope}""";
        }
    }
}