using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ComputerScience.Server.Common;

namespace ComputerScience.Server.Grader.Compiler
{
    public class PythonCompiler : ICompiler
    {
        public CompilerResult Compile(Solution solution, string directory)
        {
            if (!File.Exists(Path.Combine(solution.FileLocation, solution.File)))
                CompilerResult.Fail("Something has gone wrong.Please notify the site administrators.");
            File.Copy(Path.Combine(solution.FileLocation, solution.File), Path.Combine(directory, solution.File));
            return CompilerResult.Succeed();
        }
    }
}
