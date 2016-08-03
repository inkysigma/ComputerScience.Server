using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Grader.Executor
{
    public class CppExecutor : IExecutor
    {
        public string Root { get; set; }
        public CppExecutor(string root)
        {
            Root = root;
        }

        public ExecutionResult Run(string path)
        {
            var process = Process.Start("mbox", $"-r {Root} -n -i -- {path}");

            throw new NotImplementedException();
        }
    }
}
