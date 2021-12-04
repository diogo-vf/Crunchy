using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string jsonPath =
                Path.Join(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? ".",
                    "config.json");
            CreateJsonConfigFile(jsonPath);
            
            IConfigData config = new JsonConfig(jsonPath);
            Downloader downloader = new(config);
            IndexResponse? indexResponse = await downloader.Login();
            await downloader.Download(indexResponse, args);
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