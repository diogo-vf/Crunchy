namespace Crunchy.FFtools.Exceptions;

public class ExceptionFFmpegNotFound : FileNotFoundException
{
    /// <summary>
    ///     The exception that is thrown when a FFmpeg executables cannot be found.
    /// </summary>
    /// <param name="errorMessage">FFmpeg error output</param>
    internal ExceptionFFmpegNotFound(string errorMessage) : base(errorMessage)
    {
    }
}