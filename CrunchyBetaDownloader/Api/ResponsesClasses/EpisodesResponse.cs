using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.ResponsesClasses;

public class EpisodesResponse : Response
{
    [JsonProperty("Items")]
    public List<Episode>? Episodes;

}