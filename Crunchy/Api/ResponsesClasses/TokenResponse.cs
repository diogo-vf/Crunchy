using Newtonsoft.Json;

namespace Crunchy.Api.ResponsesClasses;

public class TokenResponse : Response
{
    //account id is only available when we use etp_rt or mobile login request
    [JsonProperty("account_id")] public string? AccountId { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()}\nAccount id: \"{AccountId}\"";
    }
}