using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Grader
{
    public class ProblemNotFoundException : Exception
    {
        public string Id { get; }
        public string SolutionId { get; }
        public ProblemNotFoundException(string id, string solutionId) 
            : base("The problem requested was not found. This is likely a server error. Please contact the administrators.")
        {
            Id = id;
            SolutionId = solutionId;
        }
    }
}
