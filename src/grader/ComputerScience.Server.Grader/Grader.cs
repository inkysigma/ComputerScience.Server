using System.Collections.Generic;
using ComputerScience.Server.Common;
using ComputerScience.Server.Grader.Compiler;
using ComputerScience.Server.Grader.Data;

namespace ComputerScience.Server.Grader
{
    public class Grader
    {
        public ISolutionCache<Solution> SolutionCache { get; }
        public IProblemSet<Problem> ProblemSet { get; }

        public Dictionary<SolutionType, ICompiler> Compilers { get; }

        public string Directory { get; }

        public Grader(ISolutionCache<Solution> solutionCache, 
            IProblemSet<Problem> problemSet, 
            Dictionary<SolutionType, ICompiler> compilers, 
            string directory)
        {
            SolutionCache = solutionCache;
            ProblemSet = problemSet;
            Compilers = compilers;
            Directory = directory;
        }
        
        public async void Start()
        {
            while (Program.IsRunning)
            {
                var solution = await SolutionCache.Fetch();
                Compilers[solution.SolutionType].Compile(solution, Directory);
            }
        }
    }
}
