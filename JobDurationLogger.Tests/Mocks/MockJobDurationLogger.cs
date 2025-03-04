   namespace JobDurationLogger.Tests
{
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
                LoggedMessages.Add($"ERROR: Job {jobid} took {durationText}".Trim());
            }
            else if (durationSeconds > 300)
            {
                LoggedMessages.Add($"WARNING: Job {jobid} took {durationText}".Trim());
            }
        }
    public void HandleMissingEntries(Dictionary<string, DateTime> entries) {
    
        if (entries.Count > 0)
            {
                //Console.WriteLine("Warning: Some jobs never ended:");
                foreach (var entry in entries)
                {
                     LoggedMessages.Add($"Warning: Job {entry.Key} did not complete");
                }
            }
        }
    }
}