using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api.utils;

public class TimespanFromMsConverter : JsonConverter<TimeSpan>
{
    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        writer.WriteValue(value.TotalMilliseconds);
    }

    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        long? ms = reader.Value as long?;
        return TimeSpan.FromMilliseconds(ms ?? 0);
    }
}