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

            var logContent = @"
                12:00:00,Job A,START,1001
                12:06:00,Job A,END,1001";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            processor.ProcessLogFile(filePath);

            // Assert
            Assert.Contains("WARNING: Job 1001 took 6Min and 0Sec", mockLogger.LoggedMessages);
        }

        // TEST 2
        [Fact]
        public void ProcessLogFile_ShouldLogError_ForJobExceeding_10_Minutes()
        {
            // Arrange
            var mockLogger = new MockJobDurationLogger();
            var processor = new LogProcessor(mockLogger);

            var logContent = @"
                12:00:00,Job B,START,2002
                12:11:00,Job B,END,2002";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            processor.ProcessLogFile(filePath);

            // Assert
            Assert.Contains("ERROR: Job 2002 took 11Min and 0Sec", mockLogger.LoggedMessages);
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
            processor.ProcessLogFile(filePath);

            // Assert
            Assert.Empty(mockLogger.LoggedMessages);  
        }

        // TEST 4 - 
        [Fact]
        public void ProcessLogFile_ShouldLogWarning_For_No_End()
        {
            // Arrange
            var mockLogger = new MockJobDurationLogger();
            var processor = new LogProcessor(mockLogger);

            var logContent = @"
                12:00:00,Job C,START,3003";

            var filePath = WriteLogToTempFile(logContent);

            // Act
            processor.ProcessLogFile(filePath);

            // Assert
            Assert.Contains("Warning: Job 3003 did not complete", mockLogger.LoggedMessages);

        }

        private static string WriteLogToTempFile(string content)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, content.Trim());
            return tempFile;
        }
    }

}
