using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Common
{
    public class TestResult
    {
        public int Iteration { get; set; }
        public TestCase Result { get; set; }
        public TimeSpan ExecutionTime { get; set; }
    }

    public enum TestCase
    {
        TimeOut,
        Error,
        Failure,
        Success
    }
}
