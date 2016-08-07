using Newtonsoft.Json;

namespace ComputerScience.Server.Common
{
    public class Problem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ProblemStatement { get; set; }

        public long SolutionSize { get; set; }

        [JsonIgnore]
        public int TestCases { get; set; }

        [JsonIgnore]
        public string ProblemPath { get; set; }
    }
}
