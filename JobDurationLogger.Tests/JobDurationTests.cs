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

            string logContent = @"
                12:00:00,Job A,START,1001
                12:06:00,Job A,END,1001";

            string filePath = WriteLogToTempFile(logContent);

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

            string logContent = @"
                12:00:00,Job B,START,2002
                12:11:00,Job B,END,2002";

            string filePath = WriteLogToTempFile(logContent);

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

            string logContent = @"
                12:00:00,Job C,START,3003
                12:03:00,Job C,END,3003";

            string filePath = WriteLogToTempFile(logContent);

            // Act
            processor.ProcessLogFile(filePath);

            // Assert
            Assert.Empty(mockLogger.LoggedMessages);  
        }


        // TEST 4 - PROBMR
        [Fact]
        public void ProcessLogFile_ShouldLogWarning_For_No_End()
        {
            // Arrange
            var mockLogger = new MockJobDurationLogger();
            var processor = new LogProcessor(mockLogger);

            string logContent = @"
                12:00:00,Job C,START,3003";

            string filePath = WriteLogToTempFile(logContent);

            // Act
            Assert.Contains("Warning: Job 3003 started at 12:00:00 but no END found", mockLogger.LoggedMessages);

        }

        private string WriteLogToTempFile(string content)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, content.Trim());
            return tempFile;
        }
    }

    public class MockJobDurationLogger : IJobDurationLogger
    {
        public List<string> LoggedMessages { get; } = new List<string>();

        //Reimplent as Logduration logs to console, we need own version here so that we can check logs locally via the test project
        //seems messy to me so needs looking at...    
        public void LogDuration(string jobid, double durationSeconds)
        {
            
            int minutes = (int)(durationSeconds / 60);
            int seconds = (int)(durationSeconds % 60);
            string durationText = $"{minutes}Min and {seconds}Sec";

            if (durationSeconds > 600)
            {
                LoggedMessages.Add($"ERROR: Job {jobid} took {durationText}");
            }
            else if (durationSeconds > 300)
            {
                LoggedMessages.Add($"WARNING: Job {jobid} took {durationText}");
            }
        }
    }
}
