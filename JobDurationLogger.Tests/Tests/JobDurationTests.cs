using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using System.Globalization;

namespace JobDurationLogger.Tests
{
    public class LogProcessorTests
    {
        
        // TEST 1 
        [Fact]
        public void ProcessLogFile_ShouldLogWarning_ForJobExceeding_5_Minutes()
        {
            // Arrange
            var mockLogger = new MockJobDurationLogger();
            var processor = new LogProcessor(mockLogger);
            var expectedMessage = "WARNING: Job 1001 took more than 5 minutes";

            var logContent = @"
                12:00:00,Job A,START,1001
                12:06:00,Job A,END,1001";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            var model = processor.ProcessLogFile(filePath);
            var acutalMessage = model[0].DurationMessage;

            // Assert
            Assert.Contains(acutalMessage, expectedMessage);
        }

        // TEST 2
        [Fact]
        public void ProcessLogFile_ShouldLogError_ForJobExceeding_10_Minutes()
        {
            // Arrange
            var mockLogger = new MockJobDurationLogger();
            var processor = new LogProcessor(mockLogger);
            var expectedMessage = "ERROR: Job 1001 took more than 10 minutes";

            var logContent = @"
                12:00:00,Job A,START,1001
                12:11:00,Job A,END,1001";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            var model = processor.ProcessLogFile(filePath);
            var acutalMessage = model[0].DurationMessage;

            // Assert
            Assert.Contains(acutalMessage, expectedMessage);
        }

        // TEST 3
        [Fact]
        public void ProcessLogFile_ShouldNotLog_For_Good_Job()
        {
            // Arrange
            var mockLogger = new MockJobDurationLogger();
            var processor = new LogProcessor(mockLogger);

            var logContent = @"
                12:00:00,Job C,START,3003
                12:03:00,Job C,END,3003";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            var model = processor.ProcessLogFile(filePath);

            // Assert
            Assert.Empty(model);
        }

        // TEST 4 - 
        [Fact]
        public void ProcessLogFile_ShouldLogWarning_For_No_End()
        {
            // Arrange
            var mockLogger = new MockJobDurationLogger();
            var processor = new LogProcessor(mockLogger);
            var expectedMessage = "WARNING: Job 3003 started but did not finish";

            var logContent = @"
                12:00:00,Job C,START,3003";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            var model = processor.ProcessLogFile(filePath);
            var acutalMessage = model[0].DurationMessage;

            // Assert
            Assert.Contains(acutalMessage, expectedMessage);

        }

        private static string WriteLogToTempFile(string content)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, content.Trim());
            return tempFile;
        }
    }

}
