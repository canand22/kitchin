using NLog;

namespace KitchIn.Core
{
    /// <summary>
    /// Describes log data writer
    /// </summary>
    public static class LogWriter
    {
        private static Logger Logger { get; set; }

        /// <summary>
        /// Initializes the <see cref="LogWriter"/> class.
        /// </summary>
        static LogWriter()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="message">The message.</param>
        public static void WriteLog(LogLevel level, string message)
        {
            Logger.Log(level, message);
        }

        /// <summary>
        /// Logs an errors with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void WriteError(string message)
        {
            Logger.Error(message);
        }

        /// <summary>
        /// Logs a warning with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void WriteWarning(string message)
        {
            Logger.Warn(message);
        }

        /// <summary>
        /// Logs an info with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void WriteInfo(string message)
        {
            Logger.Info(message);
        }
    }
}
