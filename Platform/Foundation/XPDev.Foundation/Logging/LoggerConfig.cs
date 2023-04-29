namespace XPDev.Foundation.Logging
{
    /// <summary>
    /// Represents the configuration used by all loggers.
    /// </summary>
    public class LoggerConfig : ILoggerConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerConfig"/> class.
        /// </summary>
        /// <param name="filePath">The path of the log file.</param>
        /// <param name="level">The level of the logger.</param>
        public LoggerConfig(string filePath, LogLevel level)
        {
            FilePath = filePath;
            Level = level;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerConfig"/> class.
        /// </summary>
        /// <param name="filePath">The path of the log file.</param>
        /// <param name="level">The level of the logger.</param>
        /// <param name="clearFile">Indicates whether the log file should be cleared when a new session starts.</param>
        public LoggerConfig(string filePath, LogLevel level, bool clearFile)
        {
            FilePath = filePath;
            Level = level;
            ClearFile = clearFile;
        }

        /// <summary>
        /// Gets the path of the log file.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets the level of the logger.
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// Gets whether the log file should be cleared when a new session starts.
        /// </summary>
        public bool ClearFile { get; }
    }
}
