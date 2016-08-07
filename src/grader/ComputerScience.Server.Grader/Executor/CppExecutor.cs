using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ComputerScience.Server.Grader.Executor
{
    public class CppExecutor : IExecutor
    {
        public string Root { get; set; }
        public List<string> BannedCalls { get; set; }
        public int MaxLength { get; set; }
        public CppExecutor(string root, int maxLength, List<string> bannedCalls)
        {
            Root = root;
            MaxLength = maxLength;
            BannedCalls = bannedCalls;
        }

        public ExecutionResult Run(string directory, string file, int timeLimit)
        {
            var process = Process.Start("mbox", $"-r {Root} -S calls -n -i -c -- {Path.Combine(directory, file)}");
            Task.Delay(timeLimit);
            string message = null;
            if (!process.HasExited)
                return new ExecutionResult
                {
                    Finished = false,
                    TimeOut = true
                };
            
            using (var reader = process.StandardOutput)
            {
                string init = reader.ReadLine();
                if (!init.Contains("syscall"))
                    message = init.Substring(0, MaxLength);
                if (!process.StandardError.EndOfStream)
                    return new ExecutionResult
                    {
                        Finished = false,
                        ErrorMessage = message
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
                                UsedImproperLibraries = true,
                                ErrorMessage = message
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
                    Finished = true,
                    FileImproper = true
                };
            }
            return new ExecutionResult
            {
                Finished = true,
                OutputFile = Directory.GetFiles(currentDirectory)[0];
            };
        }
    }
}
