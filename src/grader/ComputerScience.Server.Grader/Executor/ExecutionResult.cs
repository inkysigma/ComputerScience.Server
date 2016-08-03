using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Grader.Executor
{
    public class ExecutionResult
    {
        public TimeSpan TimeSpan { get; set; }
        public bool IsCorrect { get; set; }
        public bool TimeOut { get; set; }
    }
}
