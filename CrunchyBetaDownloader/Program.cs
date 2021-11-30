using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
            
            if (string.IsNullOrEmpty(config.Username) || string.IsNullOrEmpty(config.Password)) throw new Exception();
            ApiCrunchyBeta api = new();
            Response? a = await api.Login(config.Username, config.Password);
            IndexResponse? b = await api.Index(a);
            ObjectsResponse? c = await api.GetObject(b, "https://beta.crunchyroll.com/fr/watch/GJWU2JWE9/the-path-taken?modal=premium",
                "fr-FR");
            Console.WriteLine(a);
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