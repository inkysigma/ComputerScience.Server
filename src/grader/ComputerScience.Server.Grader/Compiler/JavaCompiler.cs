using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ComputerScience.Server.Common;

namespace ComputerScience.Server.Grader.Compiler
{
    public class JavaCompiler : ICompiler
    {

        public CompilerResult Compile(Solution solution, string directory)
        {
            var path = Path.Combine(directory, solution.FileLocation, solution.File);
            throw new NotImplementedException();
        }
    }
}
