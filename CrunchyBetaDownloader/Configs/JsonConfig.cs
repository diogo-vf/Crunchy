using System;
using System.IO;
using System.Text.Json.Nodes;

namespace CrunchyBetaDownloader.Configs
{
    public class JsonConfig : IConfigData
    {
        public string? Username { get; init; }
        public string? Password { get; init; }
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

        private string? GetJsonValue(ref JsonNode? json, string param) => json?[param]?.GetValue<string>();

        public static JsonObject PreRequirementJson() => new()
        {
            ["username"] = string.Empty,
            ["password"] = string.Empty,
            ["download_destination"] = Path.Join(".", "Downloads"),
            ["show_log"] = true,
            ["log_destination"] = Path.Join(".", "CrunchyBetaDownloader.log")
        };
    }
}