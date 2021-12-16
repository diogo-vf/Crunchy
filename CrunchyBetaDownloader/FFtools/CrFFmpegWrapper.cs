using System.Diagnostics;
using System.Text.RegularExpressions;
using CrunchyBetaDownloader.FFtools.Events;
using CrunchyBetaDownloader.FFtools.Exceptions;
using Microsoft.Win32.SafeHandles;

namespace CrunchyBetaDownloader.FFtools;

internal class CrFFmpegWrapper : CrFFmpegProcess
{
    private static readonly Regex STimeFormatRegex = new("\\w\\w:\\w\\w:\\w\\w", RegexOptions.Compiled);
    private List<string>? _outputLog;
    private TimeSpan _totalTime;
    private bool _wasKilled;
    public string Log => _outputLog?.ToString() ?? string.Empty;

    /// <summary>Fires when FFmpeg progress changes</summary>
    internal event ConversionProgressEventHandler? OnProgress;

    /// <summary>Fires when FFmpeg process print something</summary>
    internal event DataReceivedEventHandler? OnDataReceived;

    internal Task<bool> RunProcess(
        string args,
        CancellationToken cancellationToken,
        ProcessPriorityClass? priority)
    {
        return Task.Factory.StartNew(() =>
        {
#if DEBUG
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{FFmpegPath} {args}");
                Console.ForegroundColor = oldColor;
#endif
            _outputLog = new List<string>();
            Process process = RunProcess(args, FFmpegPath, priority, true, standardError: true);
            int processId = process.Id;
            using (process)
            {
                process.ErrorDataReceived +=
                    (DataReceivedEventHandler) ((_, e) => ProcessOutputData(e, args, processId));
                process.BeginErrorReadLine();
                using (cancellationToken.Register(() =>
                       {
                           if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                               return;
                           try
                           {
                               process.StandardInput.Write("q");
                               Task.Delay(5000, cancellationToken).GetAwaiter().GetResult();
                               if (process.HasExited)
                                   return;
                               process.CloseMainWindow();
                               process.Kill();
                               _wasKilled = true;
                           }
                           catch (InvalidOperationException)
                           {
                           }
                       }))
                {
                    using (ManualResetEvent waitHandle = new(false))
                    {
                        waitHandle.SetSafeWaitHandle(new SafeWaitHandle(process.Handle, false));
                        int num = WaitHandle.WaitAny(new[]
                        {
                            waitHandle,
                            cancellationToken.WaitHandle
                        });
                        switch (num)
                        {
                            case 1 when !process.HasExited:
                                process.CloseMainWindow();
                                process.Kill();
                                _wasKilled = true;
                                break;
                            case 0:
                                if (!process.HasExited) process.WaitForExit();
                                break;
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    if (_wasKilled)
                        throw new ExceptionConversion("Cannot stop process. Killed it.", args);
                    if (cancellationToken.IsCancellationRequested)
                        return false;
                    string str = string.Join(Environment.NewLine, _outputLog.ToArray());
                    if (process.ExitCode == 0) return true;
                    if (!_outputLog.Any()) return true;
                    if (!_outputLog.Last().Contains("dummy"))
                        throw new ExceptionConversion(str, args);
                }
            }

            return true;
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    private void ProcessOutputData(DataReceivedEventArgs e, string args, int processId)
    {
        if (e.Data == null)
            return;
        DataReceivedEventHandler? onDataReceived = OnDataReceived;
        onDataReceived?.Invoke(this, e);
        _outputLog?.Add(e.Data);
        if (OnProgress == null)
            return;
        CalculateTime(e, args, processId);
    }

    private void CalculateTime(DataReceivedEventArgs e, string args, int processId)
    {
        if (e.Data == null || e.Data.Contains("Duration: N/A"))
            return;
        if (e.Data.Contains("Duration"))
        {
            GetDuration(e, STimeFormatRegex, args);
        }
        else
        {
            if (!e.Data.Contains("size"))
                return;
            Match match = STimeFormatRegex.Match(e.Data);
            if (!match.Success)
                return;
            OnProgress?.Invoke(this,
                new ConversionProgressEventArgs(TimeSpan.Parse(match.Value), _totalTime, processId));
        }
    }

    private void GetDuration(DataReceivedEventArgs e, Regex regex, string args)
    {
        string argumentValue1 = GetArgumentValue("-t", args);
        if (!string.IsNullOrWhiteSpace(argumentValue1))
        {
            _totalTime = TimeSpan.Parse(argumentValue1);
        }
        else
        {
            Match match = regex.Match(e.Data ?? throw new ArgumentNullException(nameof(e)));
            if (!match.Success)
                return;
            _totalTime = TimeSpan.Parse(match.Value);
            string argumentValue2 = GetArgumentValue("-ss", args);
            if (string.IsNullOrWhiteSpace(argumentValue2))
                return;
            _totalTime -= TimeSpan.Parse(argumentValue2);
        }
    }

    private static string GetArgumentValue(string option, string args)
    {
        List<string> list = (args.Split(' ')).ToList();
        int num = list.IndexOf(option);
        return num >= 0 ? list[num + 1] : string.Empty;
    }
}