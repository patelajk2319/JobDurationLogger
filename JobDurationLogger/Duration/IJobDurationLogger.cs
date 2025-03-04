using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
    
    public interface IJobDurationLogger
    {
        void LogDuration(string jobid, double durationSeconds);
    }