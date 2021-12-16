using System.Text.Json.Nodes;

namespace CrunchyBetaDownloader.Configs;

public class JsonConfig : IConfigData
{
    public string? Username { get; init; }
    public string? Password { get; init; }
    public string SpacesCharacter { get; init; }
    public string NameFormat { get; init; }
    public bool ShowLog { get; init; }
    public string DownloadDestination { get; init; }
    public string LogDestination { get; init; }

    public bool AvailableCredentials() => !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password));

    public JsonConfig(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        JsonNode? json = JsonNode.Parse(jsonString);
        Username = GetJsonValue(ref json, "username");
        Password = GetJsonValue(ref json, "password");
        SpacesCharacter = GetJsonValue(ref json, "spaces_character") ?? throw new Exception("config file incomplete");
        NameFormat = GetJsonValue(ref json, "name_format") ?? throw new Exception("config file incomplete");
        ShowLog = json?["show_log"]?.GetValue<bool>() ?? false;
        DownloadDestination = string.IsNullOrEmpty(GetJsonValue(ref json, "download_destination"))
            ? Path.Join(".", "Downloads")
            : GetJsonValue(ref json, "download_destination")!;
        LogDestination =
            Path.Join(
                string.IsNullOrEmpty(json?["log_destination"]?.ToString())
                    ? "."
                    : GetJsonValue(ref json, "log_destination"), "CrunchyBetaDownloader.log");
    }

    private static string? GetJsonValue(ref JsonNode? json, string param) => json?[param]?.GetValue<string>();

    public static JsonObject PreRequirementJson() => new()
    {
        ["username"] = string.Empty,
        ["password"] = string.Empty,
        ["spaces_character"] = " ",
        ["name_format"] = "{name} - Saison {seasonNumber} - {episodeNumber}",
        ["download_destination"] = Path.Join(".", "Downloads"),
        ["show_log"] = true,
        ["log_destination"] = Path.Join(".", "CrunchyBetaDownloader.log")
    };
}