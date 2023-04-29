namespace XPDev.Foundation.Logging
{
    internal interface ILoggerFileWriter
    {
        string FilePath { get; }

        void WriteLogMessage(string message);
    }
}
