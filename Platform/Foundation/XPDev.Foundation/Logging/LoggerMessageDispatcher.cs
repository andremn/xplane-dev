using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace XPDev.Foundation.Logging
{
    internal sealed class LoggerMessageDispatcher : Disposable, ILoggerMessageDispatcher
    {
        private static readonly ConcurrentDictionary<ILoggerFileWriter, LoggerMessageDispatcher> _instances =
            new ConcurrentDictionary<ILoggerFileWriter, LoggerMessageDispatcher>();

        private readonly ILoggerFileWriter _loggerFileWriter;
        private readonly LogLevel _logLevel;
        private readonly BlockingCollection<LogMessage> _logMessages;

        private LoggerMessageDispatcher(ILoggerFileWriter loggerFileWriter, LogLevel logLevel)
        {
            _loggerFileWriter = loggerFileWriter;
            _logLevel = logLevel;
            _logMessages = new BlockingCollection<LogMessage>();

            Task.Run(FlushLog);
        }

        /// <summary>
        /// Gets an instance of <see cref="LoggerMessageDispatcher"/> with the specified <see cref="ILoggerFileWriter"/> and log level.
        /// </summary>
        /// <param name="loggerFileWriter">The <see cref="ILoggerFileWriter"/> to be used.</param>
        /// <param name="logLevel">The log level to be used.</param>
        /// <returns>An instance of <see cref="LoggerMessageDispatcher"/>.</returns>
        public static LoggerMessageDispatcher GetInstance(ILoggerFileWriter loggerFileWriter, LogLevel logLevel)
        {
            if (!_instances.TryGetValue(loggerFileWriter, out var instance))
            {
                instance = new LoggerMessageDispatcher(loggerFileWriter, logLevel);
                _instances.AddOrUpdate(loggerFileWriter, instance, (k, v) => instance);
            }

            return instance;
        }

        /// <summary>
        /// Gets an instance of <see cref="LoggerMessageDispatcher"/> for the specified file path and log level.
        /// </summary>
        /// <param name="filePath">The path of the log file.</param>
        /// <param name="logLevel">The level of the log.</param>
        /// <returns>An instance of <see cref="LoggerMessageDispatcher"/>.</returns>
        public static LoggerMessageDispatcher GetInstance(string filePath, LogLevel logLevel)
        {
            return GetInstance(new LoggerFileWriter(filePath), logLevel);
        }

        /// <summary>
        /// Disposes all instances of <see cref="LoggerMessageDispatcher"/> created by <see cref="GetInstance(string, LogLevel)"/> 
        /// before calling the dispose method.
        /// </summary>
        public static void DisposeInstances()
        {
            var diposableInstances = _instances.ToArray().Select(x => x.Value as IDisposable);

            foreach (var instance in diposableInstances)
            {
                try
                {
                    instance?.Dispose();
                }
                catch (Exception)
                {
                    // Log exception
                }
            }
        }

        /// <summary>
        /// Adds a <see cref="LogMessage"/> to be processed by this <see cref="LoggerMessageDispatcher"/>.
        /// </summary>
        /// <param name="logMessage">The <see cref="LogMessage"/> to be processed.</param>
        public void AddLogMessage(LogMessage logMessage)
        {
            _logMessages.Add(logMessage ?? throw new ArgumentNullException(nameof(logMessage)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logMessages?.CompleteAdding();
                _logMessages?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void FlushLog()
        {
            foreach (var logMessage in _logMessages.GetConsumingEnumerable())
            {
                if (_logLevel >= logMessage.LogLevel)
                {
                    try
                    {
                        _loggerFileWriter.WriteLogMessage(logMessage.Message);
                    }
                    catch (Exception)
                    {
                        // Todo: log exception
                    }
                }
            }
        }
    }
}
