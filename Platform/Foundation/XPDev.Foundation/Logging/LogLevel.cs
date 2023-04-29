namespace XPDev.Foundation.Logging
{
    /// <summary>
    /// The possible levels of logging.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Enables the logger to write only fatal error messages.
        /// </summary>
        Fatal = 0,
        /// <summary>
        /// Enables the logger to write warning and fatal error messages.
        /// </summary>
        Warning,
        /// <summary>
        /// Enables the logger to write fatal, warning and info messages and errors.
        /// </summary>
        Info,
        /// <summary>
        /// Enables the logger to write all messages.
        /// </summary>
        Debug
    }
}
