using ComputerScience.Server.Common;

namespace ComputerScience.Server.Grader.Compiler
{
    /// <summary>
    /// Compiles a given solution in a directory. Should copy the file from the source
    /// to the directory
    /// </summary>
    public interface ICompiler
    {
        /// <summary>
        /// Compiles the solution to be executed
        /// </summary>
        /// <param name="solution">The solution to be compiled</param>
        /// <param name="directory">The copied directory</param>
        /// <returns>A CompilerResult that represents the errors during the operation</returns>
        CompilerResult Compile(Solution solution, string directory);
    }
}