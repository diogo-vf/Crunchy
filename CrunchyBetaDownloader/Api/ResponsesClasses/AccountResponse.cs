using System;
using System.Text.Json.Serialization;

namespace CrunchyBetaDownloader.Api.ResponsesClasses
{
    public class AccountResponse : Response
    {
        [JsonPropertyName("account_id")]
        public string? AccountId { get; set; }
        [JsonPropertyName("external_id")]
        public string? ExternalId { get; set; }
        [JsonPropertyName("email_verified")]
        public string? EmailVerified { get; set; }
        [JsonPropertyName("created")]
        public DateTime? CreatedOn { get; set; }
    }
}