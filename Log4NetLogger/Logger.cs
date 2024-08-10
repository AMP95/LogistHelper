using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Shared.Logger;

namespace Log4NetLogger
{
    public class Logger : ILogger
    {
        private readonly ILog log;

        public Logger()
        {
            log = LogManager.GetLogger("LogAppender");
            XmlConfigurator.ConfigureAndWatch(new FileInfo("Log4NetLogger.dll.config"));
        }
        public void Log(Exception exception, string message, LogLevel level = LogLevel.Info)
        {
            switch (level)
            {
                case LogLevel.Info:
                    ThreadPool.QueueUserWorkItem((temp) =>
                    {
                        if (exception != null)
                        {
                            log.Info(message, exception);
                        }
                        else
                        {
                            log.Info(message);
                        }
                    });
                    break;
                case LogLevel.Error:
                    ThreadPool.QueueUserWorkItem(temp => log.Error(message, exception));
                    break;
                case LogLevel.Fatal:
                    ThreadPool.QueueUserWorkItem(temp => log.Fatal(message));
                    break;
            }
        }
    }
}
