using System;

namespace CrunchyBetaDownloader.FFtools
{
    public class FFmpegResult
    {
        /// <summary>Date and time of starting ffmpeg conversion</summary>
        public DateTime? StartTime { get; init;}

        /// <summary>Date and time of ending ffmpeg conversion</summary>
        public DateTime? EndTime { get; init;}

        /// <summary>ffmpeg duration time for converting </summary>
        public TimeSpan? Duration => EndTime - StartTime;

        /// <summary>Arguments passed to ffmpeg</summary>
        public string? Arguments { get; init;}

        public override string ToString()
        {
            return $"start:\t{StartTime}\nend:\t{EndTime}\ntotal:\t{Duration?.Minutes} minutes";
        }
    }
}