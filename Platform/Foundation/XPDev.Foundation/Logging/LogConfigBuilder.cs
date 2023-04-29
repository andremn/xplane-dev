namespace XPDev.Foundation.Logging
{
    /// <summary>
    /// Used to create instances of <see cref="ILoggerConfig"/>.
    /// </summary>
    public class LogConfigBuilder
    {
        private readonly string _filePath;
        private LogLevel _logLevel;
        private bool _clearFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogConfigBuilder"/> class with the specified path for the log file.
        /// </summary>
        /// <param name="filePath">The path for the log file.</param>
        public LogConfigBuilder(string filePath)
        {
            _filePath = filePath;
            _logLevel = LogLevel.Info;
            _clearFile = false;
        }

        /// <summary>
        /// Builds an instance of <see cref="ILoggerConfig"/> with the specified parts.
        /// </summary>
        /// <returns>An instance of <see cref="ILoggerConfig"/>.</returns>
        public ILoggerConfig Build()
        {
            return new LoggerConfig(_filePath, _logLevel, _clearFile);
        }

        /// <summary>
        /// Sets the log level used when building the log config.
        /// </summary>
        /// <param name="logLevel">The desired log level.</param>
        /// <returns>The current instance of <see cref="LogConfigBuilder"/>.</returns>
        public LogConfigBuilder WithLogLevel(LogLevel logLevel)
        {
            _logLevel = logLevel;
            return this;
        }

        /// <summary>
        /// Causes the log file content to be cleared at every new session instead of appending the new content to the previous one.
        /// </summary>
        /// <returns>The current instance of <see cref="LogConfigBuilder"/>.</returns>
        public LogConfigBuilder WithCleanFile()
        {
            _clearFile = true;
            return this;
        }
    }
}
