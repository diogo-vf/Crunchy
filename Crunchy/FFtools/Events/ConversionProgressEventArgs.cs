namespace Crunchy.FFtools.Events;

/// <summary>Info about conversion progress</summary>
/// <param name="sender">Sender</param>
/// <param name="args">Conversion info</param>
public delegate void ConversionProgressEventHandler(
    object sender,
    ConversionProgressEventArgs args);

public class ConversionProgressEventArgs : EventArgs
{
    public ConversionProgressEventArgs(TimeSpan timeSpan, TimeSpan totalTime, int processId)
    {
        Duration = timeSpan;
        TotalLength = totalTime;
        ProcessId = processId;
    }

    /// <summary>Current processing time</summary>
    public TimeSpan Duration { get; }

    /// <summary>Video length</summary>
    public TimeSpan TotalLength { get; }

    /// <summary>Process id</summary>
    public long ProcessId { get; }

    /// <summary>Percent of conversion</summary>
    public int Percent => (int)(Math.Round(Duration.TotalSeconds / TotalLength.TotalSeconds, 2) * 100.0);
}