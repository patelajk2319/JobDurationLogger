using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
    public class Program
    {
        static void Main()
        {
            const string filePath = "files/logs.csv"; // Update this path as needed

            //check if file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            // process the logs
            var processor = new LogProcessor(new JobDurationLogger());
            processor.ProcessLogFile(filePath);
        }
    }