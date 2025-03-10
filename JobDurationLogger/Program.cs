using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
    public class Program
    {
        static void Main()
        {
            const string filePath = "files/logs.csv"; // Update this path as needed
            ILogProcessor logProcessor = new LogProcessor();

            //check if file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }
            // process the logs
            
            var errors = logProcessor.ProcessLogFile(filePath);

            foreach (var error in errors) {
                Console.WriteLine(error.DurationMessage);
            }
        }
    }