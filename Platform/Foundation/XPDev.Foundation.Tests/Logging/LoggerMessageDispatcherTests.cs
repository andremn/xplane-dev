using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using XPDev.Foundation.Logging;

namespace XPDev.Foundation.Tests.Logging
{
    [TestClass]
    public class LoggerMessageDispatcherTests
    {
        private Mock<ILoggerFileWriter> LoggerFileWriterMock { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            LoggerFileWriterMock = new Mock<ILoggerFileWriter>();
            LoggerFileWriterMock.Setup(m => m.FilePath).Returns("log.txt");
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsFatalAndLogMessageLevelIsFatal_WriteLogMessageIsCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Fatal;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Fatal);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsFatalAndLogMessageLevelIsWarning_WriteLogMessageIsNotCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Warning;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Fatal);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage), Times.Never());
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsFatalAndLogMessageLevelIsInfo_WriteLogMessageIsNotCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Info;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Fatal);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage), Times.Never());
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsFatalAndLogMessageLevelIsDebug_WriteLogMessageIsNotCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Debug;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Fatal);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage), Times.Never());
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsWarningAndLogMessageLevelIsFatal_WriteLogMessageIsCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Fatal;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Warning);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsWarningAndLogMessageLevelIsWarning_WriteLogMessageICalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Warning;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Warning);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsWarningAndLogMessageLevelIsInfo_WriteLogMessageIsNotCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Info;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Warning);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage), Times.Never());
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsWarningAndLogMessageLevelIsDebug_WriteLogMessageIsNotCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Debug;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Warning);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage), Times.Never());
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsInfoAndLogMessageLevelIsFatal_WriteLogMessageIsCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Fatal;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Info);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsInfoAndLogMessageLevelIsWarning_WriteLogMessageICalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Warning;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Info);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsInfoAndLogMessageLevelIsInfo_WriteLogMessageIsCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Info;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Info);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsInfoAndLogMessageLevelIsDebug_WriteLogMessageIsNotCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Debug;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Info);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage), Times.Never());
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsDebugAndLogMessageLevelIsFatal_WriteLogMessageIsCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Fatal;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Debug);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsDebugAndLogMessageLevelIsWarning_WriteLogMessageICalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Warning;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Debug);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsDebugAndLogMessageLevelIsInfo_WriteLogMessageIsCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Info;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Debug);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }

        [TestMethod]
        public async Task AddLogMessage_LoggerLevelIsDebugAndLogMessageLevelIsDebug_WriteLogMessageIsCalled()
        {
            // Arrage
            const string expectedLogMessage = "Test message";
            const LogLevel expectedLogLevel = LogLevel.Debug;
            var dispatcher = LoggerMessageDispatcher.GetInstance(LoggerFileWriterMock.Object, LogLevel.Debug);

            // Act
            dispatcher.AddLogMessage(new LogMessage(expectedLogMessage, expectedLogLevel));
            await Task.Delay(50);

            // Assert
            LoggerFileWriterMock.Verify(m => m.WriteLogMessage(expectedLogMessage));
        }
    }
}
