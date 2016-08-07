using System;
using System.Diagnostics;
using System.IO;
using ComputerScience.Server.Common;

namespace ComputerScience.Server.Grader.Compiler
{
    public class CppCompiler : ICompiler
    {
        public CompilerResult Compile(Solution solution, string directory)
        {
            if (string.IsNullOrEmpty(solution.FileLocation))
                throw new ArgumentNullException(nameof(solution.FileLocation));
            if (string.IsNullOrEmpty(solution.File))
                throw new ArgumentNullException(nameof(solution.File));
            var info = new DirectoryInfo(directory);
            foreach (var file in info.EnumerateFiles())
            {
                file.Delete();
            }
            if (!File.Exists(Path.Combine(solution.FileLocation, solution.File)))
            {
                if (Directory.Exists(solution.FileLocation))
                    Directory.Delete(solution.FileLocation, true);
                return CompilerResult.Fail("Something has gone wrong. Please notify the site administrators.");
            }
            File.Copy(Path.Combine(solution.FileLocation, solution.File), Path.Combine(directory, solution.File));
            Directory.Delete(solution.FileLocation);
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "exec",
                Arguments = $"g++ -o a.exe {solution.File}",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });
            process.WaitForExit();
            var line = process.StandardOutput.ReadLine();
            return !string.IsNullOrEmpty(line) ? CompilerResult.Fail(line + "\r\n" + process.StandardOutput.ReadToEnd()) 
                : CompilerResult.Succeed("a.exe");
        }
    }
}
