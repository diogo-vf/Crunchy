using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrunchyBetaDownloader.Api.ResponsesClasses;
using CrunchyBetaDownloader.Configs;

namespace CrunchyBetaDownloader
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                foreach (string url in args.Where(url => !Downloader.IsAvailableLink(url)))
                {
                    throw new Exception($"{url} isn't a beta url of crunchyroll");
                }
                string jsonPath =
                    Path.Join(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? ".",
                        "config.json");
                CreateJsonConfigFile(jsonPath);
                if(args == null || args.Length == 0) throw new Exception("no urls given");

                IConfigData config = new JsonConfig(jsonPath);
                Downloader downloader = new(config);
                IndexResponse? indexResponse = await downloader.Login();
                await downloader.Download(indexResponse, args, "fr-FR");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
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