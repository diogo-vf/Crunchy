using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CrunchyBetaDownloader.Api;
using CrunchyBetaDownloader.Api.ResponsesClasses;
using CrunchyBetaDownloader.Configs;
using CrunchyBetaDownloader.FFtools;

namespace CrunchyBetaDownloader
{
    public class Downloader
    {
        #region attributes

        private IConfigData Config { get; }
        private ApiCrunchyBeta Api { get; }
        private HttpClient Client { get; }

        #endregion

        public Downloader(IConfigData config)
        {
            Api = new ApiCrunchyBeta();
            Client = new HttpClient();
            Config = config;
            CreateNecessaryFilesAndFolders();
        }

        private void CreateNecessaryFilesAndFolders()
        {
            if (string.IsNullOrEmpty(Config.Username) || string.IsNullOrEmpty(Config.Password))
                throw new Exception("Please complete json near your application");

            if (!Directory.Exists(Config.DownloadDestination)) Directory.CreateDirectory(Config.DownloadDestination);
        }

        public async Task<IndexResponse?> Login()
        {
            ProfileResponse? profileResponse = await Api.Login(Config.Username, Config.Password);
            return await Api.Index(profileResponse);
        }

        public async Task Download(IndexResponse? indexResponse, IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                if (!IsEpisodeUrl(url)) continue;
                ObjectsResponse? episodeResponse = await Api.GetObject(indexResponse, url, "fr-FR");
                ObjectsResponse.Item? episode = episodeResponse?.Items?.First();

                if (episode?.Playback is null ||
                    episode.EpisodeMetaData is null ||
                    episode.EpisodeMetaData.SeriesTitle is null &&
                    episode.EpisodeMetaData.SeasonTitle is null)
                    throw new Exception("no episode incomplete");

                string episodeName = episode.EpisodeMetaData?.SeriesTitle ??
                                     episode.EpisodeMetaData?.SeasonTitle!;

                VideoStreams? videoStreams = await Api.CallPlayback(episode.Playback);

                string cleanName =
                    new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]")
                        .Replace(episodeName, Config.SpacesCharacter);
                string seasonNumber = (episode.EpisodeMetaData?.SeasonNumber?.ToString() ?? "1")
                    .PadLeft(2,'0');
                string episodeNumber = (episode.EpisodeMetaData?.EpisodeNumber?.ToString() ?? "1")
                    .PadLeft(2,'0');
                
                episode.EpisodeMetaData!.FileName = Config.NameFormat
                    .Replace("{name}", cleanName, StringComparison.OrdinalIgnoreCase)
                    .Replace("{seasonNumber}",seasonNumber, StringComparison.OrdinalIgnoreCase)
                    .Replace("{episodeNumber}",episodeNumber, StringComparison.OrdinalIgnoreCase)
                    .Replace(" ", Config.SpacesCharacter);

                await DownloadSubs(Config.DownloadDestination, videoStreams, episode.EpisodeMetaData);
                await DownloadVideo(Config.DownloadDestination, videoStreams, episode.EpisodeMetaData);
                await CreateMkv(Config.DownloadDestination, episode.EpisodeMetaData);
            }
        }

        private async Task<string> DownloadWebFile(string url) =>
            await (await Client.GetAsync(url)).Content.ReadAsStringAsync();

        private async Task DownloadSubs(string folderPath, VideoStreams? videoStreams, EpisodeMetaData episode)
        {
            string[] defaultLanguagesSubs = { "fr-FR", "en-US" };
            int languageChosen = 0;
            string? subsUrl = null;
            
            for (int i = 0; i < defaultLanguagesSubs.Length; i++)
            {
                try
                {
                    subsUrl = videoStreams?.Subtitles?[defaultLanguagesSubs[languageChosen = i]]?.Url;
                }
                catch
                {
                    // ignored
                }

                if(subsUrl is not null) break;
            }
            
            if (subsUrl is null) throw new Exception("subtitle not found");

            Console.WriteLine("download the subtitles...");
            episode.SubsFileName = $"{episode.FileName}[{defaultLanguagesSubs[languageChosen]}]";
            string filePath = Path.Join(folderPath, $"{episode.SubsFileName}.ass");

            string subs = await DownloadWebFile(subsUrl);
            await File.WriteAllTextAsync(filePath, subs);
            Console.WriteLine("successful");
        }

        private async Task DownloadVideo(string folderPath, VideoStreams? videoStreams, EpisodeMetaData episode)
        {
            string? streamUrl = SearchInM3U8TheBestDownloadUrl(videoStreams?.Streams?.AdaptiveHls?[""]?.Url);

            if (streamUrl is null) throw new Exception("subtitle not found");

            string videoPath = $"{Path.Join(folderPath, $"{episode.FileName}.mp4")}";
            string audioPath = $"{Path.Join(folderPath, $"{episode.FileName}.aac")}";

            //download video with ffmpeg
            Console.WriteLine(@"download video & audio...");
            FFmpeg ffmpeg = new();
            ffmpeg.OnProgress += (_, args) =>
            {
                int percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                Console.Write($"\r[{args.Duration} / {args.TotalLength}] {percent}% {episode.FileName}");
            };

            FFmpegResult result = await ffmpeg
                .SetMultiThread(true)
                .Start(
                    $@"-y -i ""{streamUrl}"" -q:a 0 -map a -map_metadata 0 -map_metadata:s:a 0:s:a -metadata:s:a:0 language=jpn -metadata:s:a:0 title=Japonais -acodec copy ""{audioPath}"" -q:v 0 -map v -map_metadata 0 -map_metadata:s:v 0:s:v -c copy ""{videoPath}""",
                    CancellationToken.None);
            Console.WriteLine("\n");
            Console.WriteLine(result);
            Console.WriteLine("successful");
        }

        private string? SearchInM3U8TheBestDownloadUrl(string? m3U8Url)
        {
            if (m3U8Url is null) return null;
            string m3U8 = DownloadWebFile(m3U8Url).GetAwaiter().GetResult();
            MatchCollection matches = new Regex(@"RESOLUTION=\d+x\d+").Matches(m3U8);
            int indexBestResolution = 0;
            int bestResolution = 0;

            for (int i = 0; i < matches.Count; i++)
            {
                string strResolution = matches[i].Value[(matches[i].Value.LastIndexOf("x", StringComparison.Ordinal) + 1)..];
                int resolution = int.Parse(strResolution);
                //todo after tests change <= to >=
                if (bestResolution >= resolution && bestResolution != 0) continue;

                bestResolution = resolution;
                indexBestResolution = i + 1;
            }

            Console.WriteLine($"the best resolution  is: {bestResolution}p");
            return m3U8.Replace("\r", "").Split('\n')[indexBestResolution * 2];
        }

        private static async Task CreateMkv(string folderPath, EpisodeMetaData episode)
        {
            string videoPath = Path.Join(folderPath, $"{episode.FileName}.mp4");
            string audioPath = Path.Join(folderPath, $"{episode.FileName}.aac");
            string subPath = Path.Join(folderPath, $"{episode.SubsFileName}.ass");
            string mkvPath = Path.Join(folderPath, $"{episode.FileName}.mkv");

            Console.WriteLine($@"merging video, audio and subs to create ""{mkvPath}""");

            await new FFmpeg()
                .Start(
                    $@"-y -i ""{videoPath}"" -i ""{audioPath}"" -i ""{subPath}"" -map 0 -map 1 -map 2 -metadata:s:v:0 title=Crunchyroll -metadata:s:a:0 language=jpn -metadata:s:a:0 title=Japonais -metadata:s:a:0 language=jpn -metadata:s:a:0 title=Japonais -metadata:s:s:0 language=fre -metadata:s:s:0 title=Français -disposition:s:s:0 default -c copy ""{mkvPath}""",
                    CancellationToken.None);
            Console.WriteLine("successful");

            //clear mp4 and ass files after the merging
            // File.Delete(videoPath);
            // File.Delete(audioPath);
            // File.Delete(subPath);
        }

        private static bool IsEpisodeUrl(string url)
        {
            const string regex = @"https?:\/\/(www\.)?beta\.crunchyroll\.com\/[a-zA-Z]{2}\/watch\/\w+(\/\w+)?";
            return new Regex(regex).IsMatch(url);
        }
    }
}