using System;
using System.Runtime.CompilerServices;

namespace XPDev.Foundation.Logging
{
    /// <summary>
    /// Provides methods for logging messages to a file.
    /// </summary>
    public partial class Logger : ILogger
    {
        private readonly ILoggerMessageDispatcher _loggerMessageDispatcher;

        /// <summary>
        /// Creates an instance of the <see cref="Logger"/> class.
        /// </summary>
        internal Logger(ILoggerMessageDispatcher loggerMessageDispatcher)
        {
            _loggerMessageDispatcher = loggerMessageDispatcher;
        }

        /// <summary>
        /// Gets the logger name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the current log level.
        /// </summary>
        public LogLevel Level { get; internal set; }

        /// <summary>
        /// Gets whether the logger is enabled for the specified log level.
        /// </summary>
        /// <param name="logLevel">The level to check if the logger is enabled.</param>
        /// <returns>True if the logger is enabled for the specified log level; false otherwise.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return Level >= logLevel;
        }

        /// <summary>
        /// Writes a fatal message to the log if the specified log level is <see cref="LogLevel.Fatal"/>.
        /// </summary>
        /// <param name="message">The message to write to the log file.</param>
        public void Fatal(string message)
        {
            Log(message, LogLevel.Fatal);
        }

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Fatal"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        public void Fatal(Exception exception, string message)
        {
            Log(exception, message, LogLevel.Fatal);
        }

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Fatal"/>.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        public void Fatal(Exception exception)
        {
            Log(exception, LogLevel.Fatal);
        }

        /// <summary>
        /// Writes a warning message to the log if the specified log level is <see cref="LogLevel.Warning"/> or lower.
        /// </summary>
        /// <param name="message">The message to write to the log file.</param>
        public void Warning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Warning"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        public void Warning(Exception exception, string message)
        {
            Log(exception, message, LogLevel.Warning);
        }

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Info"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        public void Warning(Exception exception)
        {
            Log(exception, LogLevel.Warning);
        }

        /// <summary>
        /// Writes a info message to the log if the specified log level is <see cref="LogLevel.Info"/> or lower.
        /// </summary>
        /// <param name="message">The message to write to the log file.</param>
        public void Info(string message)
        {
            Log(message, LogLevel.Info);
        }

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Info"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        public void Info(Exception exception, string message)
        {
            Log(exception, message, LogLevel.Info);
        }

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Info"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        public void Info(Exception exception)
        {
            Log(exception, LogLevel.Info);
        }

        /// <summary>
        /// Writes a debug message to the log if the specified log level is <see cref="LogLevel.Debug"/> or lower.
        /// </summary>
        /// <param name="message">The message to write to the log file.</param>
        public void Debug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Debug"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        public void Debug(Exception exception, string message)
        {
            Log(exception, message, LogLevel.Debug);
        }

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Debug"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        public void Debug(Exception exception)
        {
            Log(exception, LogLevel.Debug);
        }

        /// <summary>
        /// Writes the specified exception stack trace to the log using the specified level.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="level">The level to be used.</param>
        protected void Log(Exception exception, LogLevel level)
        {
            Log(exception, "An exception has been thrown:", level);
        }

        /// <summary>
        /// Writes the specified exception stack trace to the log using the specified level.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        /// <param name="level">The level to be used.</param>
        protected void Log(Exception exception, string message, LogLevel level)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Log($"{message}: {exception.Message}{Environment.NewLine}{exception.StackTrace}", level);

            var innerException = exception.InnerException;

            while (innerException != null)
            {
                Log($"Inner exception: {exception.Message}{Environment.NewLine}{exception.StackTrace}", level);
                innerException = innerException.InnerException;
            }
        }

        /// <summary>
        /// Writes the specified message to the log using the specified level.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        /// <param name="level">The level to be used.</param>
        protected void Log(string message, LogLevel level)
        {
            message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}, {Name}, {level}] {message}";

            _loggerMessageDispatcher.AddLogMessage(new LogMessage(message, level));
        }
    }
}
