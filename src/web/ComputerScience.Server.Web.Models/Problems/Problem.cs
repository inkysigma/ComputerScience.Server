namespace ComputerScience.Server.Web.Models.Problems
{
    public class Problem
    {
        public string Guid { get; set; }

        public string Title { get; set; }

        public string ProblemStatement { get; set; }

        public long SolutionSize { get; set; }

        public string ProblemPath { get; set; }

        public string ProblemFile { get; set; }
    }
}
