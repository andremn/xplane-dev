namespace XPDev.Foundation.Logging
{
    internal class LogMessage
    {
        public LogMessage(string message, LogLevel logLevel)
        {
            Message = message;
            LogLevel = logLevel;
        }

        public string Message { get; }

        public LogLevel LogLevel { get; }
    }
}
