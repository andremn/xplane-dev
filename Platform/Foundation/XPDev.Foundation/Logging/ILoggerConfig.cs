namespace XPDev.Foundation.Logging
{
    /// <summary>
    /// Represents the configuration used by all loggers.
    /// </summary>
    public interface ILoggerConfig
    {
        /// <summary>
        /// Gets the path of the log file.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Gets the level of the logger.
        /// </summary>
        LogLevel Level { get; }

        /// <summary>
        /// Gets whether the log file should be cleared when a new session starts.
        /// </summary>
        bool ClearFile { get; }
    }
}
