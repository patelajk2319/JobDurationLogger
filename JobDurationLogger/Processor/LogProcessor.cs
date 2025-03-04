using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class LogProcessor
    {
        private readonly IJobDurationLogger _logger;

        public LogProcessor(IJobDurationLogger logger)
        {
            _logger = logger;
        }

        public void ProcessLogFile(string filePath)
        {
            var jobs = new Dictionary<string, DateTime>();

            // Read all the lines into memory
            var lines = File.ReadAllLines(filePath);

            // Loop through each item in the file
            foreach (var line in lines)
            {
                var columns = line.Split(',');

                // we are expecting 4 columns for each row in the csv, log it if the number of columns <> 4
                if (columns.Length != 4)
                {
                    Console.WriteLine($"Invalid log line: {line}");
                    continue;
                }

                var time = DateTime.ParseExact(columns[0].Trim(), "HH:mm:ss", CultureInfo.InvariantCulture); // Grab the time we have for the job
                var jobid = columns[3].Trim(); // Grab the jobid and remove any traling spaces.
                var eventType = columns[2].Trim().ToUpperInvariant(); // grab the eventtype and convert to uppercase for better consistency

                if (eventType == "START")
                {
                    jobs[jobid] = time;
                }
                else if (eventType == "END")
                {
                    // check to see if there is a corresponding "End" event     
                    if (jobs.TryGetValue(jobid, out var startTime))
                    {
                        // if "End" event found - calculate the duration    
                        var durationSeconds = (time - startTime).TotalSeconds;
                        // send to the logge
                        _logger.LogDuration(jobid, durationSeconds);

                        // remove the jobid from the dictionary as we have processed it and no longer need it
                        jobs.Remove(jobid);
                    }
                }
            }
            // check the dictionary to see if there is any data in it - these will be be jobs that started but never ended - log these to console
            _logger.HandleMissingEntries(jobs);
        }
    }