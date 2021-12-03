using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CrunchyBetaDownloader.FFtools.Exceptions;

namespace CrunchyBetaDownloader.FFtools
{
    public abstract class CRFFmpegProcess
    {
        public static string ExecutablesPath { get; private set; } = string.Empty;
        
        private static string? _ffmpegPath;
        private static string? _ffprobePath;
        private static readonly object FfmpegPathLock = new();
        private static readonly object FfprobePathLock = new();
        private const string FfmpegExecutableName = "ffmpeg";
        private const string FfprobeExecutableName = "ffprobe";
        protected string? FFmpegPath
        {
            get
            {
                lock (FfmpegPathLock)
                    return _ffmpegPath;
            }
            private set
            {
                lock (FfmpegPathLock)
                    _ffmpegPath = value;
            }
        }
        protected string? FFprobePath
        {
            get
            {
                lock (FfprobePathLock)
                    return _ffprobePath;
            }
            private set
            {
                lock (FfprobePathLock)
                    _ffprobePath = value;
            }
        }
        
        /// <summary>Run conversion</summary>
        /// <param name="args">Arguments</param>
        /// <param name="processPath">FilePath to executable (FFmpeg, ffprobe)</param>
        /// <param name="priority">Process priority to run executables</param>
        /// <param name="standardInput">Should redirect standard input</param>
        /// <param name="standardOutput">Should redirect standard output</param>
        /// <param name="standardError">Should redirect standard error</param>
        /// <returns>FFmpeg or  ffprobe Process</returns>
        protected Process RunProcess(
            string args,
            string? processPath,
            ProcessPriorityClass? priority,
            bool standardInput = false,
            bool standardOutput = false,
            bool standardError = false)
        {
            Process process = new()
            {
                StartInfo = {
                    FileName = processPath ?? throw new ArgumentNullException(nameof(processPath)),
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = standardInput,
                    RedirectStandardOutput = standardOutput,
                    RedirectStandardError = standardError
                },
                EnableRaisingEvents = true
            };
            process.Start();
            try
            {
                process.PriorityClass = !priority.HasValue ? Process.GetCurrentProcess().PriorityClass : priority.Value;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Process Error:");
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            return process;
        }
        
        private void FindProgramsFromPath(string path)
        {
            if (!Directory.Exists(path) || string.IsNullOrEmpty(path))
                return;
            IEnumerable<FileInfo> files = new DirectoryInfo(path).GetFiles();
            FFprobePath = GetFullName(files, FfprobeExecutableName);
            FFmpegPath = GetFullName(files, FfmpegExecutableName);
        }

        protected CRFFmpegProcess() => FindAndValidateExecutables();

        private void FindAndValidateExecutables()
        {
                Assembly? entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly != null)
                {
                    FindProgramsFromPath(Path.GetDirectoryName(entryAssembly.Location) ?? string.Empty);
                    if (FFmpegPath != null && FFprobePath != null)
                        return;
                }
                string environmentVariable = Environment.GetEnvironmentVariable("PATH") ?? throw new NullReferenceException();
                char[] chArray = { Path.PathSeparator };
                foreach (string? path in environmentVariable.Split(chArray))
                {
                    FindProgramsFromPath(path);
                    if (FFmpegPath != null && FFprobePath != null)
                        break;
                }
                ValidateExecutables();
            
        }
        
        private void ValidateExecutables()
        {
            if (FFmpegPath is null || FFprobePath is null)
                throw new ExceptionFFmpegNotFound($"Cannot find FFmpeg in {(string.IsNullOrWhiteSpace(ExecutablesPath) ? string.Empty : string.Format($"{ExecutablesPath} or"))} PATH. This package needs installed FFmpeg. Please add it to your PATH variable or specify path to DIRECTORY with FFmpeg executables in FFmpeg.ExecutablesPath");
        }
        private static string? GetFullName(IEnumerable<FileInfo> files, string fileName) => files.FirstOrDefault(x => x.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase) || x.Name.Equals($"{fileName}.exe", StringComparison.InvariantCultureIgnoreCase))?.FullName;

    }
}