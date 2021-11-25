using System.Text.Json.Serialization;

namespace CrunchyBetaDownloader.Api.ResponsesClasses
{
    public class TokenResponse : Response
    {
        //account id is only available when we use etp_rt or mobile login request
        [JsonPropertyName("account_id")]
        public string? AccountId { get; set; }

        public override string ToString()
        {
            return $"Account id: \"{AccountId}\"\n{base.ToString()}";
        }
    }
}