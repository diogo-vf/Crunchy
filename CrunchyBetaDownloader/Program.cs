using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
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
            if (config.Username is null || config.Password is null) throw new Exception();
            ApiCrunchyBeta api = new();
            Console.WriteLine(await api.Token(config.Username, config.Username));
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