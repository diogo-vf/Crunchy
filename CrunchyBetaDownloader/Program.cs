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

namespace CrunchyBetaDownloader
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string jsonPath = Path.Join(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? ".","config.json");
            CreateJsonConfigFile(jsonPath);
            IConfigData config = new JsonConfig(jsonPath);
            if (string.IsNullOrEmpty(config.Username) || string.IsNullOrEmpty(config.Password)) throw new Exception($"Please complete json inside {jsonPath}");
            
            ApiCrunchyBeta api = new();
            
            ProfileResponse? profileResponse = await api.Login(config.Username, config.Password);
            IndexResponse? indexResponse = await api.Index(profileResponse);
            foreach (var url in args)
            {
                if(!IsEpisodeUrl(url)) continue;
                ObjectsResponse? c = await api.GetObject(indexResponse, "https://beta.crunchyroll.com/fr/watch/GJWU2JWE9/the-path-taken?modal=premium",
                    "fr-FR");
                VideoStreams? d = await api.CallPlayback(c?.Items?.First()?.Playback ?? string.Empty, c);
                Console.WriteLine(d?.Streams?.AdaptiveDash?[""]?.Url);
            }
        }

        private static bool IsEpisodeUrl(string url)
        {
            const string regex = @"https?:\/\/(www\.)?beta\.crunchyroll\.com\/[a-zA-Z]{2}\/watch\/\w+(\/\w+)?";
            return new Regex(regex).IsMatch(url);
        }
        /// <summary>
        /// Create json config if not exists with pre-requirements
        /// </summary>
        /// <param name="path"></param>
        private static void CreateJsonConfigFile(string path)
        {
            if (File.Exists(path)) return;
            File.WriteAllText(path, JsonConfig.PreRequirementJson().ToString());
        }
    }
}