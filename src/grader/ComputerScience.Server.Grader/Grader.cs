using System;
using System.Collections.Generic;
using System.IO;
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
        public IResultSetService<Result> ResultService { get; }

        public Dictionary<SolutionType, ICompiler> Compilers { get; }
        public Dictionary<SolutionType, IExecutor> Executors { get; }

        public string Directory { get; }

        public int TimeLimit { get; }

        private ILogger<Grader> Logger { get; }

        public Grader(ISolutionCache<Solution> solutionCache, 
            IProblemSet<Problem> problemSet,
            IResultSetService<Result> resultService,
            Dictionary<SolutionType, ICompiler> compilers, 
            Dictionary<SolutionType, IExecutor> executors,
            ILogger<Grader> logger,
            string directory)
        {
            SolutionCache = solutionCache;
            ProblemSet = problemSet;
            ResultService = resultService;
            Compilers = compilers;
            Executors = executors;
            Directory = directory;
            Logger = logger;
        }
        
        public void Start()
        {
            while (Program.IsRunning)
            {
                try
                {
                    var solution = Task.Run(async () => await SolutionCache.Fetch()).Result;
                    if (solution == null)
                    {
                        Task.Delay(1000).Wait();
                        continue;
                    }
                    if (!Compilers.ContainsKey(solution.SolutionType))
                        throw new NotSupportedException();
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
                        Task.Run(async () => await ResultService.AddResultAsync(compileResult, CancellationToken.None))
                            .Wait();
                        continue;
                    }
                    var problem = ProblemSet.FetchProblemAsync(solution.ProblemId, CancellationToken.None).Result;
                    if (problem == null)
                    {
                        throw new ProblemNotFoundException(solution.ProblemId, solution.Id);
                    }
                    var info = new FileInfo(Path.Combine(solution.FileLocation, solution.File));
                    if (info.Length > problem.SolutionSize)
                    {
                        
                    }
                    int counter = 0;
                    Result gradedResult = null;
                    var testCases = new List<TestResult>();
                    while (counter < problem.TestCases)
                    {
                        File.Copy(Path.Combine(problem.ProblemPath, $"{counter}.in"), 
                            Path.Combine(Directory, $"{problem.NormalizedTitle}.in"), true);
                        var execution = Executors[solution.SolutionType].Run(Directory, result.FilePath, problem.TimeLimit);
                        if (!execution.Finished)
                        {
                            var message = string.IsNullOrEmpty(execution.TrimmedOutput)
                                ? execution.ErrorMessage
                                : execution.TrimmedOutput + "\n" + execution.ErrorMessage;
                            testCases.Add(new TestResult
                            {
                                Iteration = counter + 1,
                                ExecutionTime = execution.TimeSpan,
                                Result = execution.TestCase
                            });
                            gradedResult = new Result
                            {
                                Id = solution.Id,
                                TestCases = testCases,
                                Error = message,
                                TimeStamp = DateTime.UtcNow
                            };
                            break;
                        }
                        testCases.Add(new TestResult
                        {
                            Iteration = counter + 1,
                            ExecutionTime = execution.TimeSpan,
                            Result = execution.TestCase
                        });
                        File.Delete(execution.OutputFile);
                    }
                    if (gradedResult == null)
                    {
                        gradedResult = new Result
                        {
                            Id = solution.Id,
                            TestCases = testCases,
                            TimeStamp = DateTime.Now
                        };
                    }
                    ResultService.AddResultAsync(gradedResult, CancellationToken.None).RunSynchronously();
                }
                catch (ProblemNotFoundException e)
                {
                    Logger.LogError(e.HResult, e, e.Message);
                    Task.Run(async () => await ResultService.AddResultAsync(new Result
                    {
                        Id = e.SolutionId,
                        TestCases = null,
                        Error = e.Message,
                        TimeStamp = DateTime.UtcNow
                    }, CancellationToken.None)).Wait();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.HResult, e, e.Message);
                }
            }
        }
    }
}
