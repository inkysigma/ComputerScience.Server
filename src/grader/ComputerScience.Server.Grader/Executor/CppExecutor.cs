using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;

namespace ComputerScience.Server.Grader.Executor
{
    public class CppExecutor : IExecutor
    {
        public string Root { get; set; }
        public List<string> BannedCalls { get; set; }
        public int MaxLength { get; set; }
        public int Milliseconds { get; set; } = 0;
        public CppExecutor(string root, int maxLength, List<string> bannedCalls)
        {
            Root = root;
            MaxLength = maxLength;
            BannedCalls = bannedCalls;
        }

        public ExecutionResult Run(string directory, string file, int timeLimit)
        {
            var process = Process.Start("mbox", $"-r {Root} -S calls -n -i -c -- {Path.Combine(directory, file)}");
            var timer = new Timer(TrackTime, null, 0, 1);
            while (Milliseconds < timeLimit)
            {
            }
            timer.Dispose();
            string message = null;
            if (!process.HasExited)
            {
                process.Kill();
                return new ExecutionResult
                {
                    Finished = false,
                    TestCase = TestCase.TimeOut,
                    ErrorMessage = "Your program timed out"
                };
            }

            using (var reader = process.StandardOutput)
            {
                string init = reader.ReadLine();
                if (!init.Contains("syscall"))
                    message = init.Substring(0, MaxLength);
                if (!process.StandardError.EndOfStream)
                    return new ExecutionResult
                    {
                        Finished = false,
                        ErrorMessage = "A runtime error was encountered.",
                        TrimmedOutput = message
                    };
                while (!reader.EndOfStream)
                {
                    string input = reader.ReadLine();
                    if (input.StartsWith("-"))
                        continue;
                    foreach (string call in BannedCalls)
                        if (input.Contains(call))
                            return new ExecutionResult
                            {
                                Finished = false,
                                TestCase = TestCase.Error,
                                ErrorMessage = "Something banned was used. Did you try to create threads?",
                                TrimmedOutput = message
                            };
                }
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var directories = Directory.GetDirectories(currentDirectory);
            int counter = 0;
            while (counter < 10 && directories.Length > 0)
            {
                directories = Directory.GetDirectories(directories[0]);
                currentDirectory = directories[0];
            }
            if (Directory.GetFiles(currentDirectory).Length != 1)
            {
                return new ExecutionResult
                {
                    Finished = false,
                    TestCase = TestCase.Error,
                    ErrorMessage = "The output file was not created."
                };
            }
            return new ExecutionResult
            {
                Finished = true,
                TestCase = TestCase.Success,
                TimeSpan = TimeSpan.FromMilliseconds(Milliseconds),
                OutputFile = Directory.GetFiles(currentDirectory)[0]
            };
        }

        public void TrackTime(object state)
        {
            Milliseconds++;
        }
    }
}
