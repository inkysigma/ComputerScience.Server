using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Common
{
    public class Result
    {
        public string Id { get; set; }
        public TestCase[] TestCases { get; set; }
        public long TimeStamp { get; set; }
    }

    public enum TestCase
    {
        TimeOut,
        Error,
        Failure,
        Success
    }
}
