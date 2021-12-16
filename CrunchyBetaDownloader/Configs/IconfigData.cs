namespace CrunchyBetaDownloader.Configs;

public interface IConfigData
{
    public string? Username  {get; init; }
    public string? Password {get; init; }

    public string SpacesCharacter { get; init; }
    public string NameFormat { get; init; }

    public bool ShowLog {get; init; }
    public string DownloadDestination {get; init; }
    public string LogDestination {get; init; }

    public bool AvailableCredentials();
}