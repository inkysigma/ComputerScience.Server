using ComputerScience.Server.Common;
using ComputerScience.Server.Grader.Data;

namespace ComputerScience.Server.Grader
{
    public class Grader
    {
        public ISolutionCache<Solution> SolutionCache { get; }
        public IProblemSet<Problem> ProblemSet { get; }

        public string Directory { get; }

        public Grader(ISolutionCache<Solution> solutionCache, IProblemSet<Problem> problemSet, string directory)
        {
            SolutionCache = solutionCache;
            ProblemSet = problemSet;
            Directory = directory;
        }
        
        public async void Start()
        {
            while (Program.IsRunning)
            {
                var solution = await SolutionCache.Fetch();
                
            }
        }
    }
}
