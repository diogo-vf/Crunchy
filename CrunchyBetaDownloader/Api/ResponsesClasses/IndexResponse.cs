using System;
using System.Text.Json.Serialization;

namespace CrunchyBetaDownloader.Api.ResponsesClasses
{
    public class IndexResponse : Response
    {
        [JsonPropertyName("bucket")] 
        public string? Bucket { get; set; }
        [JsonPropertyName("policy")] 
        public string? Policy { get; set; }
        [JsonPropertyName("signature")] 
        public string? Signature { get; set; }
        [JsonPropertyName("key_pair_id")] 
        public string? KeyPairId { get; set; }
        [JsonPropertyName("expires")] 
        public DateTime Expires { get; set; }
        [JsonPropertyName("service_available")]
        public bool IsServiceAvailable { get; set; }
        [JsonPropertyName("default_marketing_opt_in")]
        public bool IsDefaultMarketingOptIn { get; set; }
        /*
         * "cms": {
                "bucket": "/US/M3/crunchyroll",
                "policy": "eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9iZXRhLWFwaS5jcnVuY2h5cm9sbC5jb20vY21zL3Y~L1VTL00zL2NydW5jaHlyb2xsLyoiLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE2Mzc5NTkwMTZ9fX1dfQ__",
                "signature": "ZMWvtBgw~JZQS7cTeRxZpXWJUjPn2yHPHZPupgIBUpuKmF55idlsPLbzsXPKBsicZQF87w5dISrsvSCmyskqtgtNlw~5kFGDK3FQ~QGTe~PxGV1WQeE2PypaD7ikDC8HM4WUZ0ClEGyX~uVfSKjKKZeQ5PYRA-whvO7dv7pDdpvNoPP322c~gZvFNDCbJmM0JSaeu2RJwnWtIUJYn-Br6KCDweJBI6LQb7--9ybnPL7GOacus~v7DD6l5rAHyzT6DPfkjYgATxw~69j71ZN~eE0srsn7UT03J5r70DzgwR1be37DOA0rINw2G18IHeOpZH3Ic0FhK0l2FsoIZko6Yw__",
                "key_pair_id": "APKAJMWSQ5S7ZB3MF5VA",
                "expires": "2021-11-26T20:36:56Z"
            },
            "service_available": true,
            "default_marketing_opt_in": true
         */
    }
}