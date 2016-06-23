using ComputerScience.Server.Common;

namespace ComputerScience.Server.Grader.Compiler
{
    public interface ICompiler
    {
        CompilerResult Compile(Solution solution, string directory);
    }
}