using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using System.Globalization;
using Moq;

namespace JobDurationLogger.Tests
{
    public class LogProcessorTests
    {
    private readonly LogProcessor _logProcessor;

    public LogProcessorTests () {
        _logProcessor = new LogProcessor();
    }
        
        // TEST 1 
        [Fact]
        public void ProcessLogFile_ShouldLogWarning_ForJobExceeding_5_Minutes()
        {
            // Arrange
            var expectedMessage = "WARNING: Job 1001 took more than 5 minutes";

            var logContent = @"
                12:00:00,Job A,START,1001
                12:06:00,Job A,END,1001";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            var model = _logProcessor.ProcessLogFile(filePath);
            var acutalMessage = model[0].DurationMessage;

            // Assert
            Assert.Single(model); // Single Record 
            Assert.Contains(acutalMessage, expectedMessage); //check message
        }

        // TEST 2
        [Fact]
        public void ProcessLogFile_ShouldLogError_ForJobExceeding_10_Minutes()
        {
            // Arrange
            var expectedMessage = "ERROR: Job 1002 took more than 10 minutes";

            var logContent = @"
                12:00:00,Job A,START,1002
                12:11:00,Job A,END,1002";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            var model = _logProcessor.ProcessLogFile(filePath);
            var acutalMessage = model[0].DurationMessage;

            // Assert
            Assert.Single(model); // Single Record 
            Assert.Contains(acutalMessage, expectedMessage); //check message
        }

        // TEST 3
        [Fact]
        public void ProcessLogFile_ShouldNotLog_For_Good_Job()
        {
            // Arrange
            var logContent = @"
                12:00:00,Job C,START,1003
                12:03:00,Job C,END,1003";

            var filePath = WriteLogToTempFile(logContent);
            // Act
            var model = _logProcessor.ProcessLogFile(filePath);

            // Assert
            Assert.Empty(model); // no data should be returned 
        }
        // TEST 4 - 
        [Fact]
        public void ProcessLogFile_ShouldLogWarning_For_No_End()
        {
            // Arrange
            var expectedMessage = "WARNING: Job 1004 started but did not finish";

            var logContent = @"
                12:00:00,Job C,START,1004";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            var model = _logProcessor.ProcessLogFile(filePath);
            var acutalMessage = model[0].DurationMessage;

            // Assert
            Assert.Single(model); // Single Record 
            Assert.Contains(acutalMessage, expectedMessage); // Check Message

        }

        private static string WriteLogToTempFile(string content)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, content.Trim());
            return tempFile;
        }
    }

}
