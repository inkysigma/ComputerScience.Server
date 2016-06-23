namespace ComputerScience.Server.Common
{
    public class Problem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ProblemStatement { get; set; }

        public long SolutionSize { get; set; }

        public int TestCases { get; set; }

        public string ProblemPath { get; set; }
    }
}
