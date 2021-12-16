using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses;

public class AccountResponse : Response
{
    [JsonProperty("account_id")]
    public string? AccountId { get; set; }
    [JsonProperty("external_id")]
    public string? ExternalId { get; set; }
    [JsonProperty("email_verified")]
    public bool EmailVerified { get; set; }
    [JsonProperty("created")]
    public DateTime? CreatedOn { get; set; }
        
    public override string ToString()
    {
        return $@"{base.ToString()}
Account id: ""{AccountId}""
ExternalId: ""{ExternalId}""
EmailVerified: {EmailVerified}
Created on: {CreatedOn}";
    }
}