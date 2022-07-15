using Newtonsoft.Json;

namespace Crunchy.Api.ResponsesClasses;

public class EpisodesResponse : Response
{
    [JsonProperty("Items")] public List<Episode>? Episodes;
}