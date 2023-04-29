using System;

namespace XPDev.Foundation.Logging
{
    /// <summary>
    /// Provides methods for logging messages to a file.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the logger name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether the logger is enabled for the specified log level.
        /// </summary>
        /// <param name="logLevel">The level to check if the logger is enabled.</param>
        /// <returns>True if the logger is enabled for the specified log level; false otherwise.</returns>
        bool IsEnabled(LogLevel logLevel);

        /// <summary>
        /// Writes a fatal message to the log if the specified log level is <see cref="LogLevel.Fatal"/>.
        /// </summary>
        /// <param name="message">The message to write to the log file.</param>
        void Fatal(string message);

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Fatal"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        void Fatal(Exception exception, string message);

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Fatal"/>.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        void Fatal(Exception exception);

        /// <summary>
        /// Writes a warning message to the log if the specified log level is <see cref="LogLevel.Warning"/> or lower.
        /// </summary>
        /// <param name="message">The message to write to the log file.</param>
        void Warning(string message);

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Warning"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        void Warning(Exception exception, string message);

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Info"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        void Warning(Exception exception);

        /// <summary>
        /// Writes a info message to the log if the specified log level is <see cref="LogLevel.Info"/> or lower.
        /// </summary>
        /// <param name="message">The message to write to the log file.</param>
        void Info(string message);

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Info"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        void Info(Exception exception, string message);

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Info"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        void Info(Exception exception);

        /// <summary>
        /// Writes a debug message to the log if the specified log level is <see cref="LogLevel.Debug"/> or lower.
        /// </summary>
        /// <param name="message">The message to write to the log file.</param>
        void Debug(string message);

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Debug"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        /// <param name="message">The message to write along with the exception to the log file.</param>
        void Debug(Exception exception, string message);

        /// <summary>
        /// Writes the stack trace of the specified exception to the log if the specified log level is <see cref="LogLevel.Debug"/> or lower.
        /// </summary>
        /// <param name="exception">The exception to write to the log file.</param>
        void Debug(Exception exception);
    }
}