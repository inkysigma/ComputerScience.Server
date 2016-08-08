using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScience.Server.Common;

namespace ComputerScience.Server.Grader.Executor
{
    public class ExecutionResult
    {
        public bool Finished { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public string ErrorMessage { get; set; }
        public string TrimmedOutput { get; set; }
        
        public TestCase TestCase { get; set; }

        public string OutputFile { get; set; }
    }
}
