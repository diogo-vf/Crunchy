using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CrunchyBetaDownloader.FFtools.Events;
using CrunchyBetaDownloader.FFtools.utils;

namespace CrunchyBetaDownloader.FFtools
{
    public class FFmpeg
    {
        private CrFFmpegWrapper? _ffmpeg;

        public event ConversionProgressEventHandler? OnProgress;

        public event DataReceivedEventHandler? OnDataReceived;

        private ProcessPriorityClass? _priority;

        private bool _multiThread;

        public FFmpeg SetMultiThread(bool value)
        {
            _multiThread = value;
            return this;
        }

        public void SetPriority(ProcessPriorityClass? priority)
        {
            _priority = priority;
        }

        /// <summary>
        /// Execute ffmpeg with all parameters and reset parameters at the end
        /// </summary>
        /// <param name="parameters">ffmpeg parameters</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">when ffmpeg already executed</exception>
        public async Task<FFmpegResult> Start(string parameters, CancellationToken cancellationToken)
        {
            if (_ffmpeg != null)
                throw new InvalidOperationException("ffmpeg has already been started. ");

            CustomStopwatch stopwatch = new();
            stopwatch.Start();
            _ffmpeg = new CrFFmpegWrapper();
            try
            {
                _ffmpeg.OnProgress += OnProgress;
                _ffmpeg.OnDataReceived += OnDataReceived;
                string? multiThreadParam =
                    _multiThread ? $"-threads {Math.Min(Environment.ProcessorCount, 16)} " : null;
                string args = $"{multiThreadParam}{parameters}";
                await _ffmpeg.RunProcess(args, cancellationToken, _priority);
            }
            finally
            {
                _ffmpeg.OnProgress -= OnProgress;
                _ffmpeg.OnDataReceived -= OnDataReceived;
                _ffmpeg.OnProgress += null;
                _ffmpeg.OnDataReceived -= null;
                _multiThread = false;
                _ffmpeg = null;
            }
            stopwatch.Stop();
            return new FFmpegResult
            {
                StartTime = stopwatch.StartAt,
                EndTime = stopwatch.EndAt,
                Arguments = parameters
            };
        }
    }
}