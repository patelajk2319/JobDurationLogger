using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
    
    public class JobDurationLogger : IJobDurationLogger
    {
        public void LogDuration(string jobid, double durationSeconds)
        {
            var minutes = (int)(durationSeconds / 60);
            var seconds = (int)(durationSeconds % 60);

            var durationText = $"{minutes}min {seconds}sec";

            if (durationSeconds > 600) // 10 minutes
            {
                Console.WriteLine($"ERROR: Job {jobid} took {durationText}");
            }
            else if (durationSeconds > 300) // 5 minutes
            {
                Console.WriteLine($"WARNING: Job {jobid} took {durationText}");
            }
        }
    }