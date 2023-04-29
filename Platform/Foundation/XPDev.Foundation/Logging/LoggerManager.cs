using System;
using System.IO;

namespace XPDev.Foundation.Logging
{
    /// <summary>
    /// Provides methods for creating instances of <see cref="ILogger"/>.
    /// </summary>
    public static class LoggerManager
    {
        private static readonly Lazy<NullLogger> _nullLogger = new Lazy<NullLogger>();

        private static bool _isInitialized;
        private static ILoggerConfig _config;

        /// <summary>
        /// Gets an instance of <see cref="ILogger"/> for the specified type.
        /// </summary>
        /// <param name="type">The type to create the <see cref="ILogger"/> for.</param>
        /// <returns>An instance of <see cref="ILogger"/> for <paramref name="type"/>.</returns>
        public static ILogger GetLoggerFor(Type type)
        {
            if (!_isInitialized)
            {
                return _nullLogger.Value;
            }

            var typeName = type?.FullName ?? throw new ArgumentNullException(nameof(type));
            var messageDispatcher = LoggerMessageDispatcher.GetInstance(_config.FilePath, _config.Level);

            return new Logger(messageDispatcher) { Name = typeName, Level = _config.Level };
        }

        /// <summary>
        /// Gets an instance of <see cref="ILogger"/> for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create the <see cref="ILogger"/> for.</typeparam>
        /// <returns>An instance of <see cref="ILogger"/> for <typeparamref name="T"/>.</returns>
        public static ILogger GetLoggerFor<T>()
        {
            if (!_isInitialized)
            {
                return _nullLogger.Value;
            }

            var typeName = typeof(T).FullName;
            var messageDispatcher = LoggerMessageDispatcher.GetInstance(_config.FilePath, _config.Level);

            return new Logger(messageDispatcher) { Name = typeName, Level = _config.Level };
        }

        /// <summary>
        /// Initializes the logging with the specified <see cref="ILoggerConfig"/>.
        /// </summary>
        /// <param name="loggerConfig">The <see cref="ILoggerConfig"/> to be used to configure the logger.</param>
        public static void Initialize(ILoggerConfig loggerConfig)
        {
            if (_isInitialized)
            {
                return;
            }

            if (loggerConfig == null)
            {
                throw new ArgumentNullException(nameof(loggerConfig));
            }

            if (string.IsNullOrEmpty(loggerConfig.FilePath))
            {
                throw new ArgumentException("Logger config cannot have an empty file path", nameof(loggerConfig));
            }

            _config = loggerConfig;

            if (_config.ClearFile)
            {
                if (!Directory.Exists(_config.FilePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_config.FilePath));
                }

                File.WriteAllText(_config.FilePath, string.Empty);
            }

            _isInitialized = true;
        }

        /// <summary>
        /// Shuts the logging down. Log messages will not be processed after calling this method.
        /// Call <see cref="Initialize(ILoggerConfig)"/> to start logging again.
        /// </summary>
        public static void Shutdown()
        {
            if (!_isInitialized)
            {
                return;
            }

            LoggerMessageDispatcher.DisposeInstances();
            _isInitialized = false;
        }

        private class NullLogger : ILogger
        {
            public string Name => nameof(NullLogger);

            public void Debug(string message)
            {
            }

            public void Debug(Exception exception, string message)
            {
            }

            public void Debug(Exception exception)
            {
            }

            public void Fatal(string message)
            {
            }

            public void Fatal(Exception exception, string message)
            {
            }

            public void Fatal(Exception exception)
            {
            }

            public void Info(string message)
            {
            }

            public void Info(Exception exception, string message)
            {
            }

            public void Info(Exception exception)
            {
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Warning(string message)
            {
            }

            public void Warning(Exception exception, string message)
            {
            }

            public void Warning(Exception exception)
            {
            }
        }
    }
}
