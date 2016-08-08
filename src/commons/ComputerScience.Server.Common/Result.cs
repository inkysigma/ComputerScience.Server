using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Common
{
    public class Result
    {
        public string Id { get; set; }
        public IEnumerable<TestResult> TestCases { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Error { get; set; }
    }
}
