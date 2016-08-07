using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Grader.Executor
{
    public class ExecutionResult
    {
        public bool Finished { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public string ErrorMessage { get; set; }
        public bool UsedImproperLibraries { get; set; }
        public bool TimeOut { get; set; }
        public bool FileImproper { get; set; }

        public string OutputFile { get; set; }
    }
}
