using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class LogProcessor : ILogProcessor
    {
        private readonly IJobDurationLogger _logger;

        public LogProcessor(IJobDurationLogger logger)
        {
            _logger = logger;
        }

        public List<ErrorModel> ProcessLogFile(string filePath)
        {
            var jobs = new Dictionary<string, DateTime>();
            var errorModel = new List<ErrorModel>();

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
                        var morethan5Mins = _logger.TookLongerThanExpected(durationSeconds, 5, 10);
                        var morethan10Mins = _logger.TookLongerThanExpected(durationSeconds, 10, 100);
                        var lessthaan5Mins =  _logger.TookLongerThanExpected(durationSeconds, 0, 5);

                        //Check jobs more 5 min , less than 10, 
                        if (morethan5Mins) {
                            errorModel.Add(new ErrorModel{ Id= jobid, DurationMessage = $"WARNING: Job {jobid} took more than 5 minutes"});
                            jobs.Remove(jobid);
                        } 
                          //Check jobs more 10 min, 
                        else if (morethan10Mins) {
                            errorModel.Add(new ErrorModel{ Id= jobid, DurationMessage = $"ERROR: Job {jobid} took more than 10 minutes"});    
                            jobs.Remove(jobid);
                        }
                          //Check jobs less 5 min, 
                        else if (lessthaan5Mins) {
                             jobs.Remove(jobid);
                        }
                }
            }         

        }
        // check the dictionary to see if there is any data in it - these will be be jobs that started but never ended - log these to console
            if (jobs.Count > 0) {
                 
                foreach (var job in jobs)
                {
                    errorModel.Add(new ErrorModel{ Id= job.Key, DurationMessage = $"WARNING: Job {job.Key} started but did not finish"});
                }
            }

        return errorModel;
    }
}