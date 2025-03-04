# JobDuration

This project is a C# console application that processes log files containing job start and end events. It calculates the duration of each job and logs:

* A warning if a job takes longer than 5 minutes
* An error if a job takes longer than 10 minutes

## Technologies Used

* Language: C#
* Framework: .NET Core
* Testing: xUnit

## How it Works

Input Log Format

The application expects a CSV-style log file, where each line looks like this:

Time, Job Description, Event Type, Job ID

Example log:

```bash
11:35:23,job1,START,37980
11:35:56,job2,END,37980
```
Processing Logic

- Reads the log file line by line.
- Tracks start times for each Job ID.
- When an END event is found, it calculates the duration.
- Based on the duration:
  - Logs Warning if > 5 minutes
  - Logs Error if > 10 minutes

A warning is shown if a job has an END event but no matching START.

Jobs that never ended (only START found) are also reported at the end of processing.

## Running the Application
1. Clone the repository.
2. Place your log file as logs.csv in the files folder in the JobDurationLogger project directory. 
3. Run the application via
```bash
dotnet run
```
## Running the Unit Tests
1. On the terminal navigate to the folder of JobDurationLogger.Tests and run:

```bash
dotnet test
```
## Future Improvements
1. Write logs to file or any location instead of logging to Console
2. Multi Threading support to handle large files
3. SOLID Principles
