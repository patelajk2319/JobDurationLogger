   namespace JobDurationLogger.Tests
{
    public class MockJobDurationLogger : IJobDurationLogger
    {
        public List<string> LoggedMessages { get; } = new List<string>();

        //Reimplent as Logduration logs to console, we need own version here so that we can check logs locally via the test project
        //seems messy to me so needs looking at...    
        public bool TookLongerThanExpected(double durationSeconds, int MinMinutes, int MaxMinutes) {

            var MinMinutesSeconds = (int)(MinMinutes * 60);
            var MaxMinutesSeconds = (int)(MaxMinutes * 60);

            if (durationSeconds > MinMinutesSeconds && durationSeconds < MaxMinutesSeconds) 
            {
                return true;
            }

            return false;

        }
        }
    }
