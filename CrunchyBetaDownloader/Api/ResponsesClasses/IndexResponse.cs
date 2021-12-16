using CrunchyBetaDownloader.Api.utils;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses;

[JsonConverter(typeof(JsonPathConverter))]
public class IndexResponse : Response
{
    [JsonProperty("cms.bucket")] 
    public string? Bucket { get; set; }
    [JsonProperty("cms.policy")] 
    public string? Policy { get; set; }
    [JsonProperty("cms.signature")] 
    public string? Signature { get; set; }
    [JsonProperty("cms.key_pair_id")] 
    public string? KeyPairId { get; set; }
    [JsonProperty("cms.expires")] 
    public DateTime Expires { get; set; }
    [JsonProperty("service_available")]
    public bool IsServiceAvailable { get; set; }
    [JsonProperty("default_marketing_opt_in")]
    public bool IsDefaultMarketingOptIn { get; set; }
        
    public override string ToString()
    {
        return $@"{base.ToString()}
Bucket: ""{Bucket}""
Policy: 
""{Policy}""
Signature:
""{Signature}""
Key pair id: ""{KeyPairId}""
Expires in: {Expires}
Service is available: {IsServiceAvailable}
Default marketing is opt in: {IsDefaultMarketingOptIn}
";
    }
}