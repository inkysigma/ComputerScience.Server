using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;
using ComputerScience.Server.Grader.Compiler;
using ComputerScience.Server.Grader.Data;
using ComputerScience.Server.Grader.Executor;
using Microsoft.Extensions.Logging;

namespace ComputerScience.Server.Grader
{
    public class Grader
    {
        public ISolutionCache<Solution> SolutionCache { get; }
        public IProblemSet<Problem> ProblemSet { get; }
        public IResultSetService ResultService { get; }

        public Dictionary<SolutionType, ICompiler> Compilers { get; }
        public Dictionary<SolutionType, IExecutor> Executors { get; }

        public string Directory { get; }

        public int TimeLimit { get; }

        private ILogger<Grader> Logger { get; }

        public Grader(ISolutionCache<Solution> solutionCache, 
            IProblemSet<Problem> problemSet,
            IResultSetService resultService,
            Dictionary<SolutionType, ICompiler> compilers, 
            Dictionary<SolutionType, IExecutor> executors,
            ILogger<Grader> logger,
            string directory,
            int limit)
        {
            SolutionCache = solutionCache;
            ProblemSet = problemSet;
            ResultService = resultService;
            Compilers = compilers;
            Executors = executors;
            Directory = directory;
            Logger = logger;
            TimeLimit = limit;
        }
        
        public void Start()
        {
            while (Program.IsRunning)
            {
                try
                {
                    var solution = Task.Run(async () => await SolutionCache.Fetch()).Result;
                    var result = Compilers[solution.SolutionType].Compile(solution, Directory);
                    if (!result.Succeeded)
                    {
                        var compileResult = new Result
                        {
                            Id = solution.Id,
                            TestCases = null,
                            Error = result.Message,
                            TimeStamp = DateTime.UtcNow
                        };
                        Task.Run(async () => await ResultService.AddResultAsync(compileResult, CancellationToken.None)).Wait();
                        continue;
                    }

                    var executionResult = Executors[solution.SolutionType].Run(Directory, result.FilePath, TimeLimit);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.HResult, e, e.Message);
                }
            }
        }
    }
}
