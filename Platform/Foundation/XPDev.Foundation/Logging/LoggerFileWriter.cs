using System;
using System.IO;

namespace XPDev.Foundation.Logging
{
    internal class LoggerFileWriter : ILoggerFileWriter, IEquatable<LoggerFileWriter>
    {
        public LoggerFileWriter(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }

        public void WriteLogMessage(string message)
        {
            using var fileStream = File.AppendText(FilePath);

            fileStream.WriteLine(message);
        }

        public override bool Equals(object obj)
        {
            return obj is LoggerFileWriter item && Equals(item);
        }

        public bool Equals(LoggerFileWriter other)
        {
            return FilePath == other.FilePath;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FilePath);
        }
    }
}
