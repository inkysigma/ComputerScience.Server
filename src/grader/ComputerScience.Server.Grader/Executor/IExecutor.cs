using System;

namespace ComputerScience.Server.Grader.Executor
{
    public interface IExecutor
    {
        /// <summary>
        /// Executes a file based on a given directory. All preparartions should be made before execution
        /// </summary>
        /// <param name="directory">The directory of the file</param>
        /// <param name="file">The file to run</param>
        /// <param name="timeLimit">The time limit of running the file.</param>
        /// <returns>The result of execution</returns>
        ExecutionResult Run(string directory, string file, int timeLimit);
    }
}