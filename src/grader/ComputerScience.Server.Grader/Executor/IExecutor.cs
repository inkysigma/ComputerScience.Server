using System;

namespace ComputerScience.Server.Grader.Executor
{
    public interface IExecutor
    {
        ExecutionResult Run(string path);
    }
}