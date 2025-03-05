using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
    
    public class JobDurationLogger : IJobDurationLogger
    {

        // Checks to see if a job took longer than expected
        public bool TookLongerThanExpected(double durationSeconds, int MinMinutes, int MaxMinutes) {

            var MinMinutesSeconds = (int)(MinMinutes * 60); // Convert to seconds
            var MaxMinutesSeconds = (int)(MaxMinutes * 60);  // Convert to seconds

            if (durationSeconds > MinMinutesSeconds && durationSeconds < MaxMinutesSeconds) 
            {
                return true;
            }

            return false;

        }
    }