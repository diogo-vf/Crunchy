using System.Diagnostics;
using CrunchyBetaDownloader.Api.ResponsesClasses;
using CrunchyBetaDownloader.Configs;

namespace CrunchyBetaDownloader;

public class Program
{
    public static async Task Main(string[] args)
    {
        ProcessPriorityClass? ffmpegPriority = null;
        if (args[0].Contains("-p", StringComparison.OrdinalIgnoreCase))
        {
            if (args[1].Length == 1)
            {
                ffmpegPriority = args[1].ToLower() switch
                {
                    "r" => ProcessPriorityClass.RealTime,
                    "h" => ProcessPriorityClass.High,
                    _ => ProcessPriorityClass.Normal
                };
                args = args.Where((_, index) => index is not (0 or 1)).ToArray();
            }
            else
            {
                args = args.Where((_, index) => index is not 0).ToArray();
            }
                
        }
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
            await downloader.Download(indexResponse, args, "fr-FR", ffmpegPriority);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {e.Message}");
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