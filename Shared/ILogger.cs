namespace Shared
{
    public enum LogLevel 
    { 
        Info,
        Warn, 
        Error,
        Fatal
    }

    public interface ILogger
    {
        void Log(Exception exception = null, string message = "", LogLevel level = LogLevel.Info);
    }
}
